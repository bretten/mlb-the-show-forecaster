using System.Globalization;
using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Execution.Host.Services;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Configuration;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher;

/// <summary>
/// Creates the host for tracking the MLB The Show cards and marketplace
/// </summary>
public static class HostCreator
{
    /// <summary>
    /// The background work that will be done by <see cref="ScheduledBackgroundService{T}"/> for the <see cref="IPlayerCardTracker"/>
    /// </summary>
    private static readonly Func<IPlayerCardTracker, IServiceProvider, Task> PlayerCardBackgroundWork =
        async (tracker, sp) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetRequiredService<ILogger<ScheduledBackgroundService<IPlayerCardTracker>>>();

            // Service name
            const string s = nameof(IPlayerCardTracker);

            // The seasons to track
            var seasons = config.GetRequiredValue<ushort[]>("PlayerCardTracker:Seasons");
            foreach (var season in seasons)
            {
                logger.LogInformation($"{s} - {season}");
                var result = await tracker.TrackPlayerCards(SeasonYear.Create(season));
                logger.LogInformation($"{s} - Total catalog cards = {result.TotalCatalogCards}");
                logger.LogInformation($"{s} - Total new catalog cards = {result.TotalNewCatalogCards}");
                logger.LogInformation($"{s} - Total existing player cards = {result.TotalExistingPlayerCards}");
            }
        };

    /// <summary>
    /// The background work that will be done by <see cref="ScheduledBackgroundService{T}"/> for the <see cref="ICardPriceTracker"/>
    /// </summary>
    private static readonly Func<ICardPriceTracker, IServiceProvider, Task> CardPriceBackgroundWork =
        async (tracker, sp) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetRequiredService<ILogger<ScheduledBackgroundService<ICardPriceTracker>>>();

            // Service name
            const string s = nameof(ICardPriceTracker);

            // The seasons to track
            var seasons = config.GetRequiredValue<ushort[]>("CardPriceTracker:Seasons");
            foreach (var season in seasons)
            {
                logger.LogInformation($"{s} - {season}");
                var result = await tracker.TrackCardPrices(SeasonYear.Create(season));
                logger.LogInformation($"{s} - Total cards = {result.TotalCards}");
                logger.LogInformation($"{s} - Total new listings = {result.TotalNewListings}");
                logger.LogInformation($"{s} - Total updated listings = {result.TotalUpdatedListings}");
                logger.LogInformation($"{s} - Total unchanged listings = {result.TotalUnchangedListings}");
            }
        };

    /// <summary>
    /// The background work that will be done by <see cref="ScheduledBackgroundService{T}"/> for the <see cref="IRosterUpdateOrchestrator"/>
    /// </summary>
    private static readonly Func<IRosterUpdateOrchestrator, IServiceProvider, Task> RosterUpdateBackgroundWork =
        async (rosterUpdater, sp) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetRequiredService<ILogger<ScheduledBackgroundService<IRosterUpdateOrchestrator>>>();

            // Service name
            const string s = nameof(IRosterUpdateOrchestrator);

            // The seasons to track
            var seasons = config.GetRequiredValue<ushort[]>("PlayerCardTracker:Seasons");
            foreach (var season in seasons)
            {
                logger.LogInformation($"{s} - {season}");
                var results = await rosterUpdater.SyncRosterUpdates(SeasonYear.Create(season));
                foreach (var result in results)
                {
                    logger.LogInformation($"{s} - Date = {result.Date}");
                    logger.LogInformation($"{s} - Total rating changes = {result.TotalRatingChanges}");
                    logger.LogInformation($"{s} - Total position changes = {result.TotalPositionChanges}");
                    logger.LogInformation($"{s} - Total new players = {result.TotalNewPlayers}");
                }
            }
        };

    /// <summary>
    /// The <see cref="IDomainEvent"/> types that will be published in this domain mapped to their corresponding
    /// RabbitMQ exchanges
    /// </summary>
    private static readonly Dictionary<Type, string> DomainEventPublisherTypes = new()
    {
        { typeof(PlayerCardOverallRatingDeclinedEvent), "PlayerCardOverallRatingDeclined" },
        { typeof(PlayerCardOverallRatingImprovedEvent), "PlayerCardOverallRatingImproved" },
        { typeof(PlayerCardPositionChangedEvent), "PlayerCardPositionChanged" },
        { typeof(ListingBuyPriceDecreasedEvent), "ListingBuyPriceDecreased" },
        { typeof(ListingBuyPriceIncreasedEvent), "ListingBuyPriceIncreased" },
        { typeof(ListingSellPriceDecreasedEvent), "ListingSellPriceDecreased" },
        { typeof(ListingSellPriceIncreasedEvent), "ListingSellPriceIncreased" }
    };

    /// <summary>
    /// The <see cref="IDomainEvent"/> types that will be consumed in this domain mapped to their corresponding
    /// RabbitMQ exchanges
    /// </summary>
    private static readonly Dictionary<Type, string> DomainEventConsumerTypes = new()
    {
    };

    /// <summary>
    /// Builds the <see cref="IHost"/> for this domain
    /// </summary>
    /// <param name="args">Command-line arguments</param>
    /// <returns>The built <see cref="IHost"/></returns>
    public static IHost Build(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                configurationBuilder.AddJsonFile("appsettings.json");
                configurationBuilder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json");
            })
            .ConfigureServices((context, services) =>
            {
                services.AddLogging();

                // Rabbit MQ
                var factory = new ConnectionFactory
                {
                    HostName = context.Configuration["Messaging:RabbitMq:HostName"],
                    UserName = context.Configuration["Messaging:RabbitMq:UserName"],
                    Password = context.Configuration["Messaging:RabbitMq:Password"],
                    Port = context.Configuration.GetValue<int>("Messaging:RabbitMq:Port"),
                    DispatchConsumersAsync = true
                };
                services.AddRabbitMq(factory, DomainEventPublisherTypes, DomainEventConsumerTypes, new List<Assembly>()
                {
                    typeof(PlayerCardOverallRatingDeclinedEvent).Assembly
                });

                // MLB The Show cards and marketplace dependencies
                services.AddGameCardsMapping();
                services.AddGameCardsPlayerCardTracker();
                services.AddGameCardsPriceTracker(context.Configuration);
                services.AddGameCardsRosterUpdates();
                services.AddGameCardsEntityFrameworkCoreRepositories(context.Configuration);

                // Background service for tracking player cards
                services.AddHostedService<ScheduledBackgroundService<IPlayerCardTracker>>(sp =>
                    new ScheduledBackgroundService<IPlayerCardTracker>(
                        sp.GetRequiredService<IServiceScopeFactory>(),
                        PlayerCardBackgroundWork,
                        TimeSpan.ParseExact(
                            context.Configuration.GetRequiredValue<string>("PlayerCardTracker:Interval"), "g",
                            CultureInfo.InvariantCulture)
                    ));
                // Background service for tracking marketplace card prices
                services.AddHostedService<ScheduledBackgroundService<ICardPriceTracker>>(sp =>
                    new ScheduledBackgroundService<ICardPriceTracker>(
                        sp.GetRequiredService<IServiceScopeFactory>(),
                        CardPriceBackgroundWork,
                        TimeSpan.ParseExact(
                            context.Configuration.GetRequiredValue<string>("CardPriceTracker:Interval"), "g",
                            CultureInfo.InvariantCulture)
                    ));
                // Background service for tracking marketplace card prices
                services.AddHostedService<ScheduledBackgroundService<IRosterUpdateOrchestrator>>(sp =>
                    new ScheduledBackgroundService<IRosterUpdateOrchestrator>(
                        sp.GetRequiredService<IServiceScopeFactory>(),
                        RosterUpdateBackgroundWork,
                        TimeSpan.ParseExact(
                            context.Configuration.GetRequiredValue<string>("PlayerCardTracker:Interval"), "g",
                            CultureInfo.InvariantCulture)
                    ));
            });

        return builder.Build();
    }
}