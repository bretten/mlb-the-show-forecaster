﻿using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastMlbId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetPlayerCardByExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Results;
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
    /// <exception cref="PlayerCardTrackerFoundNoCardsException">Thrown if the <see cref="ICardCatalog"/> provided no player cards</exception>
    public async Task<PlayerCardTrackerResult> TrackPlayerCards(SeasonYear seasonYear,
        CancellationToken cancellationToken = default)
    {
        // Get all player cards from the external source
        var externalCards = (await _cardCatalog.GetActiveRosterMlbPlayerCards(seasonYear, cancellationToken)).ToList();

        // It is not a real world scenario for there to be no player cards, so stop execution if none are found
        if (externalCards == null || externalCards.Count == 0)
        {
            throw new PlayerCardTrackerFoundNoCardsException($"No player cards were found for {seasonYear.Value}");
        }

        var newPlayerCards = 0;
        var updatedPlayerCards = 0;
        var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
        externalCards = externalCards.Where(x => x.IsSupported).OrderByDescending(x => x.Priority).ToList();
        await Parallel.ForEachAsync(externalCards, options, async (externalCard, token) =>
        {
            var existingPlayerCard = await _querySender.Send(
                new GetPlayerCardByExternalIdQuery(externalCard.Year, externalCard.ExternalUuid), cancellationToken);
            // If the card already exists, no further action is needed
            if (existingPlayerCard != null)
            {
                // Update the MLB ID
                await _commandSender.Send(new UpdatePlayerCardForecastMlbIdCommand(existingPlayerCard),
                    cancellationToken);

                // Update the card if it had a boost/temp rating added or removed
                if (existingPlayerCard.IsBoosted != externalCard.IsBoosted ||
                    existingPlayerCard.HasTemporaryRating != externalCard.HasTemporaryRating)
                {
                    await _commandSender.Send(new UpdatePlayerCardCommand(existingPlayerCard, externalCard),
                        cancellationToken);
                    updatedPlayerCards++;
                }

                return;
            }

            // The card does not exist in this domain, so create it
            await _commandSender.Send(new CreatePlayerCardCommand(externalCard), cancellationToken);
            newPlayerCards++;
        });

        return new PlayerCardTrackerResult(TotalCatalogCards: externalCards.Count,
            TotalNewCatalogCards: newPlayerCards,
            TotalUpdatedPlayerCards: updatedPlayerCards
        );
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        _cardCatalog.Dispose();
    }
}