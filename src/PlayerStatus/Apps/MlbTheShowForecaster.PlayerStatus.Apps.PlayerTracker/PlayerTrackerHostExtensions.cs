using System.Globalization;
using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Execution.Host.Services;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Configuration;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Events;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker;

/// <summary>
/// Creates the host for tracking player statuses
/// </summary>
public static class PlayerTrackerHostExtensions
{
    /// <summary>
    /// The background work that will be done by <see cref="ScheduledBackgroundService{T}"/> for the <see cref="IPlayerStatusTracker"/>
    /// </summary>
    private static readonly Func<IPlayerStatusTracker, IServiceProvider, CancellationToken, Task>
        PlayerTrackerBackgroundWork = async (tracker, sp, ct) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetRequiredService<ILogger<ScheduledBackgroundService<IPlayerStatusTracker>>>();

            // Service name
            const string s = nameof(IPlayerStatusTracker);

            // The seasons to track
            var seasons = config.GetRequiredValue<ushort[]>("PlayerStatusTracker:Seasons");
            foreach (var season in seasons)
            {
                logger.LogInformation($"{s} - {season}");
                var result = await tracker.TrackPlayers(SeasonYear.Create(season), ct);
                logger.LogInformation($"{s} - Total roster entries = {result.TotalRosterEntries}");
                logger.LogInformation($"{s} - Total new players = {result.TotalNewPlayers}");
                logger.LogInformation($"{s} - Total updated players = {result.TotalUpdatedPlayers}");
                logger.LogInformation($"{s} - Total unchanged players = {result.TotalUnchangedPlayers}");
            }
        };

    /// <summary>
    /// The <see cref="IDomainEvent"/> types that will be published in this domain mapped to their corresponding
    /// RabbitMQ exchanges
    /// </summary>
    private static readonly Dictionary<Type, string> DomainEventPublisherTypes = new()
    {
        { typeof(PlayerActivatedEvent), "PlayerActivated" },
        { typeof(PlayerEnteredFreeAgencyEvent), "PlayerEnteredFreeAgency" },
        { typeof(PlayerInactivatedEvent), "PlayerInactivated" },
        { typeof(PlayerSignedContractWithTeamEvent), "PlayerSignedContractWithTeam" }
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
    /// <param name="builder"><see cref="IHostBuilder"/></param>
    /// <param name="args">Command-line arguments</param>
    /// <returns>The <see cref="IHostBuilder"/> with the player tracker configured on it</returns>
    public static IHostBuilder ConfigurePlayerTracker(this IHostBuilder builder, string[] args)
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
                typeof(PlayerActivatedEvent).Assembly
            });

            // Player status tracking dependencies
            services.AddPlayerTeamProvider();
            services.AddPlayerStatusMapping();
            services.AddPlayerStatusTracker(context.Configuration);
            services.AddPlayerStatusEntityFrameworkCoreRepositories(context.Configuration);
            services.AddPlayerSearchService(context.Configuration);

            // Background service for tracking player statuses
            services.AddHostedService<ScheduledBackgroundService<IPlayerStatusTracker>>(sp =>
                new ScheduledBackgroundService<IPlayerStatusTracker>(
                    sp.GetRequiredService<IServiceScopeFactory>(),
                    PlayerTrackerBackgroundWork,
                    TimeSpan.ParseExact(
                        context.Configuration.GetRequiredValue<string>("PlayerStatusTracker:Interval"), "g",
                        CultureInfo.InvariantCulture)
                ));
        });

        return builder;
    }
}