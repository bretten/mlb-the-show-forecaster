using System.Globalization;
using System.Text.Json;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.EventStores;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using StackExchange.Redis;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.EventStores;

/// <summary>
/// Redis event store for Listing prices and orders
///
/// Uses the Redis stream to act as the append-only event store and provides checkpointing via fast in-memory lookups
/// </summary>
public sealed class RedisListingEventStore : IListingEventStore
{
    /// <summary>
    /// Redis connection
    /// </summary>
    private readonly IConnectionMultiplexer _redisConnection;

    /// <summary>
    /// The maximum number of new orders that will be appended to the event store at one time for a given Listing. This
    /// is because the MLB The Show API only provides the last 200
    /// </summary>
    private const int MaxOrderCount = 200;

    /// <summary>
    /// The format when storing <see cref="DateOnly"/>s in Redis
    /// </summary>
    internal const string DateFormat = "O";

    /// <summary>
    /// The format when storing <see cref="DateTime"/>s in Redis
    /// </summary>
    internal const string DateTimeFormat = "u";

    /// <summary>
    /// The format when storing <see cref="Guid"/>s in Redis
    /// </summary>
    internal const string GuidFormat = "D";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="redisConnection">Redis connection</param>
    public RedisListingEventStore(IConnectionMultiplexer redisConnection)
    {
        _redisConnection = redisConnection;
    }

    /// <summary>
    /// Key for storing the most recent state of a listing for peeking purposes
    /// </summary>
    /// <param name="year">The year of the listing</param>
    /// <param name="externalId">The ID of the listing</param>
    internal static string ListingKey(SeasonYear year, CardExternalId externalId) =>
        $"listings:{year.Value}:{externalId.AsStringDigits}";

    /// <summary>
    /// Key for the Listing prices event store
    /// </summary>
    /// <param name="year">The year of the listing</param>
    internal static string PricesEventStoreKey(SeasonYear year) => $"listings:prices:{year.Value}";

    /// <summary>
    /// Key for the chronologically last acknowledged Listing price in the event store
    /// </summary>
    /// <param name="year">The year of the listing</param>
    internal static string LastAcknowledgedPriceKey(SeasonYear year) =>
        $"listings:prices:{year.Value}:last_acknowledged";

    /// <summary>
    /// Key for keeping track of Listing prices that were recently added to the event store so duplicates aren't re-added
    /// </summary>
    /// <param name="year">The year of the listing</param>
    /// <param name="externalId">The ID of the listing</param>
    internal static string RecentPricesKey(SeasonYear year, CardExternalId externalId) =>
        $"listings:prices:{year.Value}:recent:{externalId.AsStringDigits}";

    /// <summary>
    /// Key for the Listing orders event store
    /// </summary>
    /// <param name="year">The year of the listing</param>
    internal static string OrdersEventStoreKey(SeasonYear year) => $"listings:orders:{year.Value}";

    /// <summary>
    /// Key for the chronologically last acknowledged Listing order in the event store
    /// </summary>
    /// <param name="year">The year of the listing</param>
    internal static string LastAcknowledgedOrderKey(SeasonYear year) =>
        $"listings:orders:{year.Value}:last_acknowledged";

    /// <summary>
    /// Key for keeping track of Listing orders that were recently added to the event store so duplicates aren't re-added
    /// </summary>
    /// <param name="year">The year of the listing</param>
    /// <param name="externalId">The ID of the listing</param>
    internal static string RecentOrdersKey(SeasonYear year, CardExternalId externalId) =>
        $"listings:orders:{year.Value}:recent:{externalId.AsStringDigits}";

    /// <summary>
    /// Uniquely identifies a price in the Redis data type that tracks prices recently added to the event store (<see cref="RecentPricesKey"/>)
    /// This ID is used to prevent duplicate prices from being added to the event store
    /// </summary>
    /// <param name="price"><see cref="CardListingPrice"/></param>
    /// <returns>An ID that uniquely identifies the <see cref="CardListingPrice"/></returns>
    internal static string PriceId(CardListingPrice price) => price.Date.ToString(DateFormat);

    /// <summary>
    /// Uniquely identifies an order in the Redis data type that tracks orders recently added to the event store (<see cref="RecentOrdersKey"/>)
    /// This ID is used to prevent duplicate orders from being added to the event store
    /// </summary>
    /// <param name="order"><see cref="CardListingOrder"/></param>
    /// <returns>An ID that uniquely identifies the <see cref="CardListingOrder"/></returns>
    internal static string OrderId(CardListingOrder order) =>
        $"{order.Date:s}-{order.Price.Value}-{order.SequenceNumber.Value}";

    /// <inheritdoc />
    public async Task AppendNewPricesAndOrders(SeasonYear year, CardListing cardListing)
    {
        await AppendNewPrices(year, cardListing);
        await AppendNewOrders(year, cardListing);
    }

    /// <summary>
    /// Checks the <see cref="CardListing"/> for new prices by comparing them to the Redis storage for prices recently
    /// added to the event store. If they don't exist in the event store, they will be appended
    /// </summary>
    /// <param name="year">The year of the listing</param>
    /// <param name="cardListing"><see cref="CardListing"/> with prices and orders</param>
    private async Task AppendNewPrices(SeasonYear year, CardListing cardListing)
    {
        var db = _redisConnection.GetDatabase();
        var eventStoreKey = PricesEventStoreKey(year);
        var recentKey = RecentPricesKey(year, cardListing.CardExternalId);

        // Check for new prices
        foreach (var price in cardListing.HistoricalPrices.OrderBy(x => x.Date))
        {
            // Uniquely identifies the card pricing
            var id = PriceId(price);

            // If it exists in the set, it is already in the event store
            var exists = await db.SortedSetScoreAsync(recentKey, id);
            if (exists.HasValue)
            {
                continue;
            }

            // Add the event to the append-only store
            await db.StreamAddAsync(eventStoreKey, [
                new NameValueEntry("card_external_id", cardListing.CardExternalId.Value.ToString(GuidFormat)),
                new NameValueEntry("date", price.Date.ToString(DateFormat)),
                new NameValueEntry("buy_price", price.BestBuyPrice.Value),
                new NameValueEntry("sell_price", price.BestSellPrice.Value)
            ]);
            // Add to the set indicating it has been appended
            await db.SortedSetAddAsync(recentKey, id,
                new DateTimeOffset(price.Date.ToDateTime(TimeOnly.MinValue)).ToUnixTimeSeconds());
        }

        // Store the Listing's recent state
        await db.StringSetAsync(ListingKey(year, cardListing.CardExternalId), JsonSerializer.Serialize(cardListing));
    }

    /// <summary>
    /// Checks the <see cref="CardListing"/> for new orders by comparing them to the Redis storage for orders recently
    /// added to the event store. If they don't exist in the event store, they will be appended
    /// </summary>
    /// <param name="year">The year of the listing</param>
    /// <param name="cardListing"><see cref="CardListing"/> with prices and orders</param>
    private async Task AppendNewOrders(SeasonYear year, CardListing cardListing)
    {
        var db = _redisConnection.GetDatabase();
        var eventStoreKey = OrdersEventStoreKey(year);
        var recentKey = RecentOrdersKey(year, cardListing.CardExternalId);

        // Check for new orders
        foreach (var order in cardListing.RecentOrders.OrderBy(x => x.Date))
        {
            // Uniquely identifies the order
            var id = OrderId(order);

            // If it exists in the set, it is already in the event store
            var exists = await db.SortedSetScoreAsync(recentKey, id);
            if (exists.HasValue)
            {
                continue;
            }

            // Add the event to the append-only store
            await db.StreamAddAsync(eventStoreKey, [
                new NameValueEntry("card_external_id", cardListing.CardExternalId.Value.ToString(GuidFormat)),
                new NameValueEntry("date", order.Date.ToString(DateTimeFormat)),
                new NameValueEntry("price", order.Price.Value),
                new NameValueEntry("sequence_number", order.SequenceNumber.Value)
            ]);
            // Add to the set indicating it has been appended
            await db.SortedSetAddAsync(recentKey, id, new DateTimeOffset(order.Date).ToUnixTimeSeconds());
        }

        // The external source only provides the last 200 orders (MaxOrderCount), so keep a buffer of some of them, but remove the rest
        await db.SortedSetRemoveRangeByRankAsync(recentKey, 0, MaxOrderCount * -2);
    }

    /// <inheritdoc />
    public async Task<NewPriceEvents> PollNewPrices(SeasonYear year, int count)
    {
        var db = _redisConnection.GetDatabase();

        // Get the last acknowledged ID
        var checkpoint = await GetCheckpoint(LastAcknowledgedPriceKey(year));

        // Read new prices that were appended after the last acknowledged ID
        var entries = await db.StreamReadAsync(PricesEventStoreKey(year), checkpoint, count: count);
        var newPrices = new Dictionary<CardExternalId, List<ListingHistoricalPrice>>();
        foreach (var entry in entries)
        {
            // Parse the price
            var externalId = entry["card_external_id"].ToString();
            var date = DateOnly.ParseExact(entry["date"].ToString(), DateFormat, CultureInfo.InvariantCulture);
            entry["buy_price"].TryParse(out int buyPrice);
            entry["sell_price"].TryParse(out int sellPrice);

            var cardExternalId = CardExternalId.Create(new Guid(externalId));
            var price = ListingHistoricalPrice.Create(date, NaturalNumber.Create(buyPrice),
                NaturalNumber.Create(sellPrice));

            // Add the price to the result
            if (!newPrices.TryGetValue(cardExternalId, out var listingPrices))
            {
                listingPrices = new List<ListingHistoricalPrice>();
                newPrices[cardExternalId] = listingPrices;
            }

            newPrices[cardExternalId].Add(price);

            checkpoint = entry.Id.ToString();
        }

        return new NewPriceEvents(checkpoint, newPrices.ToDictionary(x => x.Key, x => x.Value.AsReadOnly()));
    }

    /// <inheritdoc />
    public async Task<NewOrderEvents> PollNewOrders(SeasonYear year, int count)
    {
        var db = _redisConnection.GetDatabase();

        // Get the last acknowledged ID
        var checkpoint = await GetCheckpoint(LastAcknowledgedOrderKey(year));

        // Read new orders that were appended after the last acknowledged ID
        var entries = await db.StreamReadAsync(OrdersEventStoreKey(year), checkpoint, count: count);
        var newOrders = new Dictionary<CardExternalId, List<ListingOrder>>();
        foreach (var entry in entries)
        {
            // Parse the order
            var externalId = entry["card_external_id"].ToString();
            var date = DateTime.ParseExact(entry["date"].ToString(), DateTimeFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal).ToUniversalTime();
            entry["price"].TryParse(out int price);

            var cardExternalId = CardExternalId.Create(new Guid(externalId));
            var order = ListingOrder.Create(date, NaturalNumber.Create(price));

            // Add the order to the result
            if (!newOrders.TryGetValue(cardExternalId, out var ordersForListing))
            {
                ordersForListing = new List<ListingOrder>();
                newOrders[cardExternalId] = ordersForListing;
            }

            newOrders[cardExternalId].Add(order);

            // Will chronologically be the last entry, so keep reassigning
            checkpoint = entry.Id.ToString();
        }

        return new NewOrderEvents(checkpoint, newOrders.ToDictionary(k => k.Key, v => v.Value.AsReadOnly()));
    }

    /// <inheritdoc />
    public async Task AcknowledgePrices(SeasonYear year, string lastAcknowledgedId)
    {
        await UpdateCheckpoint(LastAcknowledgedPriceKey(year), lastAcknowledgedId);
    }

    /// <inheritdoc />
    public async Task AcknowledgeOrders(SeasonYear year, string lastAcknowledgedId)
    {
        await UpdateCheckpoint(LastAcknowledgedOrderKey(year), lastAcknowledgedId);
    }

    /// <inheritdoc />
    public async Task<CardListing> PeekListing(SeasonYear year, CardExternalId cardExternalId)
    {
        var db = _redisConnection.GetDatabase();

        var strValue = await db.StringGetAsync(ListingKey(year, cardExternalId));
        return JsonSerializer.Deserialize<CardListing>(strValue!);
    }

    /// <summary>
    /// Gets the last acknowledged ID for a given key
    /// </summary>
    /// <param name="key">The key</param>
    /// <returns>The last acknowledged ID</returns>
    private async Task<string> GetCheckpoint(string key)
    {
        var db = _redisConnection.GetDatabase();

        // Get the last acknowledged ID or if null, start at the beginning
        var lastAcknowledgedId = await db.StringGetAsync(key);
        return lastAcknowledgedId.HasValue ? lastAcknowledgedId.ToString() : "0";
    }

    /// <summary>
    /// Updates the last acknowledged ID for a given key
    /// </summary>
    /// <param name="key">The key</param>
    /// <param name="lastAcknowledgedId">The new last acknowledged ID</param>
    private async Task UpdateCheckpoint(string key, string lastAcknowledgedId)
    {
        var db = _redisConnection.GetDatabase();
        await db.StringSetAsync(key, lastAcknowledgedId);
    }
}