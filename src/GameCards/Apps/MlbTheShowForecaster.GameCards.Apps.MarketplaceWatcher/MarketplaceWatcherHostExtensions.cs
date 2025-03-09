using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.Common.Application.RealTime;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Execution.Host.Services;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.FileSystems;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Jobs;
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
using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.RealTime;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;
using Dependencies = com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Jobs.Dependencies;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher;

/// <summary>
/// Creates the host for tracking the MLB The Show cards and marketplace
/// </summary>
public static class MarketplaceWatcherHostExtensions
{
    /// <summary>
    /// Rabbit MQ exchange
    /// </summary>
    private const string Exchange = "mlb-forecaster";

    /// <summary>
    /// The <see cref="IDomainEvent"/> types that will be published in this domain mapped to their corresponding
    /// RabbitMQ exchanges
    /// </summary>
    private static readonly Dictionary<Type, Publisher> DomainEventPublisherTypes = new()
    {
        { typeof(NewPlayerCardEvent), new Publisher(Exchange, "cards.players.created") },
        { typeof(PlayerCardOverallRatingDeclinedEvent), new Publisher(Exchange, "cards.players.declined") },
        { typeof(PlayerCardOverallRatingImprovedEvent), new Publisher(Exchange, "cards.players.improved") },
        { typeof(PlayerCardBoostedEvent), new Publisher(Exchange, "cards.players.boosted") },
        { typeof(PlayerCardPositionChangedEvent), new Publisher(Exchange, "cards.players.position_changed") },
        { typeof(CardDemandIncreasedEvent), new Publisher(Exchange, "cards.demand.increased") },
        { typeof(CardDemandDecreasedEvent), new Publisher(Exchange, "cards.demand.decreased") },
        { typeof(ListingBuyPriceDecreasedEvent), new Publisher(Exchange, "cards.listings.buy_price_decreased") },
        { typeof(ListingBuyPriceIncreasedEvent), new Publisher(Exchange, "cards.listings.buy_price_increased") },
        { typeof(ListingSellPriceDecreasedEvent), new Publisher(Exchange, "cards.listings.sell_price_decreased") },
        { typeof(ListingSellPriceIncreasedEvent), new Publisher(Exchange, "cards.listings.sell_price_increased") },
    };

    /// <summary>
    /// The <see cref="IDomainEvent"/> types that will be consumed in this domain mapped to their corresponding
    /// RabbitMQ exchanges
    /// </summary>
    private static readonly Dictionary<Type, Subscriber> DomainEventConsumerTypes = new()
    {
        { typeof(NewPlayerCardEvent), new Subscriber(Exchange, "cards.players.created") },
        { typeof(BattingStatsImprovementEvent), new Subscriber(Exchange, "performance.batting.improved") },
        { typeof(BattingStatsDeclineEvent), new Subscriber(Exchange, "performance.batting.declined") },
        { typeof(PitchingStatsImprovementEvent), new Subscriber(Exchange, "performance.pitching.improved") },
        { typeof(PitchingStatsDeclineEvent), new Subscriber(Exchange, "performance.pitching.declined") },
        { typeof(FieldingStatsImprovementEvent), new Subscriber(Exchange, "performance.fielding.improved") },
        { typeof(FieldingStatsDeclineEvent), new Subscriber(Exchange, "performance.fielding.declined") },
        { typeof(OverallRatingImprovementEvent), new Subscriber(Exchange, "cards.players.improved") },
        { typeof(OverallRatingDeclineEvent), new Subscriber(Exchange, "cards.players.declined") },
        { typeof(PositionChangeEvent), new Subscriber(Exchange, "cards.players.position_changed") },
        { typeof(PlayerCardBoostEvent), new Subscriber(Exchange, "cards.players.boosted") },
        { typeof(PlayerActivationEvent), new Subscriber(Exchange, "players.status.activated") },
        { typeof(PlayerFreeAgencyEvent), new Subscriber(Exchange, "players.status.free_agency") },
        { typeof(PlayerDeactivationEvent), new Subscriber(Exchange, "players.status.deactivated") },
        { typeof(PlayerTeamSigningEvent), new Subscriber(Exchange, "players.status.team_signed") },
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

            // Add messaging
            AddMessaging(context, services);

            // MLB The Show cards and marketplace dependencies
            services.TryAddSingleton<IClock, Clock>();
            services.AddFileSystems(context.Configuration);
            services.AddGameCardsMapping();
            services.AddGameCardsPlayerCardTracker(context.Configuration);
            services.AddGameCardsPriceTracker(context.Configuration);
            services.AddGameCardsRosterUpdates(context.Configuration);
            services.AddForecasting(context.Configuration);
            services.AddTrendReporting(context.Configuration);
            services.AddGameCardsEntityFrameworkCoreRepositories(context.Configuration);
            services.TryAddTransient<IRealTimeCommService, SignalRCommService>();

            // Register the domain event consumers
            AddDomainEventConsumers(context, services);

            // Register jobs and the job manager
            AddJobs(context, services);
        });

        return builder;
    }

    /// <summary>
    /// Add messaging
    /// </summary>
    private static void AddMessaging(HostBuilderContext context, IServiceCollection services)
    {
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
    }

    /// <summary>
    /// Register the domain event consumers
    /// </summary>
    private static void AddDomainEventConsumers(HostBuilderContext context, IServiceCollection services)
    {
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
    }

    /// <summary>
    /// Register jobs and the job manager
    /// </summary>
    private static void AddJobs(HostBuilderContext context, IServiceCollection services)
    {
        services.TryAddScoped<PlayerCardTrackerJob>();
        services.TryAddScoped<CardPriceTrackerJob>();
        services.TryAddScoped<CardListingImporterJob>();
        services.TryAddScoped<RosterUpdaterJob>();
        services.TryAddScoped<TrendReporterJob>();

        services.AddJobManager(context.Configuration, Assembly.GetExecutingAssembly());
        services.AddHostedService<ScheduledBackgroundService<IJobManager>>(sp =>
            new ScheduledBackgroundService<IJobManager>(sp.GetRequiredService<IServiceScopeFactory>(),
                Dependencies.JobManagerWork, Dependencies.JobManagerInterval(context.Configuration)));
    }
}