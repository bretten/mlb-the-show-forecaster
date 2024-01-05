using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFramework;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Tests.TestClasses;
using Microsoft.EntityFrameworkCore;
using Moq;
using Npgsql;
using Testcontainers.PostgreSql;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Tests.Players;

public class EntityFrameworkPlayerRepositoryIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;

    public EntityFrameworkPlayerRepositoryIntegrationTests()
    {
        try
        {
            _container = new PostgreSqlBuilder()
                .WithName(GetType().Name)
                .WithUsername("postgres")
                .WithUsername("password99")
                .WithPortBinding(54320, 5432)
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
    public async Task Add_Player_PlayerIsAddedToRepository()
    {
        // Arrange
        var fakeTeam = Faker.FakeTeam();
        var fakePlayer = Faker.FakePlayer(team: fakeTeam);
        var stubTeamProvider = Mock.Of<ITeamProvider>(x => x.GetBy(fakeTeam.MlbId) == fakeTeam);

        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection, stubTeamProvider);
        await dbContext.Database.MigrateAsync();
        var repo = new EntityFrameworkPlayerRepository(dbContext);

        // Act
        await repo.Add(fakePlayer);

        // Assert
        Assert.NotNull(dbContext.Players);
        Assert.Equal(1, dbContext.Players.Count());
        Assert.Equal(fakePlayer, dbContext.Players.First());
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public async Task DisposeAsync() => await _container.StopAsync();

    private async Task<DbConnection> GetDbConnection()
    {
        NpgsqlConnection connection = new(_container.GetConnectionString());
        await connection.OpenAsync();
        return connection;
    }

    private PlayersDbContext GetDbContext(DbConnection connection, ITeamProvider stubTeamProvider)
    {
        var contextOptions = new DbContextOptionsBuilder<PlayersDbContext>()
            .UseNpgsql(connection)
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