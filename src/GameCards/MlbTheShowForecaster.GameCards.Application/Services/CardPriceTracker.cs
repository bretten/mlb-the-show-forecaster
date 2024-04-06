﻿using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreateListing;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdateListing;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetAllPlayerCards;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetListingByCardExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;

/// <summary>
/// Tracks the price changes of cards
/// </summary>
public sealed class CardPriceTracker : ICardPriceTracker
{
    /// <summary>
    /// Service that will retrieve marketplace pricing information for card listings
    /// </summary>
    private readonly ICardMarketplace _cardMarketplace;

    /// <summary>
    /// Sends queries to retrieve state from the system
    /// </summary>
    private readonly IQuerySender _querySender;

    /// <summary>
    /// Sends commands to mutate the system
    /// </summary>
    private readonly ICommandSender _commandSender;

    /// <summary>
    /// The percentage change threshold that determines significant listing price changes
    /// </summary>
    private readonly IListingPriceSignificantChangeThreshold _listingPriceSignificantChangeThreshold;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="cardMarketplace">Service that will retrieve marketplace pricing information for card listings</param>
    /// <param name="querySender">Sends queries to retrieve state from the system</param>
    /// <param name="commandSender">Sends commands to mutate the system</param>
    /// <param name="listingPriceSignificantChangeThreshold">The percentage change threshold that determines significant listing price changes</param>
    public CardPriceTracker(ICardMarketplace cardMarketplace, IQuerySender querySender, ICommandSender commandSender,
        IListingPriceSignificantChangeThreshold listingPriceSignificantChangeThreshold)
    {
        _cardMarketplace = cardMarketplace;
        _querySender = querySender;
        _commandSender = commandSender;
        _listingPriceSignificantChangeThreshold = listingPriceSignificantChangeThreshold;
    }

    /// <summary>
    /// Tracks the prices of card listings and adds new listings to the domain if they don't exist
    /// </summary>
    /// <param name="year">The year to check card prices for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <exception cref="CardPriceTrackerFoundNoCardsException">Thrown if no PlayerCards are found in the domain</exception>
    public async Task TrackCardPrices(SeasonYear year, CancellationToken cancellationToken = default)
    {
        // Get all PlayerCards in the domain
        var domainPlayerCards = await _querySender.Send(new GetAllPlayerCardsQuery(year), cancellationToken);

        // There should always be PlayerCards in the domain, or else the system has not been properly populated
        if (domainPlayerCards == null || !domainPlayerCards.Any())
        {
            throw new CardPriceTrackerFoundNoCardsException($"No PlayerCards found for {year.Value}");
        }

        foreach (var domainPlayerCard in domainPlayerCards)
        {
            // Get the Listing for the card as it currently exists in the domain
            var domainListing =
                await _querySender.Send(new GetListingByCardExternalIdQuery(domainPlayerCard.ExternalId),
                    cancellationToken);

            // Get the pricing information from the external card marketplace
            var externalPrices =
                await _cardMarketplace.GetCardPrice(year, domainPlayerCard.ExternalId, cancellationToken);

            // If the Listing doesn't exist in this domain yet, create it
            if (domainListing == null)
            {
                await _commandSender.Send(new CreateListingCommand(externalPrices), cancellationToken);
                continue;
            }

            // If there is new pricing information from the external source, update the domain Listing with the new data
            if (externalPrices.HasNewPrices(domainListing) || externalPrices.HasNewHistoricalPrices(domainListing))
            {
                await _commandSender.Send(
                    new UpdateListingCommand(domainListing, externalPrices, _listingPriceSignificantChangeThreshold),
                    cancellationToken
                );
            }
        }
    }
}