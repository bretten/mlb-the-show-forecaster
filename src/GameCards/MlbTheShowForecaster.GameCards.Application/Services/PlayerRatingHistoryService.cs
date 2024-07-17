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
using Microsoft.Extensions.Logging;

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
    /// Logger
    /// </summary>
    private readonly ILogger<IPlayerRatingHistoryService> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rosterUpdateFeed">Keeps tracks of roster updates that need to be applied</param>
    /// <param name="cardCatalog">The external source of player cards</param>
    /// <param name="querySender">Sends queries to retrieve state from the system</param>
    /// <param name="commandSender">Sends commands to mutate the system</param>
    /// <param name="logger">Logger</param>
    public PlayerRatingHistoryService(IRosterUpdateFeed rosterUpdateFeed, ICardCatalog cardCatalog,
        IQuerySender querySender, ICommandSender commandSender, ILogger<IPlayerRatingHistoryService> logger)
    {
        _rosterUpdateFeed = rosterUpdateFeed;
        _cardCatalog = cardCatalog;
        _querySender = querySender;
        _commandSender = commandSender;
        _logger = logger;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public async Task<PlayerRatingHistoryResult> SyncHistory(SeasonYear seasonYear,
        CancellationToken cancellationToken = default)
    {
        // Get a list of all available roster updates from the external source
        var rosterUpdates = await _rosterUpdateFeed.GetNewRosterUpdates(seasonYear, cancellationToken);

        // Sync historical ratings for all player card rating changes in the roster updates
        var results = new List<PlayerCard>();
        var tasks = SyncHistoryByPlayerCard(rosterUpdates.OldToNew, seasonYear, cancellationToken);
        foreach (var task in tasks)
        {
            try
            {
                var result = await task;
                if (result != null) results.Add(result);
            }
            catch (NoPlayerCardFoundForRosterUpdateException e)
            {
                _logger.LogWarning(e, e.Message);
            }
        }

        return new PlayerRatingHistoryResult(results);
    }

    /// <summary>
    /// Gets the tasks for syncing historical ratings for all player card rating changes in the roster updates
    /// </summary>
    /// <param name="rosterUpdates">All available roster updates from the external source</param>
    /// <param name="seasonYear">The season to sync rating changes for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>A collection of Tasks that will sync the historical ratings</returns>
    private IEnumerable<Task<PlayerCard?>> SyncHistoryByPlayerCard(IEnumerable<RosterUpdate> rosterUpdates,
        SeasonYear seasonYear,
        CancellationToken cancellationToken)
    {
        // Group rating changes by the card's external ID
        var ratingChanges = rosterUpdates.SelectMany(x => x.RatingChanges)
            .GroupBy(x => x.CardExternalId);

        // Add all historical ratings to each player card
        foreach (var ratingChangeByCardExternalId in ratingChanges)
        {
            yield return AddHistoricalRatingsForPlayerCard(seasonYear, ratingChangeByCardExternalId.Key,
                ratingChangeByCardExternalId.ToImmutableList(), cancellationToken);
        }
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

        // If the card is boosted, the history can't accurately be recreated due to its temporary stats
        if (playerCard.IsBoosted)
        {
            // The service runs regularly, so let it build the history later
            // Can be logged #171
            return null;
        }

        // Get the current state of the player card from the external card catalog
        var externalCard = await _cardCatalog.GetMlbPlayerCard(seasonYear, cardExternalId, cancellationToken);

        // Only the current overall rating and attributes are known from the external card catalog
        // So the history has to be rebuilt starting with the most recent rating change
        changes = changes.OrderByDescending(x => x.Date).ToImmutableList();

        // Determines if the PlayerCard's historical ratings were updated
        var wasUpdated = false;

        // Get the current attributes. Each attribute change set will be subtracted from the current state, starting with the newest
        // This will rebuild the history
        var currentState = externalCard.GetAttributes();

        // Iterate through all rating changes from newest to oldest
        var lastRatingChangeIndex = changes.Count - 1;
        for (var i = 0; i < changes.Count; i++)
        {
            // Start with the current, recent state of the card and subtract each rating change in newest to oldest order
            // This gives the attribute state before each rating change
            currentState = changes[i].AttributeChanges.SubtractFrom(currentState);

            // A historical rating is being created for the state of the card BEFORE the current PlayerRatingChange (changes[i])
            // The last item is the oldest/first rating change. Its start date is considered to be the beginning of the year
            var startDate = i == lastRatingChangeIndex
                ? new DateOnly(seasonYear.Value, 1, 1)
                // The start date of any other rating change is the date of the next, older rating change (changes[i + 1])
                : changes[i + 1].Date;
            // The end date is the date of the current rating change (changes[i])
            var endDate = changes[i].Date;
            // Since this is the state of the card BEFORE the current PlayerRatingChange (changes[i]), use the old rating
            var rating = changes[i].OldRating;

            // Add the historical rating to the PlayerCard if it has no record of it
            var historicalRating = PlayerCardHistoricalRating.Baseline(startDate, endDate, rating, currentState);
            if (playerCard.IsRatingAppliedFor(historicalRating.StartDate)) continue;
            wasUpdated = true;
            playerCard.AddHistoricalRating(historicalRating);
        }

        // No new historical ratings, so no further action needed
        if (!wasUpdated)
        {
            return null;
        }

        // Add a rating for the most recent state
        var recentState = PlayerCardHistoricalRating.Baseline(changes[0].Date, null, changes[0].NewRating,
            externalCard.GetAttributes());
        if (!playerCard.IsRatingAppliedFor(recentState.StartDate)) playerCard.AddHistoricalRating(recentState);

        // Update the PlayerCard with the new historical ratings
        await _commandSender.Send(new UpdatePlayerCardCommand(playerCard, null, null, null), cancellationToken);
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