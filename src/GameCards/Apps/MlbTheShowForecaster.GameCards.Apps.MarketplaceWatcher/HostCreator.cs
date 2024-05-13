using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Execution.Host.Services;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
    private static readonly Func<IPlayerCardTracker, Task> PlayerCardBackgroundWork = async tracker =>
    {
        await tracker.TrackPlayerCards(SeasonYear.Create(2024));
    };

    /// <summary>
    /// The background work that will be done by <see cref="ScheduledBackgroundService{T}"/> for the <see cref="ICardPriceTracker"/>
    /// </summary>
    private static readonly Func<ICardPriceTracker, Task> CardPriceBackgroundWork = async tracker =>
    {
        await tracker.TrackCardPrices(SeasonYear.Create(2024));
    };

    /// <summary>
    /// The background work that will be done by <see cref="ScheduledBackgroundService{T}"/> for the <see cref="IRosterUpdateOrchestrator"/>
    /// </summary>
    private static readonly Func<IRosterUpdateOrchestrator, Task> RosterUpdateBackgroundWork = async rosterUpdater =>
    {
        await rosterUpdater.SyncRosterUpdates(SeasonYear.Create(2024));
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
                        TimeSpan.FromSeconds(5)
                    ));
                // Background service for tracking marketplace card prices
                services.AddHostedService<ScheduledBackgroundService<ICardPriceTracker>>(sp =>
                    new ScheduledBackgroundService<ICardPriceTracker>(
                        sp.GetRequiredService<IServiceScopeFactory>(),
                        CardPriceBackgroundWork,
                        TimeSpan.FromSeconds(5)
                    ));
                // Background service for tracking marketplace card prices
                services.AddHostedService<ScheduledBackgroundService<IRosterUpdateOrchestrator>>(sp =>
                    new ScheduledBackgroundService<IRosterUpdateOrchestrator>(
                        sp.GetRequiredService<IServiceScopeFactory>(),
                        RosterUpdateBackgroundWork,
                        TimeSpan.FromSeconds(5)
                    ));
            });

        return builder.Build();
    }
}