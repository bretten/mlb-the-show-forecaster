using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Participation;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFramework;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Tests.TestClasses;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Testcontainers.PostgreSql;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Tests.PlayerSeasons.EntityFramework;

/// <summary>
/// Integration tests that use TestContainers to spin up database containers for the tests. Requires Docker to be
/// running for these tests
/// </summary>
public class EntityFrameworkPlayerStatsBySeasonRepositoryIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    /// <summary>
    /// Configures the container options that will be used for each test
    /// </summary>
    /// <exception cref="DockerNotRunningException">Thrown if Docker is not running</exception>
    public EntityFrameworkPlayerStatsBySeasonRepositoryIntegrationTests()
    {
        try
        {
            _container = new PostgreSqlBuilder()
                .WithName(GetType().Name + Guid.NewGuid())
                .WithUsername("postgres")
                .WithPassword("password99")
                .WithPortBinding(54321, 5432)
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
    public async Task Add_PlayerStatsBySeason_AddsPlayerStatsBySeasonToDbContextSet()
    {
        // Arrange
        var seasonYear = SeasonYear.Create(2024);
        var batting = new List<PlayerBattingStatsByGame>
        {
            Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 3, 31)),
            Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 4, 1))
        };
        var pitching = new List<PlayerPitchingStatsByGame>
        {
            Faker.FakePlayerPitchingStats(gameDate: new DateTime(2024, 4, 2)),
            Faker.FakePlayerPitchingStats(gameDate: new DateTime(2024, 4, 3))
        };
        var fielding = new List<PlayerFieldingStatsByGame>
        {
            Faker.FakePlayerFieldingStats(gameDate: new DateTime(2024, 4, 4)),
            Faker.FakePlayerFieldingStats(gameDate: new DateTime(2024, 4, 5))
        };
        var fakePlayerStatsBySeason =
            PlayerStatsBySeason.Create(MlbId.Create(1), seasonYear, batting, pitching, fielding);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        var repo = new EntityFrameworkPlayerStatsBySeasonRepository(dbContext);

        // Act
        await repo.Add(fakePlayerStatsBySeason);
        await dbContext.SaveChangesAsync();

        // Assert
        Assert.NotNull(dbContext.PlayerStatsBySeasons);
        Assert.Equal(1, dbContext.PlayerStatsBySeasons.Count());
        Assert.Equal(fakePlayerStatsBySeason, dbContext.PlayerStatsBySeasons.First());
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Update_ExistingPlayerStatsBySeason_UpdatesPlayerStatsBySeasonInDbContextSet()
    {
        // Arrange
        var seasonYear = SeasonYear.Create(2024);
        var batting = new List<PlayerBattingStatsByGame>
        {
            Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 3, 31)),
            Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 4, 1))
        };
        var pitching = new List<PlayerPitchingStatsByGame>
        {
            Faker.FakePlayerPitchingStats(gameDate: new DateTime(2024, 4, 2)),
            Faker.FakePlayerPitchingStats(gameDate: new DateTime(2024, 4, 3))
        };
        var fielding = new List<PlayerFieldingStatsByGame>
        {
            Faker.FakePlayerFieldingStats(gameDate: new DateTime(2024, 4, 4)),
            Faker.FakePlayerFieldingStats(gameDate: new DateTime(2024, 4, 5))
        };
        var fakePlayerStatsBySeason =
            PlayerStatsBySeason.Create(MlbId.Create(1), seasonYear, batting, pitching, fielding);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        await dbContext.AddAsync(fakePlayerStatsBySeason);
        await dbContext.SaveChangesAsync();

        var repo = new EntityFrameworkPlayerStatsBySeasonRepository(dbContext);

        // Act
        fakePlayerStatsBySeason.LogBattingGame(Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 4, 6)));
        await repo.Update(fakePlayerStatsBySeason);
        await dbContext.SaveChangesAsync();

        // Assert
        var expectedBatting = new List<PlayerBattingStatsByGame>
        {
            Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 3, 31)),
            Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 4, 1)),
            Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 4, 6)) // This game was part of the update
        };
        var expectedPitching = new List<PlayerPitchingStatsByGame>
        {
            Faker.FakePlayerPitchingStats(gameDate: new DateTime(2024, 4, 2)),
            Faker.FakePlayerPitchingStats(gameDate: new DateTime(2024, 4, 3))
        };
        var expectedFielding = new List<PlayerFieldingStatsByGame>
        {
            Faker.FakePlayerFieldingStats(gameDate: new DateTime(2024, 4, 4)),
            Faker.FakePlayerFieldingStats(gameDate: new DateTime(2024, 4, 5))
        };
        await using var assertConnection =
            await GetDbConnection(); // Re-create the context so that the record is freshly retrieved from the database
        await using var assertDbContext = GetDbContext(assertConnection);
        await assertDbContext.Database.MigrateAsync();
        var actual = assertDbContext.PlayerStatsBySeasonsWithGames()
            .First();
        Assert.Equal(fakePlayerStatsBySeason, actual);
        Assert.Equal(expectedBatting, actual.BattingStatsByGamesChronologically);
        Assert.Equal(expectedPitching, actual.PitchingStatsByGamesChronologically);
        Assert.Equal(expectedFielding, actual.FieldingStatsByGamesChronologically);
        Assert.Single(fakePlayerStatsBySeason.DomainEvents);
        Assert.IsType<PlayerBattedInGameEvent>(fakePlayerStatsBySeason.DomainEvents[0]);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetAll_SeasonYear_ReturnsAllPlayerStatsBySeasonWithMatchingSeason()
    {
        // Arrange
        var seasonYear = SeasonYear.Create(2024);
        var batting = new List<PlayerBattingStatsByGame>
        {
            Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 3, 31)),
            Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 4, 1))
        };
        var pitching = new List<PlayerPitchingStatsByGame>
        {
            Faker.FakePlayerPitchingStats(gameDate: new DateTime(2024, 4, 2)),
            Faker.FakePlayerPitchingStats(gameDate: new DateTime(2024, 4, 3))
        };
        var fielding = new List<PlayerFieldingStatsByGame>
        {
            Faker.FakePlayerFieldingStats(gameDate: new DateTime(2024, 4, 4)),
            Faker.FakePlayerFieldingStats(gameDate: new DateTime(2024, 4, 5))
        };
        var fakePlayerStatsBySeason =
            PlayerStatsBySeason.Create(MlbId.Create(1), seasonYear, batting, pitching, fielding);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        await dbContext.AddAsync(fakePlayerStatsBySeason);
        await dbContext.SaveChangesAsync();

        await using var assertConnection =
            await GetDbConnection(); // Re-create the context so that the record is freshly retrieved from the database
        await using var assertDbContext = GetDbContext(assertConnection);
        await assertDbContext.Database.MigrateAsync();
        var repo = new EntityFrameworkPlayerStatsBySeasonRepository(assertDbContext);

        // Act
        var actual = (await repo.GetAll(seasonYear)).ToList();

        // Assert
        Assert.NotNull(actual);
        Assert.Single(actual);
        Assert.Equal(fakePlayerStatsBySeason, actual.First());
        Assert.Equal(batting, actual.First().BattingStatsByGamesChronologically);
        Assert.Equal(pitching, actual.First().PitchingStatsByGamesChronologically);
        Assert.Equal(fielding, actual.First().FieldingStatsByGamesChronologically);
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public async Task DisposeAsync() => await _container.StopAsync();

    private async Task<DbConnection> GetDbConnection()
    {
        NpgsqlConnection connection = new(_container.GetConnectionString() + ";Pooling=false;");
        await connection.OpenAsync();
        return connection;
    }

    private PlayerSeasonsDbContext GetDbContext(DbConnection connection)
    {
        var contextOptions = new DbContextOptionsBuilder<PlayerSeasonsDbContext>()
            .UseNpgsql(connection)
            .LogTo(Console.WriteLine)
            .Options;
        return new PlayerSeasonsDbContext(contextOptions);
    }

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}