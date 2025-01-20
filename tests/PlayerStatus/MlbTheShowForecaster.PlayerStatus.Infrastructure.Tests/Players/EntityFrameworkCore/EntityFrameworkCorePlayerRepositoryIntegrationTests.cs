using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Teams.TestClasses;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Moq;
using Npgsql;
using Testcontainers.PostgreSql;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Tests.Players.EntityFrameworkCore;

/// <summary>
/// Integration tests that use TestContainers to spin up database containers for the tests. Requires Docker to be
/// running for these tests
/// </summary>
public class EntityFrameworkCorePlayerRepositoryIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    /// <summary>
    /// Configures the container options that will be used for each test
    /// </summary>
    /// <exception cref="DockerNotRunningException">Thrown if Docker is not running</exception>
    public EntityFrameworkCorePlayerRepositoryIntegrationTests()
    {
        try
        {
            _container = new PostgreSqlBuilder()
                .WithImage("postgres:15-alpine") // Has support for locale-provider=icu
                .WithEnvironment("POSTGRES_INITDB_ARGS", "--locale-provider=icu --icu-locale=en-US")
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
    public async Task Add_Player_AddsPlayerToDbContextSet()
    {
        // Arrange
        var fakeTeam = Faker.FakeTeam();
        var fakePlayer = Domain.Tests.Players.TestClasses.Faker.FakePlayer(team: fakeTeam);
        var stubTeamProvider = Mock.Of<ITeamProvider>(x => x.GetBy(fakeTeam.Abbreviation) == fakeTeam);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection, stubTeamProvider);
        await dbContext.Database.MigrateAsync();
        var repo = new EntityFrameworkCorePlayerRepository(dbContext);

        // Act
        await repo.Add(fakePlayer);
        await dbContext.SaveChangesAsync();

        // Assert
        Assert.NotNull(dbContext.Players);
        Assert.Equal(1, dbContext.Players.Count());
        Assert.Equal(fakePlayer, dbContext.Players.First());
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task Update_ExistingPlayer_UpdatesPlayerInDbContextSet()
    {
        // Arrange
        var year = SeasonYear.Create(2024);
        var fakeTeam = Faker.FakeTeam();
        var fakePlayer = Domain.Tests.Players.TestClasses.Faker.FakePlayer(active: false, team: fakeTeam);
        var stubTeamProvider = Mock.Of<ITeamProvider>(x => x.GetBy(fakeTeam.Abbreviation) == fakeTeam);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection, stubTeamProvider);
        await dbContext.Database.MigrateAsync();

        await dbContext.AddAsync(fakePlayer);
        await dbContext.SaveChangesAsync();

        var repo = new EntityFrameworkCorePlayerRepository(dbContext);

        // Act
        fakePlayer.Activate(year, new DateOnly(2024, 10, 28));
        await repo.Update(fakePlayer);
        await dbContext.SaveChangesAsync();

        // Assert
        Assert.Equal(fakePlayer, dbContext.Players.First());
        Assert.True(fakePlayer.Active);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetByMlbId_PlayerMlbId_ReturnsPlayerFromDbContextSet()
    {
        // Arrange
        var fakeTeam = Faker.FakeTeam();
        var fakePlayer = Domain.Tests.Players.TestClasses.Faker.FakePlayer(team: fakeTeam);
        var stubTeamProvider = Mock.Of<ITeamProvider>(x => x.GetBy(fakeTeam.Abbreviation) == fakeTeam);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection, stubTeamProvider);
        await dbContext.Database.MigrateAsync();

        await dbContext.AddAsync(fakePlayer);
        await dbContext.SaveChangesAsync();

        var repo = new EntityFrameworkCorePlayerRepository(dbContext);

        // Act
        var actual = await repo.GetByMlbId(fakePlayer.MlbId);

        // Assert
        Assert.Equal(fakePlayer, actual);
    }

    [Theory]
    // Both db and query accent are equal
    [InlineData("áéíóúñ", "áéíóúñ", "áéíóúñ", "áéíóúñ")]
    [InlineData("aeioun", "aeioun", "aeioun", "aeioun")]
    // Db accent is different than query
    [InlineData("áéíóúñ", "áéíóúñ", "aeioun", "aeioun")]
    [InlineData("aeioun", "aeioun", "áéíóúñ", "áéíóúñ")]
    // DB accent and case is different than query
    [InlineData("áéíóúñ", "áéíóúñ", "AEIOUN", "AEIOUN")]
    [InlineData("AEIOUN", "AEIOUN", "áéíóúñ", "áéíóúñ")]
    [Trait("Category", "Integration")]
    public async Task GetAllByName_FirstAndLastName_ReturnsPlayerFromDbContextSet(string firstName, string lastName,
        string firstNameQuery, string lastNameQuery)
    {
        // Arrange
        var fakeTeam = Faker.FakeTeam();
        var fakePlayer =
            Domain.Tests.Players.TestClasses.Faker.FakePlayer(firstName: firstName, lastName: lastName, team: fakeTeam);
        var stubTeamProvider = Mock.Of<ITeamProvider>(x => x.GetBy(fakeTeam.Abbreviation) == fakeTeam);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection, stubTeamProvider);
        await dbContext.Database.MigrateAsync();

        await dbContext.AddAsync(fakePlayer);
        await dbContext.SaveChangesAsync();

        var repo = new EntityFrameworkCorePlayerRepository(dbContext);

        // Act
        var actual = await repo.GetAllByName(firstName: PersonName.Create(firstNameQuery),
            lastName: PersonName.Create(lastNameQuery));

        // Assert
        Assert.Equal(fakePlayer, actual.First());
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public async Task DisposeAsync() => await _container.DisposeAsync();

    private async Task<DbConnection> GetDbConnection()
    {
        NpgsqlConnection connection = new(_container.GetConnectionString() + ";Pooling=false;");
        await connection.OpenAsync();
        return connection;
    }

    private PlayersDbContext GetDbContext(DbConnection connection, ITeamProvider stubTeamProvider)
    {
        var contextOptions = new DbContextOptionsBuilder<PlayersDbContext>()
            .UseNpgsql(connection,
                builder => { builder.MigrationsHistoryTable(Constants.MigrationsTable, Constants.Schema); })
            .LogTo(Console.WriteLine)
            .EnableSensitiveDataLogging()
            .Options;
        return new PlayersDbContext(contextOptions, stubTeamProvider);
    }

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}