using System.Collections.Immutable;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetPlayerCardByExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Results;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;

/// <summary>
/// Updates all <see cref="PlayerCard"/>s with a complete history of <see cref="PlayerCardHistoricalRating"/>s by comparing
/// the external card catalog to the domain's history
/// </summary>
public sealed class PlayerRatingHistoryService : IPlayerRatingHistoryService
{
    /// <summary>
    /// Keeps tracks of roster updates that need to be applied
    /// </summary>
    private readonly IRosterUpdateFeed _rosterUpdateFeed;

    /// <summary>
    /// The external source of player cards
    /// </summary>
    private readonly ICardCatalog _cardCatalog;

    /// <summary>
    /// Sends queries to retrieve state from the system
    /// </summary>
    private readonly IQuerySender _querySender;

    /// <summary>
    /// Sends commands to mutate the system
    /// </summary>
    private readonly ICommandSender _commandSender;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rosterUpdateFeed">Keeps tracks of roster updates that need to be applied</param>
    /// <param name="cardCatalog">The external source of player cards</param>
    /// <param name="querySender">Sends queries to retrieve state from the system</param>
    /// <param name="commandSender">Sends commands to mutate the system</param>
    public PlayerRatingHistoryService(IRosterUpdateFeed rosterUpdateFeed, ICardCatalog cardCatalog,
        IQuerySender querySender, ICommandSender commandSender)
    {
        _rosterUpdateFeed = rosterUpdateFeed;
        _cardCatalog = cardCatalog;
        _querySender = querySender;
        _commandSender = commandSender;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public async Task<PlayerRatingHistoryResult> SyncHistory(SeasonYear seasonYear,
        CancellationToken cancellationToken = default)
    {
        // Get a list of all available roster updates from the external source
        var rosterUpdates = await _rosterUpdateFeed.GetNewRosterUpdates(seasonYear, cancellationToken);

        // Group rating changes by the card's external ID
        var ratingChanges = rosterUpdates.SelectMany(x => x.RatingChanges)
            .GroupBy(x => x.CardExternalId);

        // Will hold updated PlayerCards
        var updatedPlayerCards = new List<PlayerCard>();

        // Add all historical ratings for each player card
        foreach (var ratingChangeByCardExternalId in ratingChanges)
        {
            var result = await AddHistoricalRatingsForPlayerCard(seasonYear, ratingChangeByCardExternalId.Key,
                ratingChangeByCardExternalId.ToImmutableList(), cancellationToken);
            if (result != null) updatedPlayerCards.Add(result);
        }

        return new PlayerRatingHistoryResult(updatedPlayerCards);
    }

    /// <summary>
    /// Adds any missing <see cref="PlayerCardHistoricalRating"/>s for a <see cref="PlayerCard"/> by comparing
    /// the domain data to the external source <see cref="ICardCatalog"/>
    /// </summary>
    /// <param name="seasonYear">The season of the <see cref="PlayerCard"/></param>
    /// <param name="cardExternalId">The <see cref="CardExternalId"/> of the <see cref="PlayerCard"/></param>
    /// <param name="changes">All rating changes from the <see cref="ICardCatalog"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The updated <see cref="PlayerCard"/> or null if nothing was changed</returns>
    /// <exception cref="NoPlayerCardFoundForRosterUpdateException">Thrown if no <see cref="PlayerCard"/> could be found in the domain</exception>
    private async Task<PlayerCard?> AddHistoricalRatingsForPlayerCard(SeasonYear seasonYear,
        CardExternalId cardExternalId, IReadOnlyList<PlayerRatingChange> changes, CancellationToken cancellationToken)
    {
        // Get the corresponding player card
        var playerCard = await _querySender.Send(new GetPlayerCardByExternalIdQuery(cardExternalId), cancellationToken);
        if (playerCard == null)
        {
            throw new NoPlayerCardFoundForRosterUpdateException(
                $"Found a rating update for {cardExternalId}, but there is no corresponding PlayerCard");
        }

        // Get the current state of the player card from the external card catalog
        var externalCard = await _cardCatalog.GetMlbPlayerCard(seasonYear, cardExternalId, cancellationToken);

        // Only the current overall rating and attributes are known from the external card catalog
        // So the history has to be rebuilt starting with the most recent rating change
        changes = changes.OrderByDescending(x => x.Date).ToImmutableList();

        // Will hold all rating changes in the form of the domain equivalent
        var historicalRatings = new List<PlayerCardHistoricalRating>();

        // Get the current attributes. Each attribute change set will be subtracted from the current state, starting with the newest
        // This will rebuild the history
        var currentState = externalCard.GetAttributes();

        // Iterate through all rating changes from newest to oldest
        var lastRatingChangeIndex = changes.Count - 1;
        for (var i = 0; i < changes.Count; i++)
        {
            // Subtract the attribute changes from the current rating change so their previous values can be known at this point in the history
            currentState = changes[i].AttributeChanges.SubtractFrom(currentState);

            // The last item is the oldest/first rating change. Its start date is considered to be the beginning of the year
            if (i == lastRatingChangeIndex)
            {
                var startOfYear = new DateOnly(seasonYear.Value, 1, 1);
                historicalRatings.Add(PlayerCardHistoricalRating.Create(startOfYear, changes[i].Date,
                    changes[i].OldRating, currentState));
                continue;
            }

            // Add a historical rating for each rating change. The start date is the date of the next oldest rating change
            var isThereAnother = (i + 1) <= lastRatingChangeIndex;
            if (isThereAnother)
            {
                historicalRatings.Add(PlayerCardHistoricalRating.Create(changes[i + 1].Date, changes[i].Date,
                    changes[i].OldRating, currentState));
            }
        }

        // Add any historical ratings to the PlayerCard that have not yet been added
        var wasUpdated = false;
        foreach (var historicalRating in historicalRatings)
        {
            if (playerCard.IsRatingAppliedFor(historicalRating.StartDate)) continue;
            wasUpdated = true;
            playerCard.AddHistoricalRating(historicalRating);
        }

        // No new historical ratings, so no further action needed
        if (!wasUpdated)
        {
            return null;
        }

        // Update the PlayerCard
        var mostRecentRatingChange = changes[0];
        await _commandSender.Send(new UpdatePlayerCardCommand(playerCard, externalCard, mostRecentRatingChange, null),
            cancellationToken);
        return playerCard;
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        _rosterUpdateFeed.Dispose();
        _cardCatalog.Dispose();
    }
}