using System.Collections.ObjectModel;
using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Database;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Marketplace.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.Npgsql;
using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Moq;
using Npgsql;
using Testcontainers.PostgreSql;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Marketplace.EntityFrameworkCore;

public class HybridNpgsqlEntityFrameworkCoreListingRepositoryIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    /// <summary>
    /// Configures the container options that will be used for each test
    /// </summary>
    /// <exception cref="DockerNotRunningException">Thrown if Docker is not running</exception>
    public HybridNpgsqlEntityFrameworkCoreListingRepositoryIntegrationTests()
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
    public async Task GetConnection_InvalidNpgsqlDataSource_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;

        var stubbedDbAtomicOperation = new Mock<IAtomicDatabaseOperation>();
        stubbedDbAtomicOperation.Setup(x => x.GetCurrentActiveConnection(cToken))
            .ReturnsAsync(Mock.Of<DbConnection>());

        var mockDbContext = new MarketplaceDbContext(new DbContextOptions<MarketplaceDbContext>());
        var repo = new HybridNpgsqlEntityFrameworkCoreListingRepository(mockDbContext, stubbedDbAtomicOperation.Object);

        var action = async () => await repo.Add(Faker.FakeListing(), cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidNpgsqlDataSourceForListingRepositoryException>(actual);
    }

    [Fact]
    public async Task GetTransaction_InvalidNpgsqlDataSource_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;

        var stubbedDbAtomicOperation = new Mock<IAtomicDatabaseOperation>();
        stubbedDbAtomicOperation.Setup(x => x.GetCurrentActiveTransaction(cToken))
            .ReturnsAsync(Mock.Of<DbTransaction>());

        var mockDbContext = new MarketplaceDbContext(new DbContextOptions<MarketplaceDbContext>());
        var repo = new HybridNpgsqlEntityFrameworkCoreListingRepository(mockDbContext, stubbedDbAtomicOperation.Object);

        var action = async () => await repo.Add(Faker.FakeListing(), cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidNpgsqlDataSourceForListingRepositoryException>(actual);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Add_Listing_AddsListingToDbContextSet()
    {
        /*
         * Arrange
         */
        // The Listing to add
        var price1 = Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1), buyPrice: 1, sellPrice: 2);
        var price2 = Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 2), buyPrice: 10, sellPrice: 20);
        var price3 = Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 3), buyPrice: 100, sellPrice: 200);
        var order1 = Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 10);
        var order2 = Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 11);
        var order3 = Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc), price: 10);
        var listing = Faker.FakeListing(2025, Faker.FakeGuid1, buyPrice: 1000, sellPrice: 2000,
            new List<ListingHistoricalPrice>()
            {
                price3, price1, price2
            }, new List<ListingOrder>()
            {
                order1, order2, order3
            });

        // Set up the database
        var (dbContext, atomicOperation) = await CreateDatabase();
        var repo = new HybridNpgsqlEntityFrameworkCoreListingRepository(dbContext, atomicOperation);

        /*
         * Act
         */
        await repo.Add(listing);
        // Commit
        var transaction = await atomicOperation.GetCurrentActiveTransaction();
        await transaction.CommitAsync();

        /*
         * Arrange
         */
        // Re-create the context so that the record is freshly retrieved from the database
        var (assertDbContext, assertAtomicOperation) = await CreateDatabase();

        // Verify the Listing was added
        Assert.Single(assertDbContext.ListingsWithHistoricalPrices());
        var actual = assertDbContext.ListingsWithHistoricalPrices().First();
        Assert.Equal(listing, actual);
        Assert.Equal(2025, actual.Year.Value);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.CardExternalId.Value);
        Assert.Equal(1000, actual.BuyPrice.Value);
        Assert.Equal(2000, actual.SellPrice.Value);

        // Verify prices were added
        Assert.Equal(3, actual.HistoricalPrices.Count);
        Assert.Contains(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1), buyPrice: 1, sellPrice: 2),
            actual.HistoricalPricesChronologically);
        Assert.Contains(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 2), buyPrice: 10, sellPrice: 20),
            actual.HistoricalPricesChronologically);
        Assert.Contains(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 3), buyPrice: 100, sellPrice: 200),
            actual.HistoricalPricesChronologically);

        // Verify orders were added
        Assert.Equal(3, actual.OrdersChronologically.Count);
        Assert.Contains(Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 11),
            actual.OrdersChronologically);
        Assert.Contains(Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 10),
            actual.OrdersChronologically);
        Assert.Contains(Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc), price: 10),
            actual.OrdersChronologically);
    }

    /// <summary>
    /// NOTE: Deprecated by <see cref="NpgsqlListingRepository"/>. This originally relied on a unique hash which included order quantity
    /// </summary>
    [Fact]
    [Trait("Category", "Integration")]
    public async Task Update_Listing_UpdatesListingInDbContextSet()
    {
        /*
         * Arrange
         */
        // The Listing to update
        var price1 = Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1), buyPrice: 1, sellPrice: 2);
        var price2 = Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 2), buyPrice: 10, sellPrice: 20);
        var order1 = Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 10);
        var order2 = Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 11);
        var order3 = Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc), price: 10);
        var listing1 = Faker.FakeListing(2025, Faker.FakeGuid1, buyPrice: 100, sellPrice: 200,
            new List<ListingHistoricalPrice>()
            {
                price2, price1
            }, new List<ListingOrder>()
            {
                order1, order2, order3
            });
        // The Listing that will remain unchanged
        var listing2 = Faker.FakeListing(2025, Faker.FakeGuid2, buyPrice: 8, sellPrice: 9);

        // Set up the database
        var (dbContext, atomicOperation) = await CreateDatabase();
        var repo = new HybridNpgsqlEntityFrameworkCoreListingRepository(dbContext, atomicOperation);

        // Seed the database with initial data
        await AddListingToDb(dbContext, listing1);
        await AddListingToDb(dbContext, listing2);

        // Update Listing1
        var mockThreshold = Mock.Of<IListingPriceSignificantChangeThreshold>();
        listing1.LogHistoricalPrice(new DateOnly(2024, 4, 3), buyPrice: NaturalNumber.Create(100),
            sellPrice: NaturalNumber.Create(200));
        listing1.UpdatePrices(newBuyPrice: NaturalNumber.Create(1000), newSellPrice: NaturalNumber.Create(2000),
            mockThreshold);
        // Deprecated
        // listing1.UpdateOrders(new List<ListingOrder>()
        // {
        //     Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 10, quantity: 1),
        //     Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 11, quantity: 2),
        //     Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc), price: 10, quantity: 3),
        //     Faker.FakeListingOrder(new DateTime(2025, 1, 17, 2, 3, 0, DateTimeKind.Utc), price: 20, quantity: 1),
        // });

        /*
         * Act
         */
        await repo.Update(listing1);
        // Commit
        var transaction = await atomicOperation.GetCurrentActiveTransaction();
        await transaction.CommitAsync();

        /*
         * Assert
         */
        // Re-create the context so that the record is freshly retrieved from the database
        var (assertDbContext, assertAtomicOperation) = await CreateDatabase();

        // Both Listings should exist
        Assert.Equal(2, assertDbContext.ListingsWithHistoricalPrices().Count());
        var listingId1 = listing1.Id;
        Assert.Single(assertDbContext.ListingsWithHistoricalPrices().Where(x => x.Id == listingId1));
        var listingId2 = listing2.Id;
        Assert.Single(assertDbContext.ListingsWithHistoricalPrices().Where(x => x.Id == listingId2));

        // Verify Listing1 was updated
        var actual = assertDbContext.ListingsWithHistoricalPrices().First(x => x.Id == listingId1);
        Assert.Equal(listing1, actual);
        Assert.Equal(2025, actual.Year.Value);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.CardExternalId.Value);
        Assert.Equal(1000, actual.BuyPrice.Value);
        Assert.Equal(2000, actual.SellPrice.Value);

        // Verify Listing1 prices were updated
        Assert.Equal(3, actual.HistoricalPricesChronologically.Count);
        Assert.Equal(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1), buyPrice: 1, sellPrice: 2),
            actual.HistoricalPricesChronologically[0]);
        Assert.Equal(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 2), buyPrice: 10, sellPrice: 20),
            actual.HistoricalPricesChronologically[1]);
        Assert.Equal(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 3), buyPrice: 100, sellPrice: 200),
            actual.HistoricalPricesChronologically[2]);

        // Deprecated
        // Verify Listing2 orders were updated
        // Assert.Equal(4, actual.OrdersChronologically.Count);
        // Assert.Equal(
        //     Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 11),
        //     actual.OrdersChronologically[0]);
        // Assert.Equal(
        //     Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 10),
        //     actual.OrdersChronologically[1]);
        // Assert.Equal(
        //     Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc), price: 10),
        //     actual.OrdersChronologically[2]);
        // Assert.Equal(
        //     Faker.FakeListingOrder(new DateTime(2025, 1, 17, 2, 3, 0, DateTimeKind.Utc), price: 20),
        //     actual.OrdersChronologically[3]);
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

        var (dbContext, atomicOperation) = await CreateDatabase();
        var repo = new HybridNpgsqlEntityFrameworkCoreListingRepository(dbContext, atomicOperation);
        await AddListingToDb(dbContext, listing);

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

        var (dbContext, atomicOperation) = await CreateDatabase();
        var repo = new HybridNpgsqlEntityFrameworkCoreListingRepository(dbContext, atomicOperation);
        await AddListingToDb(dbContext, listing);

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
        // Set up the DB
        var (dbContext, atomicOperation) = await CreateDatabase();
        var repo = new HybridNpgsqlEntityFrameworkCoreListingRepository(dbContext, atomicOperation);
        // Add the listing to the DB
        await AddListingToDb(dbContext, listing);

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
        // Commit
        var transaction = await atomicOperation.GetCurrentActiveTransaction();
        await transaction.CommitAsync();

        /*
         * Assert
         */
        // Separate db context for asserting
        var (assertDbContext, assertAtomicOperation) = await CreateDatabase();
        // Verify the prices were inserted
        var actual = await assertDbContext.ListingHistoricalPrices
            .Where(x => EF.Property<Guid>(x, Constants.ListingHistoricalPrices.ListingId) == listing.Id).ToListAsync();
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
        // Set up the DB
        var (dbContext, atomicOperation) = await CreateDatabase();
        var repo = new HybridNpgsqlEntityFrameworkCoreListingRepository(dbContext, atomicOperation);
        // Add the listing to the DB
        await AddListingToDb(dbContext, listing);

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
        // Commit
        var transaction = await atomicOperation.GetCurrentActiveTransaction();
        await transaction.CommitAsync();

        /*
         * Assert
         */
        // Separate db context for asserting
        var (assertDbContext, assertAtomicOperation) = await CreateDatabase();
        // Verify the orders were inserted
        var actual = await assertDbContext.ListingOrders
            .Where(x => EF.Property<Guid>(x, Constants.ListingOrders.ListingId) == listing.Id).ToListAsync();
        Assert.Equal(3, actual.Count);
        Assert.Contains(Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 11),
            actual);
        Assert.Contains(Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 10),
            actual);
        Assert.Contains(Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc), price: 10),
            actual);
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public async Task DisposeAsync() => await _container.DisposeAsync();

    private async Task<DbConnection> GetDbConnection()
    {
        NpgsqlConnection connection = new(_container.GetConnectionString() + ";Pooling=false;");
        await connection.OpenAsync();
        return connection;
    }

    private MarketplaceDbContext GetDbContext(DbConnection connection)
    {
        var contextOptions = new DbContextOptionsBuilder<MarketplaceDbContext>()
            .UseNpgsql(connection)
            .LogTo(Console.WriteLine)
            .Options;
        return new MarketplaceDbContext(contextOptions);
    }

    private NpgsqlDataSource GetNpgsqlDataSource()
    {
        return new NpgsqlDataSourceBuilder(_container.GetConnectionString() + ";Pooling=false;")
            .Build();
    }

    private async Task<(MarketplaceDbContext dbContext, DbAtomicDatabaseOperation atomicOperation)> CreateDatabase()
    {
        var connection = await GetDbConnection();
        var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        var dbDataSource = GetNpgsqlDataSource();
        var atomicDbOperation = new DbAtomicDatabaseOperation(dbDataSource);
        return (dbContext, atomicDbOperation);
    }

    private async Task AddListingToDb(MarketplaceDbContext dbContext, Listing listing)
    {
        await dbContext.AddAsync(listing);
        await dbContext.AddRangeAsync(listing.HistoricalPrices);
        await dbContext.AddRangeAsync(listing.Orders);
        await dbContext.SaveChangesAsync();
    }

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}