using System.Collections.ObjectModel;
using System.Globalization;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.EventStores;
using StackExchange.Redis;
using Testcontainers.Redis;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Services.EventStores;

public class RedisListingEventStoreIntegrationTests : IAsyncLifetime
{
    private readonly RedisContainer _container;

    private const string FakePassword = "mypassword";

    /// <summary>
    /// Configures the container options that will be used for each test
    /// </summary>
    /// <exception cref="DockerNotRunningException">Thrown if Docker is not running</exception>
    public RedisListingEventStoreIntegrationTests()
    {
        try
        {
            _container = new RedisBuilder()
                .WithName(GetType().Name + Guid.NewGuid())
                .WithImage("redis/redis-stack:latest")
                .WithEnvironment("REDIS_ARGS", "--requirepass mypassword --appendonly yes")
                .WithPortBinding(6379, true)
                .WithPortBinding(8001, true)
                .Build();
        }
        catch (ArgumentException e)
        {
            if (!e.Message.Contains("Docker is either not running or misconfigured"))
            {
                throw;
            }

            throw new DockerNotRunningException($"Docker is required to run tests for {GetType().Name}");
        }
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task AppendNewPricesAndOrders_NewAndExisting_AppendsOnlyNew()
    {
        /*
         * Arrange
         */
        var year = SeasonYear.Create(2025);
        var cardListing = Faker.FakeCardListing(historicalPrices: new List<CardListingPrice>()
        {
            Faker.FakeCardListingPrice(new DateOnly(2025, 3, 25), bestBuyPrice: 1, 2),
            Faker.FakeCardListingPrice(new DateOnly(2025, 3, 26), bestBuyPrice: 10, 20),
        }, completedOrders: new List<CardListingOrder>()
        {
            Faker.FakeCompletedOrder(new DateTime(2025, 3, 1, 1, 2, 3), 10, 0),
            Faker.FakeCompletedOrder(new DateTime(2025, 3, 1, 1, 2, 3), 10, 1),
            Faker.FakeCompletedOrder(new DateTime(2025, 3, 1, 1, 2, 4), 100, 0),
        });

        var connection = await GetConnection();
        var db = connection.GetDatabase();
        var eventStore = new RedisListingEventStore(connection);

        // Add the existing entries to the event store
        await AddPriceToEventStore(db, year, cardListing.CardExternalId, cardListing.HistoricalPrices[0]);
        await AddOrderToEventStore(db, year, cardListing.CardExternalId, cardListing.RecentOrders[0]);

        /*
         * Act
         */
        await eventStore.AppendNewPricesAndOrders(year, cardListing);

        /*
         * Assert
         */
        // The prices have been added to the event store
        var allPrices = await GetPricesFromEventStore(db, year);
        var prices = allPrices[cardListing.CardExternalId];
        Assert.Equal(2, prices.Count);
        var expectedPrice1 =
            Domain.Tests.Marketplace.TestClasses.Faker.FakeListingHistoricalPrice(new DateOnly(2025, 3, 25), 1, 2);
        Assert.Contains(expectedPrice1, prices);
        var expectedPrice2 =
            Domain.Tests.Marketplace.TestClasses.Faker.FakeListingHistoricalPrice(new DateOnly(2025, 3, 26), 10, 20);
        Assert.Contains(expectedPrice2, prices);
        // The prices have been marked as appended so they aren't duplicated later on
        Assert.True(await SortedSetEntryExists(db,
            RedisListingEventStore.RecentPricesKey(year, cardListing.CardExternalId), "2025-03-25"));
        Assert.True(await SortedSetEntryExists(db,
            RedisListingEventStore.RecentPricesKey(year, cardListing.CardExternalId), "2025-03-26"));
        // The orders have been added to the event store
        var allOrders = await GetOrdersFromEventStore(db, year);
        var orders = allOrders[cardListing.CardExternalId];
        Assert.Equal(3, orders.Count);
        var expectedOrder1 = Domain.Tests.Marketplace.TestClasses.Faker.FakeListingOrder(
            new DateTime(2025, 3, 1, 1, 2, 3, DateTimeKind.Utc), 10);
        Assert.Contains(expectedOrder1, orders);
        var expectedOrder2 = Domain.Tests.Marketplace.TestClasses.Faker.FakeListingOrder(
            new DateTime(2025, 3, 1, 1, 2, 3, DateTimeKind.Utc), 10);
        Assert.Contains(expectedOrder2, orders);
        var expectedOrder3 = Domain.Tests.Marketplace.TestClasses.Faker.FakeListingOrder(
            new DateTime(2025, 3, 1, 1, 2, 4, DateTimeKind.Utc), 100);
        Assert.Contains(expectedOrder3, orders);
        // The orders have been marked as appended so they aren't duplicated later on
        Assert.True(await SortedSetEntryExists(db,
            RedisListingEventStore.RecentOrdersKey(year, cardListing.CardExternalId), "2025-03-01T01:02:03-10-0"));
        Assert.True(await SortedSetEntryExists(db,
            RedisListingEventStore.RecentOrdersKey(year, cardListing.CardExternalId), "2025-03-01T01:02:03-10-1"));
        Assert.True(await SortedSetEntryExists(db,
            RedisListingEventStore.RecentOrdersKey(year, cardListing.CardExternalId), "2025-03-01T01:02:04-100-0"));
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task PollNewPrices_AcknowledgePrices_NewPrices_ReturnsNewThenAcknowledgesNew()
    {
        /*
         * Arrange
         */
        var year = SeasonYear.Create(2025);
        var price1 = Faker.FakeCardListingPrice(new DateOnly(2025, 3, 25), bestBuyPrice: 1, 2);
        var price2 = Faker.FakeCardListingPrice(new DateOnly(2025, 3, 26), bestBuyPrice: 10, 20);
        var cardExternalId = Domain.Tests.Cards.TestClasses.Faker.FakeCardExternalId();

        var connection = await GetConnection();
        var db = connection.GetDatabase();
        var eventStore = new RedisListingEventStore(connection);

        // Add the existing entries to the event store
        var id = await AddPriceToEventStore(db, year, cardExternalId, price1);
        // Acknowledge it so it is no longer returned when polled
        await UpdateCheckpoint(db, RedisListingEventStore.LastAcknowledgedPriceKey(year), id);

        // Add the new price but don't acknowledge it
        var id2 = await AddPriceToEventStore(db, year, cardExternalId, price2);

        /*
         * Act
         */
        var actual = await eventStore.PollNewPrices(year, 100);
        await eventStore.AcknowledgePrices(year, actual.Checkpoint);

        /*
         * Assert
         */
        // The new price was polled
        Assert.Equal(id2, actual.Checkpoint);
        Assert.Single(actual.Prices);

        // The new prices for cardExternalId
        Assert.Single(actual.Prices[cardExternalId]);
        var expectedPrice2 =
            Domain.Tests.Marketplace.TestClasses.Faker.FakeListingHistoricalPrice(new DateOnly(2025, 3, 26), 10, 20);
        Assert.Contains(expectedPrice2, actual.Prices[cardExternalId]);

        // The new prices were acknowledged and are no longer returned when polled
        var nextPoll = await eventStore.PollNewPrices(year, 100);
        Assert.Empty(nextPoll.Prices);
        Assert.Equal(nextPoll.Checkpoint, actual.Checkpoint);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task PollNewOrders_AcknowledgeOrders_NewOrders_ReturnsNewThenAcknowledgesNew()
    {
        /*
         * Arrange
         */
        var year = SeasonYear.Create(2025);
        var order1 = Faker.FakeCompletedOrder(new DateTime(2025, 3, 1, 1, 2, 3), 10, 0);
        var order2 = Faker.FakeCompletedOrder(new DateTime(2025, 3, 1, 1, 2, 3, DateTimeKind.Utc), 10, 1);
        var order3 = Faker.FakeCompletedOrder(new DateTime(2025, 3, 1, 1, 2, 4, DateTimeKind.Local), 100, 0);
        var order4 = Faker.FakeCompletedOrder(new DateTime(2025, 3, 1, 1, 2, 4, DateTimeKind.Local), 100, 1);
        var cardExternalId = Domain.Tests.Cards.TestClasses.Faker.FakeCardExternalId();

        var connection = await GetConnection();
        var db = connection.GetDatabase();
        var eventStore = new RedisListingEventStore(connection);

        // Add the existing entries to the event store
        var id = await AddOrderToEventStore(db, year, cardExternalId, order1);
        // Acknowledge it so it is no longer returned when polled
        await UpdateCheckpoint(db, RedisListingEventStore.LastAcknowledgedOrderKey(year), id);

        // Add the new orders but don't acknowledge them
        var id2 = await AddOrderToEventStore(db, year, cardExternalId, order2);
        var id3 = await AddOrderToEventStore(db, year, cardExternalId, order3);
        var id4 = await AddOrderToEventStore(db, year, cardExternalId, order4);

        /*
         * Act
         */
        var actual = await eventStore.PollNewOrders(year, 100);
        await eventStore.AcknowledgeOrders(year, actual.Checkpoint);

        /*
         * Assert
         */
        // The new orders were polled
        Assert.Equal(id4, actual.Checkpoint);
        Assert.Single(actual.Orders);

        // The new orders for cardExternalId
        Assert.Equal(3, actual.Orders[cardExternalId].Count);
        var expectedOrder2 =
            Domain.Tests.Marketplace.TestClasses.Faker.FakeListingOrder(new DateTime(2025, 3, 1, 1, 2, 3), 10);
        Assert.Contains(expectedOrder2, actual.Orders[cardExternalId]);
        var expectedOrder3 =
            Domain.Tests.Marketplace.TestClasses.Faker.FakeListingOrder(new DateTime(2025, 3, 1, 1, 2, 4), 100);
        Assert.Contains(expectedOrder3, actual.Orders[cardExternalId]);
        var expectedOrder4 =
            Domain.Tests.Marketplace.TestClasses.Faker.FakeListingOrder(new DateTime(2025, 3, 1, 1, 2, 4), 100);
        Assert.Contains(expectedOrder4, actual.Orders[cardExternalId]);

        // The new orders were acknowledged and are no longer returned when polled
        var nextPoll = await eventStore.PollNewOrders(year, 100);
        Assert.Empty(nextPoll.Orders);
        Assert.Equal(nextPoll.Checkpoint, actual.Checkpoint);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task PeekListing_MissingListingState_ReturnsNull()
    {
        // Arrange
        var year = SeasonYear.Create(2025);
        var cardListing = Faker.FakeCardListing(year.Value);

        var connection = await GetConnection();
        var eventStore = new RedisListingEventStore(connection);

        // Act
        var actual = await eventStore.PeekListing(year, cardListing.CardExternalId);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task PeekListing_AppendListingPricesAndOrders_ReturnsRecentListingState()
    {
        // Arrange
        var year = SeasonYear.Create(2025);
        var cardListing = Faker.FakeCardListing(year.Value, "name1", 10, 20,
            historicalPrices: new List<CardListingPrice>()
            {
                Faker.FakeCardListingPrice(new DateOnly(2025, 3, 25), bestBuyPrice: 1, 2),
                Faker.FakeCardListingPrice(new DateOnly(2025, 3, 26), bestBuyPrice: 10, 20),
            }, completedOrders: new List<CardListingOrder>()
            {
                Faker.FakeCompletedOrder(new DateTime(2025, 3, 1, 1, 2, 3), 10, 1),
                Faker.FakeCompletedOrder(new DateTime(2025, 3, 1, 1, 2, 3, DateTimeKind.Utc), 10, 2),
                Faker.FakeCompletedOrder(new DateTime(2025, 3, 1, 1, 2, 4, DateTimeKind.Local), 100, 1)
            });

        var connection = await GetConnection();
        var eventStore = new RedisListingEventStore(connection);

        await eventStore.AppendNewPricesAndOrders(year, cardListing);

        // Act
        var actual = await eventStore.PeekListing(year, cardListing.CardExternalId);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(cardListing.ListingName, actual.Value.ListingName);
        Assert.Equal(cardListing.BestBuyPrice, actual.Value.BestBuyPrice);
        Assert.Equal(cardListing.BestSellPrice, actual.Value.BestSellPrice);
        Assert.Equal(cardListing.CardExternalId, actual.Value.CardExternalId);
        Assert.Equal(cardListing.HistoricalPrices, actual.Value.HistoricalPrices);
        Assert.Equal(cardListing.RecentOrders, actual.Value.RecentOrders);
    }

    private async Task<string> AddPriceToEventStore(IDatabase db, SeasonYear year, CardExternalId cardExternalId,
        CardListingPrice price)
    {
        var eventStoreKey = RedisListingEventStore.PricesEventStoreKey(year);
        var recentKey = RedisListingEventStore.RecentPricesKey(year, cardExternalId);
        var id = RedisListingEventStore.PriceId(price);

        var checkpoint = await db.StreamAddAsync(eventStoreKey, [
            new NameValueEntry("card_external_id", cardExternalId.Value.ToString(RedisListingEventStore.GuidFormat)),
            new NameValueEntry("date", price.Date.ToString(RedisListingEventStore.DateFormat)),
            new NameValueEntry("buy_price", price.BestBuyPrice.Value),
            new NameValueEntry("sell_price", price.BestSellPrice.Value)
        ]);
        await db.SortedSetAddAsync(recentKey, id,
            new DateTimeOffset(price.Date.ToDateTime(TimeOnly.MinValue)).ToUnixTimeSeconds());

        return checkpoint.ToString();
    }

    private async Task<string> AddOrderToEventStore(IDatabase db, SeasonYear year, CardExternalId cardExternalId,
        CardListingOrder order)
    {
        var eventStoreKey = RedisListingEventStore.OrdersEventStoreKey(year);
        var recentKey = RedisListingEventStore.RecentOrdersKey(year, cardExternalId);
        var id = RedisListingEventStore.OrderId(order);

        var checkpoint = await db.StreamAddAsync(eventStoreKey, [
            new NameValueEntry("card_external_id", cardExternalId.Value.ToString(RedisListingEventStore.GuidFormat)),
            new NameValueEntry("date", order.Date.ToString(RedisListingEventStore.DateTimeFormat)),
            new NameValueEntry("price", order.Price.Value),
            new NameValueEntry("sequence_number", order.SequenceNumber.Value)
        ]);
        await db.SortedSetAddAsync(recentKey, id, new DateTimeOffset(order.Date).ToUnixTimeSeconds());

        return checkpoint.ToString();
    }

    private async Task<Dictionary<CardExternalId, ReadOnlyCollection<ListingHistoricalPrice>>> GetPricesFromEventStore(
        IDatabase db, SeasonYear year, string checkpoint = "0", int count = 100)
    {
        var entries =
            await db.StreamReadAsync(RedisListingEventStore.PricesEventStoreKey(year), checkpoint, count: count);
        var newPrices = new Dictionary<CardExternalId, List<ListingHistoricalPrice>>();
        foreach (var entry in entries)
        {
            var externalId = entry["card_external_id"].ToString();
            var date = DateOnly.ParseExact(entry["date"].ToString(), RedisListingEventStore.DateFormat,
                CultureInfo.InvariantCulture);
            entry["buy_price"].TryParse(out int buyPrice);
            entry["sell_price"].TryParse(out int sellPrice);

            var cardExternalId = CardExternalId.Create(new Guid(externalId));
            if (!newPrices.TryGetValue(cardExternalId, out var listingPrices))
            {
                listingPrices = new List<ListingHistoricalPrice>();
                newPrices[cardExternalId] = listingPrices;
            }

            var price = ListingHistoricalPrice.Create(date, NaturalNumber.Create(buyPrice),
                NaturalNumber.Create(sellPrice));
            newPrices[cardExternalId].Add(price);
        }

        return newPrices.ToDictionary(k => k.Key, v => v.Value.AsReadOnly());
    }

    private async Task<Dictionary<CardExternalId, ReadOnlyCollection<ListingOrder>>> GetOrdersFromEventStore(
        IDatabase db, SeasonYear year, string checkpoint = "0", int count = 100)
    {
        var entries =
            await db.StreamReadAsync(RedisListingEventStore.OrdersEventStoreKey(year), checkpoint, count: count);
        var newOrders = new Dictionary<CardExternalId, List<ListingOrder>>();
        foreach (var entry in entries)
        {
            var externalId = entry["card_external_id"].ToString();
            var date = DateTime.ParseExact(entry["date"].ToString(), RedisListingEventStore.DateTimeFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
            entry["price"].TryParse(out int price);

            var cardExternalId = CardExternalId.Create(new Guid(externalId));
            if (!newOrders.TryGetValue(cardExternalId, out var listingOrders))
            {
                listingOrders = new List<ListingOrder>();
                newOrders[cardExternalId] = listingOrders;
            }

            var order = ListingOrder.Create(date, NaturalNumber.Create(price));
            newOrders[cardExternalId].Add(order);
        }

        return newOrders.ToDictionary(k => k.Key, v => v.Value.AsReadOnly());
    }

    private async Task<bool> SortedSetEntryExists(IDatabase db, string key, string member)
    {
        var entry = await db.SortedSetScoreAsync(key, member);
        return entry.HasValue;
    }

    private async Task UpdateCheckpoint(IDatabase db, string key, string lastAcknowledgedId)
    {
        await db.StringSetAsync(key, lastAcknowledgedId);
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public async Task DisposeAsync() => await _container.DisposeAsync();

    private async Task<IConnectionMultiplexer> GetConnection()
    {
        return await ConnectionMultiplexer.ConnectAsync(_container.GetConnectionString(),
            options => { options.Password = FakePassword; });
    }

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}