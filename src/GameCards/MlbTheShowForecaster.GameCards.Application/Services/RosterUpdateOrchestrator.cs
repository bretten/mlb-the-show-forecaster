using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetPlayerCardByExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;

/// <summary>
/// Service that applies roster updates to the domain
/// </summary>
public sealed class RosterUpdateOrchestrator : IRosterUpdateOrchestrator
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
    /// Applies roster updates to the domain
    /// </summary>
    /// <param name="seasonYear">The season to apply roster updates for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task SyncRosterUpdates(SeasonYear seasonYear, CancellationToken cancellationToken = default)
    {
        // Get a list of all available roster updates from the external source
        var rosterUpdates = await _rosterUpdateFeed.GetNewRosterUpdates(seasonYear, cancellationToken);

        foreach (var rosterUpdate in rosterUpdates)
        {
            await ApplyPlayerCardChanges(rosterUpdate.RatingChanges, rosterUpdate.PositionChanges,
                cancellationToken);
            await ApplyPlayerAdditions(seasonYear, rosterUpdate.NewPlayers, cancellationToken);

            // The roster update has been successfully applied, so close it
            await _rosterUpdateFeed.CompleteRosterUpdate(rosterUpdate, cancellationToken);
        }
    }

    /// <summary>
    /// Applies player rating changes and position changes to the domain
    /// </summary>
    /// <param name="ratingChanges">A collection of rating changes for different player cards</param>
    /// <param name="positionChanges">A collection of position changes for different player cards</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <exception cref="NoPlayerCardFoundForRosterUpdateException">Thrown if no <see cref="PlayerCard"/> could be found in the domain to apply the updates to</exception>
    private async Task ApplyPlayerCardChanges(IReadOnlyList<PlayerRatingChange> ratingChanges,
        IReadOnlyList<PlayerPositionChange> positionChanges, CancellationToken cancellationToken)
    {
        // Hash set is used to check if we already applied the position change in the rating change loop for a given player card
        var appliedPositionChangeCardExternalIds = new HashSet<CardExternalId>();

        // Apply rating changes to the domain
        foreach (var ratingChange in ratingChanges.OrderByDescending(x => x.Improved ? 1 : 0))
        {
            // Get the corresponding player card
            var playerCard = await _querySender.Send(new GetPlayerCardByExternalIdQuery(ratingChange.CardExternalId),
                cancellationToken);
            if (playerCard == null)
            {
                throw new NoPlayerCardFoundForRosterUpdateException(
                    $"Found a rating update for {ratingChange.CardExternalId}, but there is no corresponding PlayerCard");
            }

            // If the ratings have already been applied to the player card, no further action is required
            if (ratingChange.IsApplied(playerCard))
            {
                continue;
            }

            // To prevent querying the domain for the PlayerCard again, check if there is also a position change for this card
            var positionChange = positionChanges.Cast<PlayerPositionChange?>()
                .FirstOrDefault(x => x?.CardExternalId == ratingChange.CardExternalId);
            if (positionChange != null)
            {
                // There is both a rating change and a position change. Mark the position change as done by adding it to the hash set
                appliedPositionChangeCardExternalIds.Add(ratingChange.CardExternalId);
                // Update the domain PlayerCard with both the rating and position change
                await _commandSender.Send(new UpdatePlayerCardCommand(playerCard, ratingChange, positionChange),
                    cancellationToken);
                continue;
            }

            // There is only a rating change, so update the domain PlayerCard with the rating change
            await _commandSender.Send(new UpdatePlayerCardCommand(playerCard, ratingChange, null), cancellationToken);
        }

        // Now (after applying rating changes), apply pending position changes
        foreach (var positionChange in positionChanges)
        {
            // If we have already applied the position change during the rating change update, skip it
            if (appliedPositionChangeCardExternalIds.Contains(positionChange.CardExternalId))
            {
                continue;
            }

            // Get the corresponding player card
            var playerCard = await _querySender.Send(new GetPlayerCardByExternalIdQuery(positionChange.CardExternalId),
                cancellationToken);
            if (playerCard == null)
            {
                throw new NoPlayerCardFoundForRosterUpdateException(
                    $"Found a position change for {positionChange.CardExternalId}, but there is no corresponding PlayerCard");
            }

            // Update the domain PlayerCard with the new position
            await _commandSender.Send(new UpdatePlayerCardCommand(playerCard, null, positionChange), cancellationToken);
        }
    }

    /// <summary>
    /// Applies player additions to the domain
    /// </summary>
    /// <param name="seasonYear">The season to apply roster updates for</param>
    /// <param name="playerAdditions">The players added to the roster</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <exception cref="NoExternalPlayerCardFoundForRosterUpdateException">Thrown when the external source did not have any information on the newly added player card</exception>
    private async Task ApplyPlayerAdditions(SeasonYear seasonYear, IEnumerable<PlayerAddition> playerAdditions,
        CancellationToken cancellationToken)
    {
        foreach (var playerAddition in playerAdditions)
        {
            // Get details about the player card
            var externalCard =
                await _cardCatalog.GetMlbPlayerCard(seasonYear, playerAddition.CardExternalId, cancellationToken);
            if (externalCard == null)
            {
                throw new NoExternalPlayerCardFoundForRosterUpdateException(
                    $"Roster Update had a new player {playerAddition.CardExternalId}, but no external data could be found");
            }

            // Create the player card in this domain
            await _commandSender.Send(new CreatePlayerCardCommand(externalCard.Value), cancellationToken);
        }
    }
}