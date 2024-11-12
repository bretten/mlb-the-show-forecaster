using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Tests.TestClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RabbitMQ.Client;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.Tests;

public class ProgramIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    private readonly RabbitMqContainer _rabbitMqContainer;

    private const int PostgreSqlPort = 5432;
    private const int RabbitMqPort = 5672;

    private int HostRabbitMqPort => _rabbitMqContainer.GetMappedPublicPort(RabbitMqPort);

    public ProgramIntegrationTests()
    {
        try
        {
            _dbContainer = new PostgreSqlBuilder()
                .WithName(GetType().Name + Guid.NewGuid())
                .WithUsername("postgres")
                .WithPassword("password99")
                .WithPortBinding(PostgreSqlPort, true)
                .Build();
            _rabbitMqContainer = new RabbitMqBuilder()
                .WithImage("rabbitmq:3-management")
                .WithName(GetType().Name + Guid.NewGuid())
                .WithPortBinding(RabbitMqPort, true)
                .WithPortBinding(15672, true)
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
    public async Task Program_PlayerTracker_ExecutesAndAddsPlayers()
    {
        /*
         * Arrange
         */
        // Command line arguments when running the program
        var args = Array.Empty<string>();

        // Builder
        var builder = AppBuilder.CreateBuilder(args);
        // Config overrides
        builder.Configuration["Jobs:RunOnStartup"] = "true";
        builder.Configuration["ConnectionStrings:Players"] = _dbContainer.GetConnectionString() + ";Pooling=false;";
        builder.Configuration["Messaging:RabbitMq:UserName"] = "rabbitmq"; // Default for RabbitMqBuilder
        builder.Configuration["Messaging:RabbitMq:Password"] = "rabbitmq";
        builder.Configuration["Messaging:RabbitMq:Port"] = HostRabbitMqPort.ToString();
        // Build the app
        var app = AppBuilder.BuildApp(args, builder);

        // Setup the database
        await using var connection = await GetDbConnection();
        await using var dbContext = GetDbContext(connection, new TeamProvider());
        await dbContext.Database.MigrateAsync();
        // Add an existing player so it can be activated (first player alphabetically in the 2024 season)
        var player = Faker.FakePlayer(mlbId: 592450, team: Faker.FakeTeam(), active: false);
        await dbContext.Players.AddAsync(player);
        await dbContext.SaveChangesAsync();

        // Will be used for asserting
        using var rabbitMqChannel = GetRabbitMqModel(app.Configuration);

        /*
         * Act
         */
        // Start the host
        _ = app.RunAsync();
        // Let it do some work
        await Task.Delay(TimeSpan.FromSeconds(3), CancellationToken.None);

        /*
         * Assert
         */
        // Some Players should have been added to the DB
        await using var assertConnection = await GetDbConnection();
        await using var assertDbContext = GetDbContext(assertConnection, new TeamProvider());
        var players = assertDbContext.Players.Count();
        Assert.True(players > 0);
        // Domain events should have been published
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
        await _dbContainer.DisposeAsync();
        await _rabbitMqContainer.DisposeAsync();
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

    private IModel GetRabbitMqModel(IConfiguration config)
    {
        return new ConnectionFactory
        {
            HostName = config["Messaging:RabbitMq:HostName"],
            UserName = config["Messaging:RabbitMq:UserName"],
            Password = config["Messaging:RabbitMq:Password"],
            Port = config.GetValue<int>("Messaging:RabbitMq:Port"),
            DispatchConsumersAsync = true
        }.CreateConnection().CreateModel();
    }

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}