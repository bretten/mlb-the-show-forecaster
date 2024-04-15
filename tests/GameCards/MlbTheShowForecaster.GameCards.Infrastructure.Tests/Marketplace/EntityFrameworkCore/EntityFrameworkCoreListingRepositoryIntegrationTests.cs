using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.TestClasses;
using Microsoft.EntityFrameworkCore;
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
                fakeHistoricalPrice1, fakeHistoricalPrice2, fakeHistoricalPrice3
            });

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        var repo = new EntityFrameworkCoreListingRepository(dbContext);

        // Act
        await repo.Add(fakeListing);

        // Assert
        await using var assertConnection =
            await GetDbConnection(); // Re-create the context so that the record is freshly retrieved from the database
        await using var assertDbContext = GetDbContext(assertConnection);
        await assertDbContext.Database.MigrateAsync();
        var actual = assertDbContext.ListingsWithHistoricalPrices().First();
        Assert.Equal(fakeListing, actual);
        Assert.Equal(3, actual.HistoricalPricesChronologically.Count);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Update_Listing_UpdatesListingInDbContextSet()
    {
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetByExternalId_CardExternalId_ReturnsListingFromDbContextSet()
    {
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

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}