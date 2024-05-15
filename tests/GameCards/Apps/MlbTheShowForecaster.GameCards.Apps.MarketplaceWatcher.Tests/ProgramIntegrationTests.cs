using System.Data.Common;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Cards.EntityFrameworkCore;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Marketplace.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using RabbitMQ.Client;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Tests;

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
                .WithPortBinding(54327, 5432)
                .Build();
            _rabbitMqContainer = new RabbitMqBuilder()
                .WithImage("rabbitmq:3-management")
                .WithName(GetType().Name + Guid.NewGuid())
                .WithPortBinding(56723, 5672)
                .WithPortBinding(15676, 15672)
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
    public async Task Program_MarketplaceWatcher_ExecutesAndAddsPlayerCardsAndListings()
    {
        /*
         * Arrange
         */
        // Command line arguments when running the program
        var args = Array.Empty<string>();

        // Key-value pairs for IConfiguration
        var inMemoryConfig = new Dictionary<string, string>()
        {
            { "ConnectionStrings:Cards", _dbContainer.GetConnectionString() + ";Pooling=false;" },
            { "ConnectionStrings:Marketplace", _dbContainer.GetConnectionString() + ";Pooling=false;" },
            { "PlayerCardTracker:Interval", "01:00:00:00" },
            { "PlayerCardTracker:Seasons:0", "2024" },
            { "CardPriceTracker:Interval", "01:00:00:00" },
            { "CardPriceTracker:Seasons:0", "2024" },
            { "CardPriceTracker:BuyPricePercentageChangeThreshold", "15" },
            { "CardPriceTracker:SellPricePercentageChangeThreshold", "15" },
            { "Messaging:RabbitMq:HostName", "localhost" },
            { "Messaging:RabbitMq:UserName", "rabbitmq" },
            { "Messaging:RabbitMq:Password", "rabbitmq" },
            { "Messaging:RabbitMq:Port", "56723" },
        };

        // Build the host
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                configurationBuilder.AddInMemoryCollection(inMemoryConfig!);
            })
            .ConfigureMarketplaceWatcher(args); // Configure MarketplaceWatcher
        var host = builder.Build();

        // Setup the cards database
        await using var connection = await GetDbConnection();
        await using var cardsDbContext = GetCardsDbContext(connection);
        await CreateSchema(connection);
        await cardsDbContext.Database.MigrateAsync();

        // Setup the marketplace database
        await using var marketplaceDbContext = GetMarketplaceDbContext(connection);
        await marketplaceDbContext.Database.MigrateAsync();

        /*
         * Act
         */
        // Cancellation token to stop the program
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(25));
        // Start the host
        await host.StartAsync(cts.Token);
        // Let it do some work
        await Task.Delay(TimeSpan.FromSeconds(10), cts.Token);
        // Stop the host
        await host.StopAsync(cts.Token);

        /*
         * Assert
         */
        // There should be player cards
        await using var assertConnection = await GetDbConnection();
        await using var assertCardsDbContext = GetCardsDbContext(connection);
        var playerCards = assertCardsDbContext.PlayerCards.Count();
        Assert.True(playerCards > 0);
        // There should be marketplace listings
        await using var assertMarketplaceDbContext = GetMarketplaceDbContext(connection);
        var listings = assertMarketplaceDbContext.Listings.Count();
        Assert.True(listings > 0);
        // Domain events should have been published
        using var rabbitMqChannel = host.Services.GetRequiredService<IModel>();
        var messageCount = rabbitMqChannel.MessageCount("PlayerCardOverallRatingImproved");
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

    private async Task CreateSchema(NpgsqlConnection connection)
    {
        await using var cmd = new NpgsqlCommand("CREATE SCHEMA game_cards;", connection);
        await cmd.ExecuteNonQueryAsync();
    }

    private sealed class DockerNotRunningException : Exception
    {
        public DockerNotRunningException(string? message) : base(message)
        {
        }
    }
}