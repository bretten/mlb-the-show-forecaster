using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.TestClasses;
using Microsoft.EntityFrameworkCore;
using Moq;
using Npgsql;
using Testcontainers.PostgreSql;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Marketplace.EntityFrameworkCore;

public class EntityFrameworkCoreListingRepositoryIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    /// <summary>
    /// Configures the container options that will be used for each test
    /// </summary>
    /// <exception cref="DockerNotRunningException">Thrown if Docker is not running</exception>
    public EntityFrameworkCoreListingRepositoryIntegrationTests()
    {
        try
        {
            _container = new PostgreSqlBuilder()
                .WithName(GetType().Name + Guid.NewGuid())
                .WithUsername("postgres")
                .WithPassword("password99")
                .WithPortBinding(54324, 5432)
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
    public async Task Add_Listing_AddsListingToDbContextSet()
    {
        // Arrange
        var fakeHistoricalPrice1 =
            Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1), buyPrice: 1, sellPrice: 2);
        var fakeHistoricalPrice2 =
            Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 2), buyPrice: 10, sellPrice: 20);
        var fakeHistoricalPrice3 =
            Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 3), buyPrice: 100, sellPrice: 200);
        var fakeListing = Faker.FakeListing(Faker.FakeGuid1, buyPrice: 1000, sellPrice: 2000,
            new List<ListingHistoricalPrice>()
            {
                fakeHistoricalPrice3, fakeHistoricalPrice1, fakeHistoricalPrice2
            });

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();
        var dbDataSource = GetNpgsqlDataSource();

        var repo = new EntityFrameworkCoreListingRepository(dbContext, dbDataSource);

        // Act
        await repo.Add(fakeListing);

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
        var fakeListing1 = Faker.FakeListing(Faker.FakeGuid1, buyPrice: 100, sellPrice: 200,
            new List<ListingHistoricalPrice>()
            {
                fakeHistoricalPrice2, fakeHistoricalPrice1
            });
        var fakeListing2 = Faker.FakeListing(Faker.FakeGuid2, buyPrice: 8, sellPrice: 9);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();
        var dbDataSource = GetNpgsqlDataSource();

        await dbContext.AddAsync(fakeListing1);
        await dbContext.AddAsync(fakeListing2);
        await dbContext.SaveChangesAsync();

        var repo = new EntityFrameworkCoreListingRepository(dbContext, dbDataSource);
        var mockThreshold = Mock.Of<IListingPriceSignificantChangeThreshold>();

        // Act
        fakeListing1.LogHistoricalPrice(new DateOnly(2024, 4, 3), buyPrice: NaturalNumber.Create(100),
            sellPrice: NaturalNumber.Create(200));
        fakeListing1.UpdatePrices(newBuyPrice: NaturalNumber.Create(1000), newSellPrice: NaturalNumber.Create(2000),
            mockThreshold);
        await repo.Update(fakeListing1);

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
        var fakeListing1 = Faker.FakeListing(Faker.FakeGuid1, buyPrice: 100, sellPrice: 200,
            new List<ListingHistoricalPrice>()
            {
                fakeHistoricalPrice2, fakeHistoricalPrice1
            });
        var fakeListing2 = Faker.FakeListing(Faker.FakeGuid2, buyPrice: 8, sellPrice: 9);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();
        var dbDataSource = GetNpgsqlDataSource();

        var repo = new EntityFrameworkCoreListingRepository(dbContext, dbDataSource);
        await repo.Add(fakeListing1);
        await repo.Add(fakeListing2);
        await dbContext.SaveChangesAsync();

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
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public async Task DisposeAsync() => await _container.StopAsync();

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