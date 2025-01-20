using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Database;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Marketplace.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore.Exceptions;
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
        // Arrange
        var fakeHistoricalPrice1 =
            Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1), buyPrice: 1, sellPrice: 2);
        var fakeHistoricalPrice2 =
            Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 2), buyPrice: 10, sellPrice: 20);
        var fakeHistoricalPrice3 =
            Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 3), buyPrice: 100, sellPrice: 200);
        var order1 =
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 10, quantity: 1);
        var order2 =
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 11, quantity: 1);
        var order3 =
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc), price: 10, quantity: 1);
        var fakeListing = Faker.FakeListing(Faker.FakeGuid1, buyPrice: 1000, sellPrice: 2000,
            new List<ListingHistoricalPrice>()
            {
                fakeHistoricalPrice3, fakeHistoricalPrice1, fakeHistoricalPrice2
            }, new List<ListingOrder>()
            {
                order1, order2, order3
            });

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();
        var dbDataSource = GetNpgsqlDataSource();
        var atomicDbOperation = new DbAtomicDatabaseOperation(dbDataSource);

        var repo = new HybridNpgsqlEntityFrameworkCoreListingRepository(dbContext, atomicDbOperation);

        // Act
        await repo.Add(fakeListing);
        var transaction = await atomicDbOperation.GetCurrentActiveTransaction();
        await transaction.CommitAsync();

        // Assert
        await using var assertConnection =
            await GetDbConnection(); // Re-create the context so that the record is freshly retrieved from the database
        await using var assertDbContext = GetDbContext(assertConnection);
        await assertDbContext.Database.MigrateAsync();

        Assert.Single(assertDbContext.ListingsWithHistoricalPrices());

        var actual = assertDbContext.ListingsWithHistoricalPrices().First();
        Assert.Equal(fakeListing, actual);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.CardExternalId.Value);
        Assert.Equal(1000, actual.BuyPrice.Value);
        Assert.Equal(2000, actual.SellPrice.Value);

        Assert.Equal(3, actual.HistoricalPricesChronologically.Count);
        Assert.Equal(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1), buyPrice: 1, sellPrice: 2),
            actual.HistoricalPricesChronologically[0]);
        Assert.Equal(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 2), buyPrice: 10, sellPrice: 20),
            actual.HistoricalPricesChronologically[1]);
        Assert.Equal(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 3), buyPrice: 100, sellPrice: 200),
            actual.HistoricalPricesChronologically[2]);

        Assert.Equal(3, actual.OrdersChronologically.Count);
        Assert.Equal(
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 11, quantity: 1),
            actual.OrdersChronologically[0]);
        Assert.Equal(
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 10, quantity: 1),
            actual.OrdersChronologically[1]);
        Assert.Equal(
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc), price: 10, quantity: 1),
            actual.OrdersChronologically[2]);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Update_Listing_UpdatesListingInDbContextSet()
    {
        // Arrange
        var fakeHistoricalPrice1 =
            Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1), buyPrice: 1, sellPrice: 2);
        var fakeHistoricalPrice2 =
            Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 2), buyPrice: 10, sellPrice: 20);
        var order1 =
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 10, quantity: 1);
        var order2 =
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 11, quantity: 1);
        var order3 =
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc), price: 10, quantity: 1);
        var fakeListing1 = Faker.FakeListing(Faker.FakeGuid1, buyPrice: 100, sellPrice: 200,
            new List<ListingHistoricalPrice>()
            {
                fakeHistoricalPrice2, fakeHistoricalPrice1
            }, new List<ListingOrder>()
            {
                order1, order2, order3
            });
        var fakeListing2 = Faker.FakeListing(Faker.FakeGuid2, buyPrice: 8, sellPrice: 9);

        var mockThreshold = Mock.Of<IListingPriceSignificantChangeThreshold>();

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();
        var dbDataSource = GetNpgsqlDataSource();
        var atomicDbOperation = new DbAtomicDatabaseOperation(dbDataSource);

        var repo = new HybridNpgsqlEntityFrameworkCoreListingRepository(dbContext, atomicDbOperation);

        await repo.Add(fakeListing1);
        await repo.Add(fakeListing2);
        var transaction = await atomicDbOperation.GetCurrentActiveTransaction();
        await transaction.CommitAsync();

        // Act
        await using var actConnection = await GetDbConnection();
        await using var actDbContext = GetDbContext(actConnection);
        await actDbContext.Database.MigrateAsync();
        var actDbDataSource = GetNpgsqlDataSource();
        var actAtomicDbOperation = new DbAtomicDatabaseOperation(actDbDataSource);

        var actRepo = new HybridNpgsqlEntityFrameworkCoreListingRepository(actDbContext, actAtomicDbOperation);
        fakeListing1.LogHistoricalPrice(new DateOnly(2024, 4, 3), buyPrice: NaturalNumber.Create(100),
            sellPrice: NaturalNumber.Create(200));
        fakeListing1.UpdatePrices(newBuyPrice: NaturalNumber.Create(1000), newSellPrice: NaturalNumber.Create(2000),
            mockThreshold);
        fakeListing1.UpdateOrders(new List<ListingOrder>()
        {
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 10, quantity: 1),
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 11, quantity: 2),
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc), price: 10, quantity: 3),
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 2, 3, 0, DateTimeKind.Utc), price: 20, quantity: 1),
        });
        await actRepo.Update(fakeListing1);
        var actTransaction = await actAtomicDbOperation.GetCurrentActiveTransaction();
        await actTransaction.CommitAsync();

        // Assert
        await using var assertConnection =
            await GetDbConnection(); // Re-create the context so that the record is freshly retrieved from the database
        await using var assertDbContext = GetDbContext(assertConnection);
        await assertDbContext.Database.MigrateAsync();

        Assert.Equal(2, assertDbContext.ListingsWithHistoricalPrices().Count());

        var listingId1 = fakeListing1.Id;
        Assert.Single(assertDbContext.ListingsWithHistoricalPrices().Where(x => x.Id == listingId1));
        var listingId2 = fakeListing2.Id;
        Assert.Single(assertDbContext.ListingsWithHistoricalPrices().Where(x => x.Id == listingId2));

        var actual = assertDbContext.ListingsWithHistoricalPrices().First(x => x.Id == listingId1);
        Assert.Equal(fakeListing1, actual);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.CardExternalId.Value);
        Assert.Equal(1000, actual.BuyPrice.Value);
        Assert.Equal(2000, actual.SellPrice.Value);

        Assert.Equal(3, actual.HistoricalPricesChronologically.Count);
        Assert.Equal(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1), buyPrice: 1, sellPrice: 2),
            actual.HistoricalPricesChronologically[0]);
        Assert.Equal(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 2), buyPrice: 10, sellPrice: 20),
            actual.HistoricalPricesChronologically[1]);
        Assert.Equal(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 3), buyPrice: 100, sellPrice: 200),
            actual.HistoricalPricesChronologically[2]);

        Assert.Equal(4, actual.OrdersChronologically.Count);
        Assert.Equal(
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 11, quantity: 2),
            actual.OrdersChronologically[0]);
        Assert.Equal(
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 10, quantity: 1),
            actual.OrdersChronologically[1]);
        Assert.Equal(
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 3, 0, DateTimeKind.Utc), price: 10, quantity: 3),
            actual.OrdersChronologically[2]);
        Assert.Equal(
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 2, 3, 0, DateTimeKind.Utc), price: 20, quantity: 1),
            actual.OrdersChronologically[3]);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetByExternalId_CardExternalId_ReturnsListingFromDbContextSet()
    {
        // Arrange
        var fakeHistoricalPrice1 =
            Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1), buyPrice: 1, sellPrice: 2);
        var fakeHistoricalPrice2 =
            Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 2), buyPrice: 10, sellPrice: 20);
        var order1 =
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 10, quantity: 1);
        var order2 =
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 11, quantity: 1);
        var fakeListing1 = Faker.FakeListing(Faker.FakeGuid1, buyPrice: 100, sellPrice: 200,
            new List<ListingHistoricalPrice>()
            {
                fakeHistoricalPrice2, fakeHistoricalPrice1
            }, new List<ListingOrder>()
            {
                order1, order2
            });
        var fakeListing2 = Faker.FakeListing(Faker.FakeGuid2, buyPrice: 8, sellPrice: 9);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();
        var dbDataSource = GetNpgsqlDataSource();
        var atomicDbOperation = new DbAtomicDatabaseOperation(dbDataSource);

        var repo = new HybridNpgsqlEntityFrameworkCoreListingRepository(dbContext, atomicDbOperation);
        await repo.Add(fakeListing1);
        await repo.Add(fakeListing2);
        var transaction = await atomicDbOperation.GetCurrentActiveTransaction();
        await transaction.CommitAsync();

        // Act
        var actual = await repo.GetByExternalId(fakeListing1.CardExternalId);

        // Assert
        await using var assertConnection =
            await GetDbConnection(); // Re-create the context so that the record is freshly retrieved from the database
        await using var assertDbContext = GetDbContext(assertConnection);
        await assertDbContext.Database.MigrateAsync();

        Assert.NotNull(actual);
        Assert.Equal(fakeListing1, actual);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.CardExternalId.Value);
        Assert.Equal(100, actual.BuyPrice.Value);
        Assert.Equal(200, actual.SellPrice.Value);

        Assert.Equal(2, actual.HistoricalPricesChronologically.Count);
        Assert.Equal(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1), buyPrice: 1, sellPrice: 2),
            actual.HistoricalPricesChronologically[0]);
        Assert.Equal(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 2), buyPrice: 10, sellPrice: 20),
            actual.HistoricalPricesChronologically[1]);

        Assert.Equal(2, actual.OrdersChronologically.Count);
        Assert.Equal(
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 11, quantity: 1),
            actual.OrdersChronologically[0]);
        Assert.Equal(
            Faker.FakeListingOrder(new DateTime(2025, 1, 17, 1, 2, 0, DateTimeKind.Utc), price: 10, quantity: 1),
            actual.OrdersChronologically[1]);
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

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}