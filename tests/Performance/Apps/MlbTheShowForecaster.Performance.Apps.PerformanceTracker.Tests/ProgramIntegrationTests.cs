using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using RabbitMQ.Client;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Tests;

public class ProgramIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    private readonly RabbitMqContainer _rabbitMqContainer;

    private const int PostgreSqlPort = 54326;
    private const int RabbitMqPort = 56722;

    public ProgramIntegrationTests()
    {
        try
        {
            _dbContainer = new PostgreSqlBuilder()
                .WithName(GetType().Name + Guid.NewGuid())
                .WithUsername("postgres")
                .WithPassword("password99")
                .WithPortBinding(PostgreSqlPort, 5432)
                .Build();
            _rabbitMqContainer = new RabbitMqBuilder()
                .WithImage("rabbitmq:3-management")
                .WithName(GetType().Name + Guid.NewGuid())
                .WithPortBinding(RabbitMqPort, 5672)
                .WithPortBinding(15675, 15672)
                .WithCommand("rabbitmq-server", "rabbitmq-plugins enable --offline rabbitmq_management")
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
    public async Task Program_PerformanceTracker_ExecutesAndAddsSeasonStats()
    {
        /*
         * Arrange
         */
        // Command line arguments when running the program
        var args = Array.Empty<string>();

        // Builder
        var builder = AppBuilder.CreateBuilder(args);
        // Config overrides
        builder.Configuration["ConnectionStrings:PlayerSeasons"] =
            _dbContainer.GetConnectionString() + ";Pooling=false;";
        builder.Configuration["Messaging:RabbitMq:Port"] = RabbitMqPort.ToString();
        // Build the app
        var app = AppBuilder.BuildApp(args, builder);

        // Setup the database
        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection);
        await CreateSchema(connection);
        await dbContext.Database.MigrateAsync();

        /*
         * Act
         */
        // Cancellation token to stop the program
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        // Start the host
        await app.StartAsync(cts.Token);
        // Let it do some work
        await Task.Delay(TimeSpan.FromSeconds(3), cts.Token);
        // Stop the host
        await app.StopAsync(cts.Token);

        /*
         * Assert
         */
        // The player season should have performance stats
        await using var assertConnection = await GetDbConnection();
        await using var assertDbContext = GetDbContext(connection);
        var playerSeasons = assertDbContext.PlayerStatsBySeasonsWithGames().ToList();
        var playerSeason = playerSeasons.FirstOrDefault(x =>
            x.BattingStatsByGamesChronologically.Count > 0 || x.PitchingStatsByGamesChronologically.Count > 0 ||
            x.FieldingStatsByGamesChronologically.Count > 0);
        Assert.NotNull(playerSeason);
        // Domain events should have been published
        using var rabbitMqChannel = app.Services.GetRequiredService<IModel>();
        var messageCount = rabbitMqChannel.MessageCount("PlayerBattedInGame")
                           + rabbitMqChannel.MessageCount("PlayerPitchedInGame")
                           + rabbitMqChannel.MessageCount("PlayerFieldedInGame");
        Assert.True(messageCount > 0);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _rabbitMqContainer.StopAsync();
    }

    private async Task<NpgsqlConnection> GetDbConnection()
    {
        NpgsqlConnection connection = new(_dbContainer.GetConnectionString() + ";Pooling=false;");
        await connection.OpenAsync();
        return connection;
    }

    private PlayerSeasonsDbContext GetDbContext(DbConnection connection)
    {
        var contextOptions = new DbContextOptionsBuilder<PlayerSeasonsDbContext>()
            .UseNpgsql(connection)
            .Options;
        return new PlayerSeasonsDbContext(contextOptions);
    }

    private async Task CreateSchema(NpgsqlConnection connection)
    {
        await using var cmd = new NpgsqlCommand("CREATE SCHEMA performance;", connection);
        await cmd.ExecuteNonQueryAsync();
    }

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}