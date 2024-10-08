using System.Globalization;
using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Execution.Host.Services;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Configuration;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.FileSystems;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.BattingStatsChange;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.FieldingStatsChange;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.OverallRatingChange;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PitchingStatsChange;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerActivation;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerCardBoosted;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerDeactivation;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerFreeAgency;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerTeamSigning;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PositionChange;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher;

/// <summary>
/// Creates the host for tracking the MLB The Show cards and marketplace
/// </summary>
public static class MarketplaceWatcherHostExtensions
{
    /// <summary>
    /// The background work that will be done by <see cref="ScheduledBackgroundService{T}"/> for the <see cref="IPlayerCardTracker"/>
    /// </summary>
    private static readonly Func<IPlayerCardTracker, IServiceProvider, CancellationToken, Task>
        PlayerCardBackgroundWork = async (tracker, sp, ct) =>
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
                var result = await tracker.TrackPlayerCards(SeasonYear.Create(season), ct);
                logger.LogInformation($"{s} - Total catalog cards = {result.TotalCatalogCards}");
                logger.LogInformation($"{s} - Total new catalog cards = {result.TotalNewCatalogCards}");
                logger.LogInformation($"{s} - Total updated player cards = {result.TotalUpdatedPlayerCards}");
                logger.LogInformation($"{s} - Total unchanged player cards = {result.TotalUnchangedPlayerCards}");
            }
        };

    /// <summary>
    /// The background work that will be done by <see cref="ScheduledBackgroundService{T}"/> for the <see cref="ICardPriceTracker"/>
    /// </summary>
    private static readonly Func<ICardPriceTracker, IServiceProvider, CancellationToken, Task> CardPriceBackgroundWork =
        async (tracker, sp, ct) =>
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
                var result = await tracker.TrackCardPrices(SeasonYear.Create(season), ct);
                logger.LogInformation($"{s} - Total cards = {result.TotalCards}");
                logger.LogInformation($"{s} - Total new listings = {result.TotalNewListings}");
                logger.LogInformation($"{s} - Total updated listings = {result.TotalUpdatedListings}");
                logger.LogInformation($"{s} - Total unchanged listings = {result.TotalUnchangedListings}");
            }
        };

    /// <summary>
    /// The background work that will be done by <see cref="ScheduledBackgroundService{T}"/> for the <see cref="IRosterUpdateOrchestrator"/>
    /// </summary>
    private static readonly Func<IRosterUpdateOrchestrator, IServiceProvider, CancellationToken, Task>
        RosterUpdateBackgroundWork = async (rosterUpdater, sp, ct) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetRequiredService<ILogger<ScheduledBackgroundService<IRosterUpdateOrchestrator>>>();
            var historyService = sp.GetRequiredService<IPlayerRatingHistoryService>();

            // Service name
            const string s = nameof(IRosterUpdateOrchestrator);
            const string h = nameof(IPlayerRatingHistoryService);

            // The seasons to track
            var seasons = config.GetRequiredValue<ushort[]>("PlayerCardTracker:Seasons");
            foreach (var season in seasons)
            {
                logger.LogInformation($"{s} - {season}");

                // Sync the player card historical ratings before running roster updates
                var historyResult = await historyService.SyncHistory(SeasonYear.Create(season), ct);
                logger.LogInformation($"{h} - Total cards updated = {historyResult.UpdatedPlayerCards.Count()}");
                foreach (var historicalUpdate in historyResult.UpdatedPlayerCards)
                {
                    logger.LogInformation(
                        $"{h} - Updated {historicalUpdate.Name.Value}, {historicalUpdate.ExternalId.Value}, {historicalUpdate.Id}");
                }

                var results = await rosterUpdater.SyncRosterUpdates(SeasonYear.Create(season), ct);
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
        { typeof(NewPlayerCardEvent), "NewPlayerCard" },
        { typeof(PlayerCardOverallRatingDeclinedEvent), "PlayerCardOverallRatingDeclined" },
        { typeof(PlayerCardOverallRatingImprovedEvent), "PlayerCardOverallRatingImproved" },
        { typeof(PlayerCardBoostedEvent), "PlayerCardBoostedEvent" },
        { typeof(PlayerCardPositionChangedEvent), "PlayerCardPositionChanged" },
        { typeof(CardDemandIncreasedEvent), "CardDemandIncreasedEvent" },
        { typeof(CardDemandDecreasedEvent), "CardDemandDecreasedEvent" },
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
        { typeof(NewPlayerCardEvent), "NewPlayerCard" },

        { typeof(BattingStatsImprovementEvent), "BattingImprovement" },
        { typeof(BattingStatsDeclineEvent), "BattingDecline" },
        { typeof(PitchingStatsImprovementEvent), "PitchingImprovement" },
        { typeof(PitchingStatsDeclineEvent), "PitchingDecline" },
        { typeof(FieldingStatsImprovementEvent), "FieldingImprovement" },
        { typeof(FieldingStatsDeclineEvent), "FieldingDecline" },

        { typeof(OverallRatingImprovementEvent), "PlayerCardOverallRatingImproved" },
        { typeof(OverallRatingDeclineEvent), "PlayerCardOverallRatingDeclined" },
        { typeof(PositionChangeEvent), "PlayerCardPositionChanged" },

        { typeof(PlayerCardBoostEvent), "PlayerCardBoostedEvent" },
        { typeof(PlayerActivationEvent), "PlayerActivated" },
        { typeof(PlayerFreeAgencyEvent), "PlayerEnteredFreeAgency" },
        { typeof(PlayerDeactivationEvent), "PlayerInactivated" },
        { typeof(PlayerTeamSigningEvent), "PlayerSignedContractWithTeam" },
    };

    /// <summary>
    /// Builds the <see cref="IHost"/> for this domain
    /// </summary>
    /// <param name="builder"><see cref="IHostBuilder"/></param>
    /// <param name="args">Command-line arguments</param>
    /// <returns>The <see cref="IHostBuilder"/> with the marketplace watcher configured on it</returns>
    public static IHostBuilder ConfigureMarketplaceWatcher(this IHostBuilder builder, string[] args)
    {
        builder.ConfigureServices((context, services) =>
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
                typeof(PlayerCardOverallRatingDeclinedEvent).Assembly,
                typeof(BattingStatsImprovementEvent).Assembly
            });

            // MLB The Show cards and marketplace dependencies
            services.AddFileSystems(context.Configuration);
            services.AddGameCardsMapping();
            services.AddGameCardsPlayerCardTracker();
            services.AddGameCardsPriceTracker(context.Configuration);
            services.AddGameCardsRosterUpdates();
            services.AddForecasting(context.Configuration);
            services.AddTrendReporting(context.Configuration);
            services.AddGameCardsEntityFrameworkCoreRepositories(context.Configuration);

            // Register the domain event consumers
            foreach (var consumerEvent in DomainEventConsumerTypes)
            {
                var rabbitMqConsumerWrapperType =
                    typeof(RabbitMqDomainEventConsumer<>).MakeGenericType(consumerEvent.Key);
                var keepAliveServiceType =
                    typeof(KeepAliveBackgroundService<>).MakeGenericType(rabbitMqConsumerWrapperType);

                // The following mimics services.AddHostedService()
                var descriptor = ServiceDescriptor.DescribeKeyed(typeof(IHostedService), null, keepAliveServiceType,
                    ServiceLifetime.Singleton);
                services.TryAddEnumerable(descriptor);
            }

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
            // Background service for tracking roster updates
            services.AddHostedService<ScheduledBackgroundService<IRosterUpdateOrchestrator>>(sp =>
                new ScheduledBackgroundService<IRosterUpdateOrchestrator>(
                    sp.GetRequiredService<IServiceScopeFactory>(),
                    RosterUpdateBackgroundWork,
                    TimeSpan.ParseExact(
                        context.Configuration.GetRequiredValue<string>("PlayerCardTracker:Interval"), "g",
                        CultureInfo.InvariantCulture)
                ));
        });

        return builder;
    }
}