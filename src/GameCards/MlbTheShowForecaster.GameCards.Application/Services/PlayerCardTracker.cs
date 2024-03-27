using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetPlayerCardByExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;

/// <summary>
/// Service that retrieves player cards from an external source and creates a corresponding <see cref="PlayerCard"/>
/// in this domain if it does not yet exist
/// </summary>
public sealed class PlayerCardTracker : IPlayerCardTracker
{
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
    /// <param name="cardCatalog">The external source of player cards</param>
    /// <param name="querySender">Sends queries to retrieve state from the system</param>
    /// <param name="commandSender">Sends commands to mutate the system</param>
    public PlayerCardTracker(ICardCatalog cardCatalog, IQuerySender querySender, ICommandSender commandSender)
    {
        _cardCatalog = cardCatalog;
        _querySender = querySender;
        _commandSender = commandSender;
    }

    /// <summary>
    /// Retrieves player cards from an external source and creates a corresponding <see cref="PlayerCard"/>
    /// in this domain if it does not yet exist
    /// </summary>
    /// <param name="seasonYear">The year to retrieve cards for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task TrackPlayerCards(SeasonYear seasonYear, CancellationToken cancellationToken = default)
    {
        // Get all player cards from the external system
        var mlbPlayerCards = await _cardCatalog.GetAllMlbPlayerCards(seasonYear, cancellationToken);

        foreach (var mlbPlayerCard in mlbPlayerCards)
        {
            var existingPlayerCard =
                await _querySender.Send(new GetPlayerCardByExternalIdQuery(mlbPlayerCard.ExternalUuid),
                    cancellationToken);
            // If the card already exists, no further action is needed
            if (existingPlayerCard != null)
            {
                continue;
            }

            // The card does not exist in this domain, so create it
            await _commandSender.Send(new CreatePlayerCardCommand(mlbPlayerCard), cancellationToken);
        }
    }
}