using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Participation;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Tests.TestClasses;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Testcontainers.PostgreSql;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Tests.PlayerSeasons.EntityFrameworkCore;

/// <summary>
/// Integration tests that use TestContainers to spin up database containers for the tests. Requires Docker to be
/// running for these tests
/// </summary>
public class EntityFrameworkCorePlayerStatsBySeasonRepositoryIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    /// <summary>
    /// Configures the container options that will be used for each test
    /// </summary>
    /// <exception cref="DockerNotRunningException">Thrown if Docker is not running</exception>
    public EntityFrameworkCorePlayerStatsBySeasonRepositoryIntegrationTests()
    {
        try
        {
            _container = new PostgreSqlBuilder()
                .WithName(GetType().Name + Guid.NewGuid())
                .WithUsername("postgres")
                .WithPassword("password99")
                .WithPortBinding(54322, 5432)
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
        var batting = new List<PlayerBattingStatsByGame>
        {
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 3, 31), gameId: 1),
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 4, 1), gameId: 2)
        };
        var pitching = new List<PlayerPitchingStatsByGame>
        {
            Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 4, 2), gameId: 1),
            Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 4, 3), gameId: 2)
        };
        var fielding = new List<PlayerFieldingStatsByGame>
        {
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 4), gameId: 1),
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 5), gameId: 2)
        };
        var fakePlayerStatsBySeason = Faker.FakePlayerStatsBySeason(1, 2024, battingStatsByGames: batting,
            pitchingStatsByGames: pitching, fieldingStatsByGames: fielding);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        var repo = new EntityFrameworkCorePlayerStatsBySeasonRepository(dbContext);

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
    public async Task Add_MultipleFieldingPositionsInSingleGame_AddsPlayerStatsBySeasonToDbContextSet()
    {
        // Arrange
        var batting = new List<PlayerBattingStatsByGame>();
        var pitching = new List<PlayerPitchingStatsByGame>();
        var fielding = new List<PlayerFieldingStatsByGame>
        {
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 4), gameId: 1, position: Position.FirstBase),
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 4), gameId: 1, position: Position.SecondBase)
        };
        var fakePlayerStatsBySeason = Faker.FakePlayerStatsBySeason(1, 2024, battingStatsByGames: batting,
            pitchingStatsByGames: pitching, fieldingStatsByGames: fielding);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        var repo = new EntityFrameworkCorePlayerStatsBySeasonRepository(dbContext);

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
        var batting = new List<PlayerBattingStatsByGame>
        {
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 3, 31), gameId: 1),
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 4, 1), gameId: 2)
        };
        var pitching = new List<PlayerPitchingStatsByGame>
        {
            Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 4, 2), gameId: 1),
            Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 4, 3), gameId: 2)
        };
        var fielding = new List<PlayerFieldingStatsByGame>
        {
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 4), gameId: 1),
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 5), gameId: 2)
        };
        var fakePlayerStatsBySeason = Faker.FakePlayerStatsBySeason(1, 2024, battingStatsByGames: batting,
            pitchingStatsByGames: pitching, fieldingStatsByGames: fielding);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        await dbContext.AddAsync(fakePlayerStatsBySeason);
        await dbContext.SaveChangesAsync();

        var repo = new EntityFrameworkCorePlayerStatsBySeasonRepository(dbContext);

        // Act
        fakePlayerStatsBySeason.LogBattingGame(
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 4, 6), gameId: 3));
        await repo.Update(fakePlayerStatsBySeason);
        await dbContext.SaveChangesAsync();

        // Assert
        var expectedBatting = new List<PlayerBattingStatsByGame>
        {
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 3, 31), gameId: 1),
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 4, 1), gameId: 2),
            // The game below was part of the update
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 4, 6), gameId: 3)
        };
        var expectedPitching = new List<PlayerPitchingStatsByGame>
        {
            Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 4, 2), gameId: 1),
            Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 4, 3), gameId: 2)
        };
        var expectedFielding = new List<PlayerFieldingStatsByGame>
        {
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 4), gameId: 1),
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 5), gameId: 2)
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
    public async Task GetById_Id_ReturnsPlayerStatsBySeasonForId()
    {
        // Arrange
        var batting = new List<PlayerBattingStatsByGame>
        {
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 3, 31), gameId: 1),
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 4, 1), gameId: 2)
        };
        var pitching = new List<PlayerPitchingStatsByGame>
        {
            Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 4, 2), gameId: 1),
            Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 4, 3), gameId: 2)
        };
        var fielding = new List<PlayerFieldingStatsByGame>
        {
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 4), gameId: 1),
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 5), gameId: 2)
        };
        var fakePlayerStatsBySeason = Faker.FakePlayerStatsBySeason(1, 2024, battingScore: 0.1234m,
            pitchingScore: 0.2345m, fieldingScore: 0.3456m, battingStatsByGames: batting,
            pitchingStatsByGames: pitching, fieldingStatsByGames: fielding);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        await dbContext.AddAsync(fakePlayerStatsBySeason);
        await dbContext.SaveChangesAsync();

        await using var assertConnection =
            await GetDbConnection(); // Re-create the context so that the record is freshly retrieved from the database
        await using var assertDbContext = GetDbContext(assertConnection);
        await assertDbContext.Database.MigrateAsync();
        var repo = new EntityFrameworkCorePlayerStatsBySeasonRepository(assertDbContext);

        // Act
        var actual = await repo.GetById(fakePlayerStatsBySeason.Id);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(fakePlayerStatsBySeason, actual);
        Assert.Equal(0.1234m, actual.BattingScore.Value);
        Assert.Equal(0.2345m, actual.PitchingScore.Value);
        Assert.Equal(0.3456m, actual.FieldingScore.Value);
        Assert.Equal(batting, actual.BattingStatsByGamesChronologically);
        Assert.Equal(pitching, actual.PitchingStatsByGamesChronologically);
        Assert.Equal(fielding, actual.FieldingStatsByGamesChronologically);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetByMlbId_MlbId_ReturnsPlayerStatsBySeasonForMlbId()
    {
        // Arrange
        var batting = new List<PlayerBattingStatsByGame>
        {
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 3, 31), gameId: 1),
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 4, 1), gameId: 2)
        };
        var pitching = new List<PlayerPitchingStatsByGame>
        {
            Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 4, 2), gameId: 1),
            Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 4, 3), gameId: 2)
        };
        var fielding = new List<PlayerFieldingStatsByGame>
        {
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 4), gameId: 1),
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 5), gameId: 2)
        };
        var fakePlayerStatsBySeason = Faker.FakePlayerStatsBySeason(1, 2024, battingScore: 0.1234m,
            pitchingScore: 0.2345m, fieldingScore: 0.3456m, battingStatsByGames: batting,
            pitchingStatsByGames: pitching, fieldingStatsByGames: fielding);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        await dbContext.AddAsync(fakePlayerStatsBySeason);
        await dbContext.SaveChangesAsync();

        await using var assertConnection =
            await GetDbConnection(); // Re-create the context so that the record is freshly retrieved from the database
        await using var assertDbContext = GetDbContext(assertConnection);
        await assertDbContext.Database.MigrateAsync();
        var repo = new EntityFrameworkCorePlayerStatsBySeasonRepository(assertDbContext);

        // Act
        var actual = await repo.GetByMlbId(fakePlayerStatsBySeason.PlayerMlbId);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(fakePlayerStatsBySeason, actual);
        Assert.Equal(0.1234m, actual.BattingScore.Value);
        Assert.Equal(0.2345m, actual.PitchingScore.Value);
        Assert.Equal(0.3456m, actual.FieldingScore.Value);
        Assert.Equal(batting, actual.BattingStatsByGamesChronologically);
        Assert.Equal(pitching, actual.PitchingStatsByGamesChronologically);
        Assert.Equal(fielding, actual.FieldingStatsByGamesChronologically);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetAll_SeasonYear_ReturnsAllPlayerStatsBySeasonWithMatchingSeason()
    {
        // Arrange
        var seasonYear = SeasonYear.Create(2024);
        var batting = new List<PlayerBattingStatsByGame>
        {
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 3, 31), gameId: 1),
            Faker.FakePlayerBattingStats(gameDate: new DateOnly(2024, 4, 1), gameId: 2)
        };
        var pitching = new List<PlayerPitchingStatsByGame>
        {
            Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 4, 2), gameId: 1),
            Faker.FakePlayerPitchingStats(gameDate: new DateOnly(2024, 4, 3), gameId: 2)
        };
        var fielding = new List<PlayerFieldingStatsByGame>
        {
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 4), gameId: 1),
            Faker.FakePlayerFieldingStats(gameDate: new DateOnly(2024, 4, 5), gameId: 2)
        };
        var fakePlayerStatsBySeason = Faker.FakePlayerStatsBySeason(1, seasonYear.Value, battingStatsByGames: batting,
            pitchingStatsByGames: pitching, fieldingStatsByGames: fielding);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await dbContext.Database.MigrateAsync();

        await dbContext.AddAsync(fakePlayerStatsBySeason);
        await dbContext.SaveChangesAsync();

        await using var assertConnection =
            await GetDbConnection(); // Re-create the context so that the record is freshly retrieved from the database
        await using var assertDbContext = GetDbContext(assertConnection);
        await assertDbContext.Database.MigrateAsync();
        var repo = new EntityFrameworkCorePlayerStatsBySeasonRepository(assertDbContext);

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