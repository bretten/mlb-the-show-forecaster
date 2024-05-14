using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using RabbitMQ.Client;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.Tests;

public class ProgramIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    private readonly RabbitMqContainer _rabbitMqContainer;

    public ProgramIntegrationTests()
    {
        try
        {
            _dbContainer = new PostgreSqlBuilder()
                .WithName(GetType().Name + Guid.NewGuid())
                .WithUsername("postgres")
                .WithPassword("password99")
                .WithPortBinding(54325, 5432)
                .Build();
            _rabbitMqContainer = new RabbitMqBuilder()
                .WithImage("rabbitmq:3-management")
                .WithName(GetType().Name + Guid.NewGuid())
                .WithPortBinding(56721, 5672)
                .WithPortBinding(15674, 15672)
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
    public async Task Program_PlayerTracker_ExecutesAndAddsPlayers()
    {
        /*
         * Arrange
         */
        var args = Array.Empty<string>();

        // Key-value pairs for IConfiguration
        var inMemoryConfig = new Dictionary<string, string>()
        {
            { "ConnectionStrings:Players", _dbContainer.GetConnectionString() + ";Pooling=false;" },
            { "PlayerStatusTracker:Interval", "01:00:00:00" },
            { "PlayerStatusTracker:Seasons:0", "2024" },
            { "Api:Mlb:BaseAddress", "https://statsapi.mlb.com/api" },
            { "Messaging:RabbitMq:HostName", "localhost" },
            { "Messaging:RabbitMq:UserName", "rabbitmq" },
            { "Messaging:RabbitMq:Password", "rabbitmq" },
            { "Messaging:RabbitMq:Port", "56721" },
        };

        // Build the host
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                configurationBuilder.AddInMemoryCollection(inMemoryConfig!);
            })
            .ConfigurePlayerTracker(args); // Configure PlayerTracker
        var host = builder.Build();

        // Setup the database
        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection, new TeamProvider());
        await CreateSchema(connection);
        await dbContext.Database.MigrateAsync();

        /*
         * Act
         */
        // Start the host
        await host.StartAsync();
        // Let it do some work
        await Task.Delay(TimeSpan.FromSeconds(3));
        // Stop the host
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        await host.StopAsync(cts.Token);

        /*
         * Assert
         */
        // Some Players should have been added to the DB
        await using var assertConnection = await GetDbConnection();
        await using var assertDbContext = GetDbContext(connection, new TeamProvider());
        var players = assertDbContext.Players.Count();
        Assert.True(players > 0);
        // Domain events should have been published
        using var rabbitMqChannel = host.Services.GetRequiredService<IModel>();
        var messageCount = rabbitMqChannel.MessageCount("PlayerActivated");
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

    private PlayersDbContext GetDbContext(DbConnection connection, ITeamProvider teamProvider)
    {
        var contextOptions = new DbContextOptionsBuilder<PlayersDbContext>()
            .UseNpgsql(connection)
            .Options;
        return new PlayersDbContext(contextOptions, teamProvider);
    }

    private async Task CreateSchema(NpgsqlConnection connection)
    {
        await using var cmd = new NpgsqlCommand("CREATE SCHEMA players;", connection);
        await cmd.ExecuteNonQueryAsync();
    }

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}