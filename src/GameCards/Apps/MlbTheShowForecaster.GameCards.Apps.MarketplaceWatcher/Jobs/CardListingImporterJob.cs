using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs.Io;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.EventStores;
using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs.Io;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs;

/// <summary>
/// Job that bulk imports a card's listing's prices and orders to an event store
/// </summary>
public sealed class CardListingImporterJob : BaseJob<SeasonJobInput, CardListingImporterJobResult>, IDisposable
{
    /// <summary>
    /// Gets all player cards
    /// </summary>
    private readonly IPlayerCardRepository _playerCardRepository;

    /// <summary>
    /// Service that will retrieve marketplace pricing and orders information for card listings
    /// </summary>
    private readonly ICardMarketplace _cardMarketplace;

    /// <summary>
    /// The event store to append new Listing prices and orders
    /// </summary>
    private readonly IListingEventStore _listingEventStore;

    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<CardListingImporterJob> _logger;

    /// <summary>
    /// Service name
    /// </summary>
    private const string S = nameof(CardListingImporterJob);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerCardRepository">Gets all player cards</param>
    /// <param name="cardMarketplace">Service that will retrieve marketplace pricing and orders information for card listings</param>
    /// <param name="listingEventStore">The event store to append new Listing prices and orders</param>
    /// <param name="logger">Logger</param>
    public CardListingImporterJob(IPlayerCardRepository playerCardRepository, ICardMarketplace cardMarketplace,
        IListingEventStore listingEventStore, ILogger<CardListingImporterJob> logger)
    {
        _playerCardRepository = playerCardRepository;
        _cardMarketplace = cardMarketplace;
        _listingEventStore = listingEventStore;
        _logger = logger;
    }

    /// <inheritdoc />
    public override async Task<CardListingImporterJobResult> Execute(SeasonJobInput input,
        CancellationToken cancellationToken = default)
    {
        // Get all player cards whose listings will be imported
        var playerCards = (await _playerCardRepository.GetAll(input.Year)).ToList();

        var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
        await Parallel.ForEachAsync(playerCards, options, async (card, token) =>
        {
            // Get the pricing and orders from the external card marketplace
            var externalPricesAndOrders =
                await _cardMarketplace.GetCardPrice(card.Year, card.ExternalId, cancellationToken);

            // Append the new prices and orders to the event store
            await _listingEventStore.AppendNewPricesAndOrders(card.Year, externalPricesAndOrders);
        });

        _logger.LogInformation($"{S} - {input.Year.Value}");
        _logger.LogInformation($"{S} - Total listings imported = {playerCards.Count}");

        return new CardListingImporterJobResult(playerCards.Count);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
    }
}