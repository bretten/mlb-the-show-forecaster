using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCard.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetPlayerCardByExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Results;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using Microsoft.Extensions.Logging;

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
    /// Logger
    /// </summary>
    private readonly ILogger<IRosterUpdateOrchestrator> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rosterUpdateFeed">Keeps tracks of roster updates that need to be applied</param>
    /// <param name="cardCatalog">The external source of player cards</param>
    /// <param name="querySender">Sends queries to retrieve state from the system</param>
    /// <param name="commandSender">Sends commands to mutate the system</param>
    /// <param name="logger">Logger</param>
    public RosterUpdateOrchestrator(IRosterUpdateFeed rosterUpdateFeed, ICardCatalog cardCatalog,
        IQuerySender querySender, ICommandSender commandSender, ILogger<IRosterUpdateOrchestrator> logger)
    {
        _rosterUpdateFeed = rosterUpdateFeed;
        _cardCatalog = cardCatalog;
        _querySender = querySender;
        _commandSender = commandSender;
        _logger = logger;
    }

    /// <summary>
    /// Applies roster updates to the domain
    /// </summary>
    /// <param name="seasonYear">The season to apply roster updates for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task<IEnumerable<RosterUpdateOrchestratorResult>> SyncRosterUpdates(SeasonYear seasonYear,
        CancellationToken cancellationToken = default)
    {
        // Get a list of all available roster updates from the external source
        var rosterUpdates = await _rosterUpdateFeed.GetNewRosterUpdates(seasonYear, cancellationToken);

        // Apply each roster update sequentially (synchronously) so that historical changes are preserved in order
        var results = new List<RosterUpdateOrchestratorResult>();
        foreach (var rosterUpdate in rosterUpdates.OldToNew)
        {
            await ApplyRosterUpdate(seasonYear, rosterUpdate, cancellationToken);
            results.Add(RosterUpdateOrchestratorResult.Create(rosterUpdate));
        }

        return results;
    }

    /// <summary>
    /// Applies a single roster update to the domain
    /// </summary>
    /// <param name="seasonYear">The season of the roster update</param>
    /// <param name="rosterUpdate">The roster update to apply</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <exception cref="RosterUpdateOrchestratorInterruptedException">Thrown if there was an inner exception in any of the changes of the roster update</exception>
    private async Task ApplyRosterUpdate(SeasonYear seasonYear, RosterUpdate rosterUpdate,
        CancellationToken cancellationToken)
    {
        // Get tasks for all rating changes, position changes, and player additions in this roster update
        var tasks = PreparePlayerCardChanges(seasonYear, rosterUpdate.RatingChanges, rosterUpdate.PositionChanges,
                cancellationToken)
            .Concat(PreparePlayerAdditions(seasonYear, rosterUpdate.NewPlayers, cancellationToken));

        // Apply all roster update changes synchronously until issue #172
        var exceptions = new List<Exception>();
        foreach (var task in tasks)
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                exceptions.Add(e);
            }
        }

        if (exceptions.Count != 0)
        {
            var msg = string.Join(", ", exceptions.Select(e => e.Message));
            throw new RosterUpdateOrchestratorInterruptedException(msg, exceptions);
        }

        // The roster update has been successfully applied, so close it
        await _rosterUpdateFeed.CompleteRosterUpdate(rosterUpdate, cancellationToken);
    }

    /// <summary>
    /// Prepares player rating changes and position changes to the domain by getting tasks for all of them
    /// </summary>
    /// <param name="seasonYear">The season of the roster update</param>
    /// <param name="ratingChanges">A collection of rating changes for different player cards</param>
    /// <param name="positionChanges">A collection of position changes for different player cards</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>A collection of Tasks that will execute the roster update changes</returns>
    private IEnumerable<Task> PreparePlayerCardChanges(SeasonYear seasonYear,
        IReadOnlyList<PlayerRatingChange> ratingChanges, IReadOnlyList<PlayerPositionChange> positionChanges,
        CancellationToken cancellationToken)
    {
        // Hash set is used to check if we already applied the position change in the rating change loop for a given player card
        var appliedPositionChangeCardExternalIds = new HashSet<CardExternalId>();

        // Apply rating changes to the domain
        foreach (var ratingChange in ratingChanges.OrderByDescending(x => x.Improved ? 1 : 0))
        {
            // To prevent querying the domain for the PlayerCard twice, check if there is also a position change for this card
            var positionChange = positionChanges.Cast<PlayerPositionChange?>()
                .FirstOrDefault(x => x?.CardExternalId == ratingChange.CardExternalId);
            if (positionChange != null)
            {
                // There is both a rating change and a position change. Mark the position change as done by adding it to the hash set
                appliedPositionChangeCardExternalIds.Add(ratingChange.CardExternalId);
            }

            yield return ApplyRatingChange(seasonYear, ratingChange, positionChange, cancellationToken);
        }

        // Now (after applying rating changes), apply pending position changes
        foreach (var positionChange in positionChanges)
        {
            // If we have already applied the position change during the rating change update, skip it
            if (appliedPositionChangeCardExternalIds.Contains(positionChange.CardExternalId))
            {
                continue;
            }

            yield return ApplyPositionChange(seasonYear, positionChange, cancellationToken);
        }
    }

    /// <summary>
    /// Prepares player additions to the domain by getting tasks for them
    /// </summary>
    /// <param name="seasonYear">The season to apply roster updates for</param>
    /// <param name="playerAdditions">The players added to the roster</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>A collection of Tasks that will execute the roster update changes</returns>
    private IEnumerable<Task> PreparePlayerAdditions(SeasonYear seasonYear,
        IEnumerable<PlayerAddition> playerAdditions, CancellationToken cancellationToken)
    {
        foreach (var playerAddition in playerAdditions)
        {
            yield return ApplyPlayerAddition(seasonYear, playerAddition, cancellationToken);
        }
    }

    /// <summary>
    /// Applies a rating change and a position change (if there is one)
    /// </summary>
    /// <param name="seasonYear">The season of the roster update</param>
    /// <param name="ratingChange">The rating change to apply</param>
    /// <param name="positionChange">The position change to apply if there is one</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <exception cref="NoPlayerCardFoundForRosterUpdateException">Thrown if no <see cref="PlayerCard"/> could be found in the domain to apply the updates to</exception>
    private async Task ApplyRatingChange(SeasonYear seasonYear, PlayerRatingChange ratingChange,
        PlayerPositionChange? positionChange, CancellationToken cancellationToken)
    {
        // Get the corresponding player card
        var playerCard = await _querySender.Send(
            new GetPlayerCardByExternalIdQuery(seasonYear, ratingChange.CardExternalId), cancellationToken);
        if (playerCard == null)
        {
            throw new NoPlayerCardFoundForRosterUpdateException(
                $"Found a rating update for {ratingChange.CardExternalId}, but there is no corresponding PlayerCard");
        }

        // If the ratings have already been applied to the player card, no further action is required
        if (ratingChange.IsApplied(playerCard))
        {
            return;
        }

        // Get the current state of the player card from the external card catalog
        var externalCard =
            await _cardCatalog.GetMlbPlayerCard(playerCard.Year, playerCard.ExternalId, cancellationToken);

        // Update the domain PlayerCard with the rating and the position change if there is one
        await _commandSender.Send(new UpdatePlayerCardCommand(playerCard, externalCard, ratingChange, positionChange),
            cancellationToken);
    }

    /// <summary>
    /// Applies a position change
    /// </summary>
    /// <param name="seasonYear">The season of the roster update</param>
    /// <param name="positionChange">The position change to apply</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <exception cref="NoPlayerCardFoundForRosterUpdateException">Thrown if no <see cref="PlayerCard"/> could be found in the domain to apply the updates to</exception>
    private async Task ApplyPositionChange(SeasonYear seasonYear, PlayerPositionChange positionChange,
        CancellationToken cancellationToken)
    {
        // Get the corresponding player card
        var playerCard = await _querySender.Send(
            new GetPlayerCardByExternalIdQuery(seasonYear, positionChange.CardExternalId), cancellationToken);
        if (playerCard == null)
        {
            throw new NoPlayerCardFoundForRosterUpdateException(
                $"Found a position change for {positionChange.CardExternalId}, but there is no corresponding PlayerCard");
        }

        // Update the domain PlayerCard with the new position
        await _commandSender.Send(new UpdatePlayerCardCommand(playerCard, null, null, positionChange),
            cancellationToken);
    }

    /// <summary>
    /// Applies a player addition change
    /// </summary>
    /// <param name="seasonYear">The season of the change</param>
    /// <param name="playerAddition">The newly added player card</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <exception cref="NoExternalPlayerCardFoundForRosterUpdateException">Thrown when the external source did not have any information on the newly added player card</exception>
    private async Task ApplyPlayerAddition(SeasonYear seasonYear, PlayerAddition playerAddition,
        CancellationToken cancellationToken)
    {
        try
        {
            // Get details about the player card
            var externalCard =
                await _cardCatalog.GetMlbPlayerCard(seasonYear, playerAddition.CardExternalId, cancellationToken);

            // Create the player card in this domain
            await _commandSender.Send(new CreatePlayerCardCommand(externalCard), cancellationToken);
        }
        catch (MlbPlayerCardNotFoundInCatalogException)
        {
            throw new NoExternalPlayerCardFoundForRosterUpdateException(
                $"Roster Update had a new player named {playerAddition.PlayerName} with ID {playerAddition.CardExternalId}, but no external data could be found");
        }
        catch (EmptyPlayerAdditionCardExternalIdException e)
        {
            // We can safely skip these as an external service doesn't have any further info on the player in this case
            _logger.LogWarning(e, e.Message);
        }
        catch (PlayerCardAlreadyExistsException)
        {
            // The player card has already been added. Creating a player card is idempotent and multiple create commands should do nothing
        }
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