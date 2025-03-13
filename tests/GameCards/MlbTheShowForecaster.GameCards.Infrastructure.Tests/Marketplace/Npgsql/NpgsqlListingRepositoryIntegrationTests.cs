using System.Collections.ObjectModel;
using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Marketplace.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.Npgsql;
using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Moq;
using Npgsql;
using Testcontainers.PostgreSql;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Marketplace.Npgsql;

public class NpgsqlListingRepositoryIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    /// <summary>
    /// Configures the container options that will be used for each test
    /// </summary>
    /// <exception cref="DockerNotRunningException">Thrown if Docker is not running</exception>
    public NpgsqlListingRepositoryIntegrationTests()
    {
        try
        {
            _container = new PostgreSqlBuilder()
                .WithName(GetType().Name + Guid.NewGuid())
                .WithUsername("postgres")
                .WithPassword("password99")
                .WithPortBinding(5432, true)
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilPortIsAvailable(5432, o => o.WithTimeout(TimeSpan.FromMinutes(1)))
                    .UntilCommandIsCompleted(["pg_isready", "-U", "postgres", "-d", "postgres"],
                        o => o.WithTimeout(TimeSpan.FromMinutes(1)))
                )
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
    public async Task Add_Listing_InsertsListing()
    {
        // Arrange
        var listing = Faker.FakeListing(2025, Faker.FakeGuid1, buyPrice: 1000, sellPrice: 2000);

        var dataSource = GetNpgsqlDataSource();
        var repo = new NpgsqlListingRepository(dataSource);

        // Act
        await repo.Add(listing);

        // Assert
        var listings = await GetListingsFromDb();
        var actual = listings.First();
        Assert.Equal(listing, actual);
        Assert.Equal(2025, actual.Year.Value);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.CardExternalId.Value);
        Assert.Equal(1000, actual.BuyPrice.Value);
        Assert.Equal(2000, actual.SellPrice.Value);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Update_ListingPricesChanged_UpdatesListing()
    {
        // Arrange
        var listing = Faker.FakeListing(2025, Faker.FakeGuid1, buyPrice: 1000, sellPrice: 2000);
        await AddListingToDb(listing);

        var dataSource = GetNpgsqlDataSource();
        var repo = new NpgsqlListingRepository(dataSource);

        listing.UpdatePrices(NaturalNumber.Create(1111), NaturalNumber.Create(2222),
            Mock.Of<IListingPriceSignificantChangeThreshold>());

        // Act
        await repo.Update(listing);

        // Assert
        var listings = await GetListingsFromDb();
        var actual = listings.First();
        Assert.Equal(listing, actual);
        Assert.Equal(2025, actual.Year.Value);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.CardExternalId.Value);
        Assert.Equal(1111, actual.BuyPrice.Value);
        Assert.Equal(2222, actual.SellPrice.Value);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetByExternalId_ExcludeRelated_ReturnsJustListing()
    {
        // Arrange
        var listing = Faker.FakeListing(2025, Faker.FakeGuid1, buyPrice: 1000, sellPrice: 2000,
            new List<ListingHistoricalPrice>()
            {
                Faker.FakeListingHistoricalPrice(new DateOnly(2025, 3, 9), 1, 2),
                Faker.FakeListingHistoricalPrice(new DateOnly(2025, 3, 10), 10, 20),
            }, new List<ListingOrder>()
            {
                Faker.FakeListingOrder(new DateTime(2025, 3, 9, 1, 2, 3, DateTimeKind.Utc), 10),
                Faker.FakeListingOrder(new DateTime(2025, 3, 10, 1, 2, 3, DateTimeKind.Utc), 100),
            });
        await AddListingToDb(listing);

        var dataSource = GetNpgsqlDataSource();
        var repo = new NpgsqlListingRepository(dataSource);

        // Act
        var actual = await repo.GetByExternalId(SeasonYear.Create(2025), CardExternalId.Create(Faker.FakeGuid1), false);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(2025, actual.Year.Value);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.CardExternalId.Value);
        Assert.Equal(1000, actual.BuyPrice.Value);
        Assert.Equal(2000, actual.SellPrice.Value);
        Assert.Empty(actual.HistoricalPrices);
        Assert.Empty(actual.Orders);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetByExternalId_IncludeRelated_ReturnsListingWithRelated()
    {
        // Arrange
        var listing = Faker.FakeListing(2025, Faker.FakeGuid1, buyPrice: 1000, sellPrice: 2000,
            new List<ListingHistoricalPrice>()
            {
                Faker.FakeListingHistoricalPrice(new DateOnly(2025, 3, 9), 1, 2),
                Faker.FakeListingHistoricalPrice(new DateOnly(2025, 3, 10), 10, 20),
            }, new List<ListingOrder>()
            {
                Faker.FakeListingOrder(new DateTime(2025, 3, 9, 1, 2, 3, DateTimeKind.Utc), 10),
                Faker.FakeListingOrder(new DateTime(2025, 3, 10, 1, 2, 3, DateTimeKind.Utc), 100),
            });
        await AddListingToDb(listing);

        var dataSource = GetNpgsqlDataSource();
        var repo = new NpgsqlListingRepository(dataSource);

        // Act
        var actual = await repo.GetByExternalId(SeasonYear.Create(2025), CardExternalId.Create(Faker.FakeGuid1), true);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(2025, actual.Year.Value);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.CardExternalId.Value);
        Assert.Equal(1000, actual.BuyPrice.Value);
        Assert.Equal(2000, actual.SellPrice.Value);

        Assert.Equal(2, actual.HistoricalPrices.Count);
        Assert.Contains(Faker.FakeListingHistoricalPrice(new DateOnly(2025, 3, 9), 1, 2), actual.HistoricalPrices);
        Assert.Contains(Faker.FakeListingHistoricalPrice(new DateOnly(2025, 3, 10), 10, 20), actual.HistoricalPrices);

        Assert.Equal(2, actual.Orders.Count);
        Assert.Contains(Faker.FakeListingOrder(new DateTime(2025, 3, 9, 1, 2, 3), 10), actual.Orders);
        Assert.Contains(Faker.FakeListingOrder(new DateTime(2025, 3, 10, 1, 2, 3), 100), actual.Orders);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Add_ListingPrices_BulkInsertsPrices()
    {
        /*
         * Arrange
         */
        // Prices to add
        var price1 = Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1), buyPrice: 1, sellPrice: 2);
        var price2 = Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 2), buyPrice: 10, sellPrice: 20);
        var price3 = Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 3), buyPrice: 100, sellPrice: 200);
        // Associated Listing
        var listing = Faker.FakeListing();
        // Add the listing to the DB
        await AddListingToDb(listing);

        // Repo
        var dataSource = GetNpgsqlDataSource();
        var repo = new NpgsqlListingRepository(dataSource);

        // The method input
        var listings = new Dictionary<CardExternalId, Listing>()
        {
            { listing.CardExternalId, listing }
        };
        var prices = new Dictionary<CardExternalId, ReadOnlyCollection<ListingHistoricalPrice>>()
        {
            {
                listing.CardExternalId, new List<ListingHistoricalPrice>()
                {
                    price1, price2, price3
                }.AsReadOnly()
            }
        };

        /*
         * Act
         */
        await repo.Add(listings, prices);

        /*
         * Assert
         */
        // Get prices currently in DB
        var allPrices = await GetListingPricesFromDb();
        // Verify the prices were inserted
        var actual = allPrices[listing.Id];
        Assert.Equal(3, actual.Count);
        Assert.Contains(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1), buyPrice: 1, sellPrice: 2), actual);
        Assert.Contains(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 2), buyPrice: 10, sellPrice: 20),
            actual);
        Assert.Contains(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 3), buyPrice: 100, sellPrice: 200),
            actual);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Add_ListingOrders_BulkInsertsOrders()
    {
        /*
         * Arrange
         */
        // Orders to add
        var order1 = Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 10);
        var order2 = Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 11);
        var order3 = Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc), price: 10);
        // Associated Listing
        var listing = Faker.FakeListing();
        // Add the listing to the DB
        await AddListingToDb(listing);

        // Repo
        var dataSource = GetNpgsqlDataSource();
        var repo = new NpgsqlListingRepository(dataSource);

        // The method input
        var listings = new Dictionary<CardExternalId, Listing>()
        {
            { listing.CardExternalId, listing }
        };
        var orders = new Dictionary<CardExternalId, ReadOnlyCollection<ListingOrder>>()
        {
            {
                listing.CardExternalId, new List<ListingOrder>()
                {
                    order1, order2, order3
                }.AsReadOnly()
            }
        };

        /*
         * Act
         */
        await repo.Add(listings, orders);

        /*
         * Assert
         */
        // Get orders currently in DB
        var allOrders = await GetListingOrdersFromDb();
        // Verify the orders were inserted
        var actual = allOrders[listing.Id];
        Assert.Equal(3, actual.Count);
        Assert.Contains(Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 11),
            actual);
        Assert.Contains(Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 10),
            actual);
        Assert.Contains(Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc), price: 10),
            actual);
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        await CreateDatabase();
    }

    public async Task DisposeAsync() => await _container.DisposeAsync();

    private NpgsqlDataSource GetNpgsqlDataSource()
    {
        return new NpgsqlDataSourceBuilder(_container.GetConnectionString() + ";Pooling=false;")
            .Build();
    }

    private async Task<DbConnection> GetDbConnection()
    {
        NpgsqlConnection connection = new(_container.GetConnectionString() + ";Pooling=false;");
        await connection.OpenAsync();
        return connection;
    }

    /// <summary>
    /// EF Core migrations is used to build the database
    /// </summary>
    private MarketplaceDbContext GetDbContext(DbConnection connection)
    {
        var contextOptions = new DbContextOptionsBuilder<MarketplaceDbContext>()
            .UseNpgsql(connection)
            .LogTo(Console.WriteLine)
            .Options;
        return new MarketplaceDbContext(contextOptions);
    }

    /// <summary>
    /// Use EF Core to build the database in the container
    /// </summary>
    private async Task CreateDatabase()
    {
        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();
    }

    private async Task AddListingToDb(Listing listing)
    {
        var dataSource = GetNpgsqlDataSource();
        var repo = new NpgsqlListingRepository(dataSource);
        var listings = new Dictionary<CardExternalId, Listing>()
        {
            { listing.CardExternalId, listing }
        };
        var prices = new Dictionary<CardExternalId, ReadOnlyCollection<ListingHistoricalPrice>>()
        {
            {
                listing.CardExternalId, listing.HistoricalPrices.ToList().AsReadOnly()
            }
        };
        var orders = new Dictionary<CardExternalId, ReadOnlyCollection<ListingOrder>>()
        {
            {
                listing.CardExternalId, listing.Orders.ToList().AsReadOnly()
            }
        };

        await repo.Add(listing);
        await repo.Add(listings, prices);
        await repo.Add(listings, orders);
    }

    private async Task<List<Listing>> GetListingsFromDb()
    {
        var dataSource = GetNpgsqlDataSource();
        await using var connection = await dataSource.OpenConnectionAsync();
        await using var command = new NpgsqlCommand($"SELECT * FROM {Constants.Schema}.{Constants.Listings.TableName}",
            connection);

        await using var reader = await command.ExecuteReaderAsync();
        var listings = new List<Listing>();
        while (await reader.ReadAsync())
        {
            var id = reader.GetGuid(0);
            var year = (ushort)reader.GetInt16(1);
            var cardExternalId = reader.GetGuid(2);
            var buyPrice = reader.GetInt32(3);
            var sellPrice = reader.GetInt32(4);

            var listing = Listing.Create(SeasonYear.Create(year), CardExternalId.Create(cardExternalId),
                NaturalNumber.Create(buyPrice), NaturalNumber.Create(sellPrice), new List<ListingHistoricalPrice>(),
                new List<ListingOrder>(), id);
            listings.Add(listing);
        }

        await reader.CloseAsync();

        return listings;
    }

    private async Task<Dictionary<Guid, List<ListingHistoricalPrice>>> GetListingPricesFromDb()
    {
        var dataSource = GetNpgsqlDataSource();
        await using var connection = await dataSource.OpenConnectionAsync();
        await using var command =
            new NpgsqlCommand($"SELECT * FROM {Constants.Schema}.{Constants.ListingHistoricalPrices.TableName}",
                connection);

        await using var reader = await command.ExecuteReaderAsync();
        var prices = new Dictionary<Guid, List<ListingHistoricalPrice>>();
        while (await reader.ReadAsync())
        {
            var listingId = reader.GetGuid(0);
            var date = reader.GetDateTime(1);
            var buyPrice = reader.GetInt32(2);
            var sellPrice = reader.GetInt32(3);
            var price = ListingHistoricalPrice.Create(DateOnly.FromDateTime(date), NaturalNumber.Create(buyPrice),
                NaturalNumber.Create(sellPrice));

            if (!prices.TryGetValue(listingId, out var listingPrices))
            {
                listingPrices = new List<ListingHistoricalPrice>();
                prices[listingId] = listingPrices;
            }

            listingPrices.Add(price);
        }

        await reader.CloseAsync();

        return prices;
    }

    private async Task<Dictionary<Guid, List<ListingOrder>>> GetListingOrdersFromDb()
    {
        var dataSource = GetNpgsqlDataSource();
        await using var connection = await dataSource.OpenConnectionAsync();
        await using var command =
            new NpgsqlCommand($"SELECT * FROM {Constants.Schema}.{Constants.ListingOrders.TableName}", connection);

        await using var reader = await command.ExecuteReaderAsync();
        var orders = new Dictionary<Guid, List<ListingOrder>>();
        while (await reader.ReadAsync())
        {
            var id = reader.GetInt64(0);
            var listingId = reader.GetGuid(1);
            var date = reader.GetDateTime(2);
            var price = reader.GetInt32(3);
            var order = ListingOrder.Create(date, NaturalNumber.Create(price));

            if (!orders.TryGetValue(listingId, out var listingOrders))
            {
                listingOrders = new List<ListingOrder>();
                orders[listingId] = listingOrders;
            }

            listingOrders.Add(order);
        }

        await reader.CloseAsync();

        return orders;
    }

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}