using System.Data.Common;
using System.Diagnostics;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Cards.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;
using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RabbitMQ.Client;
using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Tests;

public class ProgramIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    private readonly RabbitMqContainer _rabbitMqContainer;
    private readonly MongoDbContainer _mongoDbContainer;

    private const int PostgreSqlPort = 5432;
    private const int RabbitMqPort = 5672;
    private const string MongoUser = "mongo";
    private const string MongoPass = "password99";
    private const int MongoPort = 27017;

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
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilPortIsAvailable(PostgreSqlPort, o => o.WithTimeout(TimeSpan.FromMinutes(1)))
                    .UntilCommandIsCompleted(["pg_isready", "-U", "postgres", "-d", "postgres"],
                        o => o.WithTimeout(TimeSpan.FromMinutes(1)))
                )
                .Build();
            _rabbitMqContainer = new RabbitMqBuilder()
                .WithImage("rabbitmq:3-management")
                .WithName(GetType().Name + Guid.NewGuid())
                .WithPortBinding(RabbitMqPort, true)
                .WithPortBinding(15672, true)
                .WithCommand("rabbitmq-server", "rabbitmq-plugins enable --offline rabbitmq_management")
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilPortIsAvailable(RabbitMqPort, o => o.WithTimeout(TimeSpan.FromMinutes(1)))
                )
                .Build();
            _mongoDbContainer = new MongoDbBuilder()
                .WithName(GetType().Name + Guid.NewGuid())
                .WithUsername(MongoUser)
                .WithPassword(MongoPass)
                .WithPortBinding(MongoPort, true)
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilPortIsAvailable(MongoPort, o => o.WithTimeout(TimeSpan.FromMinutes(1)))
                )
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
    public async Task Program_MarketplaceWatcher_ExecutesAndAddsPlayerCardsAndListings()
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
        builder.Configuration["ConnectionStrings:Cards"] = _dbContainer.GetConnectionString() + ";Pooling=false;";
        builder.Configuration["ConnectionStrings:Forecasts"] = _dbContainer.GetConnectionString() + ";Pooling=false;";
        builder.Configuration["ConnectionStrings:Marketplace"] = _dbContainer.GetConnectionString() + ";Pooling=false;";
        builder.Configuration["ConnectionStrings:TrendsMongoDb"] = _mongoDbContainer.GetConnectionString();
        builder.Configuration["Messaging:RabbitMq:UserName"] = "rabbitmq"; // Default for RabbitMqBuilder
        builder.Configuration["Messaging:RabbitMq:Password"] = "rabbitmq";
        builder.Configuration["Messaging:RabbitMq:Port"] = HostRabbitMqPort.ToString();
        // Build the app
        var app = AppBuilder.BuildApp(args, builder);

        // Setup the cards database
        await using var connection = await GetDbConnection();
        await using var cardsDbContext = GetCardsDbContext(connection);
        await cardsDbContext.Database.MigrateAsync();
        // Add a PlayerCard (and a listing below) so the ICardPriceTracker can update the listing and dispatch price change domain events
        var playerCardExternalId1 = Guid.Parse("7d6c7d95a1e5e861c54d20002585a809");
        await cardsDbContext.PlayerCards.AddAsync(Faker.FakePlayerCard(externalId: playerCardExternalId1));
        // Add another PlayerCard (with no listing) so the ICardPriceTracker can create a new listing
        var playerCardExternalId2 = Guid.Parse("da757117dff1551f109453a8b80f28c8");
        await cardsDbContext.PlayerCards.AddAsync(Faker.FakePlayerCard(externalId: playerCardExternalId2));
        await cardsDbContext.SaveChangesAsync();

        // Setup the marketplace database
        await using var marketplaceDbContext = GetMarketplaceDbContext(connection);
        await marketplaceDbContext.Database.MigrateAsync();
        // Add a Listing so price change domain events can be dispatched
        await marketplaceDbContext.Listings.AddAsync(
            Domain.Tests.Marketplace.TestClasses.Faker.FakeListing(playerCardExternalId1));
        await marketplaceDbContext.SaveChangesAsync();

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
        var conditionsMet = false;
        var timeLimit = new TimeSpan(0, 1, 0);
        var stopwatch = Stopwatch.StartNew();
        while (!conditionsMet)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), CancellationToken.None);
            if (stopwatch.Elapsed > timeLimit)
            {
                throw new TimeoutException(
                    $"Timeout waiting {nameof(Program_MarketplaceWatcher_ExecutesAndAddsPlayerCardsAndListings)}");
            }

            // There should be more than one player cards
            await using var assertConnection = await GetDbConnection();
            await using var assertCardsDbContext = GetCardsDbContext(assertConnection);
            var playerCards = assertCardsDbContext.PlayerCards.Count();
            // Make sure the cards inserted above exist
            // NOTE: The time it takes to get all the MLB cards exceeds the test time, so IPlayerCardTracker may not have time to update the cards
            var playerCardsSaved = playerCards >= 2;
            // There should be marketplace listings
            await using var assertMarketplaceDbContext = GetMarketplaceDbContext(assertConnection);
            var listings = assertMarketplaceDbContext.Listings.Count();
            var listingsSaved = listings > 1; // One was already inserted by the setup of this test
            // Domain events should have been published
            using var rabbitMqChannel = GetRabbitMqModel(app.Configuration);
            var messageCount = rabbitMqChannel.MessageCount("cards.listings.buy_price_decreased") +
                               rabbitMqChannel.MessageCount("cards.listings.buy_price_increased");
            var messagesPublished = messageCount > 0;

            conditionsMet = playerCardsSaved && listingsSaved && messagesPublished;
        }

        stopwatch.Stop();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();
        await _mongoDbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
        await _rabbitMqContainer.DisposeAsync();
        await _mongoDbContainer.DisposeAsync();
    }

    private async Task<NpgsqlConnection> GetDbConnection()
    {
        NpgsqlConnection connection = new(_dbContainer.GetConnectionString() + ";Pooling=false;");
        await connection.OpenAsync();
        return connection;
    }

    private CardsDbContext GetCardsDbContext(DbConnection connection)
    {
        var contextOptions = new DbContextOptionsBuilder<CardsDbContext>()
            .UseNpgsql(connection)
            .Options;
        return new CardsDbContext(contextOptions);
    }

    private MarketplaceDbContext GetMarketplaceDbContext(DbConnection connection)
    {
        var contextOptions = new DbContextOptionsBuilder<MarketplaceDbContext>()
            .UseNpgsql(connection)
            .Options;
        return new MarketplaceDbContext(contextOptions);
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