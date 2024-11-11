using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Cards.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.TestClasses;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Testcontainers.PostgreSql;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Cards.EntityFrameworkCore;

public class EntityFrameworkCorePlayerCardRepositoryIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    /// <summary>
    /// Configures the container options that will be used for each test
    /// </summary>
    /// <exception cref="DockerNotRunningException">Thrown if Docker is not running</exception>
    public EntityFrameworkCorePlayerCardRepositoryIntegrationTests()
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
    [Trait("Category", "Integration")]
    public async Task Add_PlayerCard_AddsPlayerCardToDbContextSet()
    {
        // Arrange
        var fakePlayerCard = Faker.FakePlayerCard(externalId: Faker.FakeGuid1);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        var repo = new EntityFrameworkCorePlayerCardRepository(dbContext);

        // Act
        await repo.Add(fakePlayerCard);
        await dbContext.SaveChangesAsync();

        // Assert
        Assert.NotNull(dbContext.PlayerCards);
        Assert.Equal(1, dbContext.PlayerCards.Count());
        Assert.Equal(fakePlayerCard, dbContext.PlayerCards.First());
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Update_PlayerCard_UpdatesPlayerCardInDbContextSet()
    {
        // Arrange
        var fakePlayerCard = Faker.FakePlayerCard(externalId: Faker.FakeGuid1);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        await dbContext.AddAsync(fakePlayerCard);
        await dbContext.SaveChangesAsync();

        var repo = new EntityFrameworkCorePlayerCardRepository(dbContext);

        // Act
        fakePlayerCard.ChangePlayerRating(new DateOnly(2024, 5, 1),
            Faker.FakeOverallRating(70),
            Faker.FakePlayerCardAttributes(stamina: 111)
        );
        await repo.Update(fakePlayerCard);
        await dbContext.SaveChangesAsync();

        // Assert
        await using var assertConnection =
            await GetDbConnection(); // Re-create the context so that the record is freshly retrieved from the database
        await using var assertDbContext = GetDbContext(assertConnection);
        await assertDbContext.Database.MigrateAsync();
        var actual = assertDbContext.PlayerCardsWithHistoricalRatings().First();
        Assert.Equal(fakePlayerCard, actual);
        Assert.Equal(2, actual.HistoricalRatingsChronologically.Count);
        Assert.Equal(70, actual.OverallRating.Value);
        Assert.Equal(111, actual.PlayerCardAttributes.Stamina.Value);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetAll_SeasonYear_ReturnsAllPlayerCardsWithMatchingSeason()
    {
        // Arrange
        var seasonYear = SeasonYear.Create(2024);
        var fakePlayerCard1 = Faker.FakePlayerCard(year: seasonYear.Value, externalId: Faker.FakeGuid1);
        var fakePlayerCard2 = Faker.FakePlayerCard(year: seasonYear.Value, externalId: Faker.FakeGuid2);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        await dbContext.AddAsync(fakePlayerCard1);
        await dbContext.AddAsync(fakePlayerCard2);
        await dbContext.SaveChangesAsync();

        var repo = new EntityFrameworkCorePlayerCardRepository(dbContext);

        // Act
        var actual = await repo.GetAll(seasonYear);

        // Assert
        Assert.NotNull(actual);
        var actualList = actual.ToList();
        Assert.Equal(2, actualList.Count);
        Assert.Equal(fakePlayerCard1,
            actualList.First(x => x.ExternalId.Value == new Guid("00000000-0000-0000-0000-000000000001")));
        Assert.Equal(fakePlayerCard2,
            actualList.First(x => x.ExternalId.Value == new Guid("00000000-0000-0000-0000-000000000002")));
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetByExternalId_CardExternalId_ReturnsPlayerCardFromDbContextSet()
    {
        // Arrange
        var fakePlayerCard = Faker.FakePlayerCard(externalId: Faker.FakeGuid1);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        await dbContext.AddAsync(fakePlayerCard);
        await dbContext.SaveChangesAsync();

        var repo = new EntityFrameworkCorePlayerCardRepository(dbContext);

        // Act
        var actual = await repo.GetByExternalId(Faker.FakeCardExternalId(Faker.FakeGuid1));

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(fakePlayerCard, actual);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Exists_PlayerCardExists_ReturnsTrue()
    {
        // Arrange
        var fakePlayerCard = Faker.FakePlayerCard(externalId: Faker.FakeGuid1);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        await dbContext.AddAsync(fakePlayerCard);
        await dbContext.SaveChangesAsync();

        await using var assertConnection =
            await GetDbConnection(); // Re-create the context so that the record is freshly retrieved from the database
        await using var assertDbContext = GetDbContext(assertConnection);
        await assertDbContext.Database.MigrateAsync();

        var repo = new EntityFrameworkCorePlayerCardRepository(assertDbContext);

        // Act
        var actual = await repo.Exists(Faker.FakeCardExternalId(Faker.FakeGuid1));

        // Assert
        Assert.True(actual);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Exists_PlayerCardDoesNotExist_ReturnsFalse()
    {
        // Arrange
        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        var repo = new EntityFrameworkCorePlayerCardRepository(dbContext);

        // Act
        var actual = await repo.Exists(Faker.FakeCardExternalId(Faker.FakeGuid1));

        // Assert
        Assert.False(actual);
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public async Task DisposeAsync() => await _container.DisposeAsync();

    private async Task<DbConnection> GetDbConnection()
    {
        NpgsqlConnection connection = new(_container.GetConnectionString() + ";Pooling=false;");
        await connection.OpenAsync();
        return connection;
    }

    private CardsDbContext GetDbContext(DbConnection connection)
    {
        var contextOptions = new DbContextOptionsBuilder<CardsDbContext>()
            .UseNpgsql(connection)
            .LogTo(Console.WriteLine)
            .Options;
        return new CardsDbContext(contextOptions);
    }

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}