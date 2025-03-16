using System.Collections.Concurrent;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreateListing;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdateListing;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdateListingsPricesAndOrders;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetAllPlayerCards;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetListingByCardExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.EventStores;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Results;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;

/// <summary>
/// Tracks the price changes of cards
/// </summary>
public sealed class CardPriceTracker : ICardPriceTracker
{
    /// <summary>
    /// Retrieves marketplace pricing information for card listings
    /// </summary>
    private readonly IListingEventStore _listingEventStore;

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
    /// Batch size for prices and orders
    /// </summary>
    private readonly int _batchSize;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="listingEventStore">Retrieves marketplace pricing information for card listings</param>
    /// <param name="querySender">Sends queries to retrieve state from the system</param>
    /// <param name="commandSender">Sends commands to mutate the system</param>
    /// <param name="listingPriceSignificantChangeThreshold">The percentage change threshold that determines significant listing price changes</param>
    /// <param name="batchSize">Batch size for prices and orders</param>
    public CardPriceTracker(IListingEventStore listingEventStore, IQuerySender querySender,
        ICommandSender commandSender, IListingPriceSignificantChangeThreshold listingPriceSignificantChangeThreshold,
        int batchSize)
    {
        _listingEventStore = listingEventStore;
        _querySender = querySender;
        _commandSender = commandSender;
        _listingPriceSignificantChangeThreshold = listingPriceSignificantChangeThreshold;
        _batchSize = batchSize;
    }

    /// <summary>
    /// Tracks the prices of card listings and adds new listings to the domain if they don't exist
    /// </summary>
    /// <param name="year">The year to check card prices for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <exception cref="CardPriceTrackerFoundNoCardsException">Thrown if no PlayerCards are found in the domain</exception>
    public async Task<CardPriceTrackerResult> TrackCardPrices(SeasonYear year,
        CancellationToken cancellationToken = default)
    {
        // Get all PlayerCards in the domain
        var domainPlayerCards = (await _querySender.Send(new GetAllPlayerCardsQuery(year), cancellationToken) ??
                                 Array.Empty<PlayerCard>()).ToList();

        // There should always be PlayerCards in the domain, or else the system has not been properly populated
        if (domainPlayerCards == null || domainPlayerCards.Count == 0)
        {
            throw new CardPriceTrackerFoundNoCardsException($"No PlayerCards found for {year.Value}");
        }

        // Make sure each card price listings are up-to-date
        var newListings = 0;
        var updatedListings = 0;
        var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
        var listings = new ConcurrentDictionary<CardExternalId, Listing>();
        await Parallel.ForEachAsync(domainPlayerCards, options, async (domainPlayerCard, token) =>
        {
            // Get the Listing for the card as it currently exists in the domain
            var domainListing = await _querySender.Send(
                new GetListingByCardExternalIdQuery(domainPlayerCard.Year, domainPlayerCard.ExternalId, false),
                cancellationToken);

            // Get the pricing information from the external card marketplace
            var externalPrices = await _listingEventStore.PeekListing(year, domainPlayerCard.ExternalId);

            // If the Listing doesn't exist in this domain yet, create it
            if (domainListing == null)
            {
                await _commandSender.Send(new CreateListingCommand(externalPrices), cancellationToken);
                Interlocked.Increment(ref newListings);

                var addedListing = await _querySender.Send(
                    new GetListingByCardExternalIdQuery(domainPlayerCard.Year, domainPlayerCard.ExternalId, false),
                    cancellationToken);
                listings.TryAdd(domainPlayerCard.ExternalId, addedListing!);

                return;
            }

            listings.TryAdd(domainPlayerCard.ExternalId, domainListing);

            // If there is new pricing information from the external source, update the domain Listing with the new data
            if (externalPrices.HasNewPrices(domainListing))
            {
                await _commandSender.Send(
                    new UpdateListingCommand(domainListing, externalPrices, _listingPriceSignificantChangeThreshold),
                    cancellationToken
                );
                Interlocked.Increment(ref updatedListings);
            }
        });

        // Batch insert orders and prices
        await _commandSender.Send(new UpdateListingsPricesAndOrdersCommand(year, listings.ToDictionary(), _batchSize),
            cancellationToken);

        return new CardPriceTrackerResult(TotalCards: domainPlayerCards.Count,
            TotalNewListings: newListings,
            TotalUpdatedListings: updatedListings
        );
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
    }
}