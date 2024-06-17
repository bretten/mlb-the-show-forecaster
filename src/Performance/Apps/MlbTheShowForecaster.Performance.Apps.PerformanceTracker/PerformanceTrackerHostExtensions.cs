using System.Globalization;
using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Execution.Host.Services;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Configuration;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Events.NewPlayerSeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Participation;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Calendar = com.brettnamba.MlbTheShowForecaster.Common.DateAndTime.Calendar;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker;

/// <summary>
/// Creates the host for tracking player performance
/// </summary>
public static class PerformanceTrackerHostExtensions
{
    /// <summary>
    /// The background work that will be done by <see cref="ScheduledBackgroundService{T}"/> for the <see cref="IPerformanceTracker"/>
    /// </summary>
    private static readonly Func<IPerformanceTracker, IServiceProvider, CancellationToken, Task>
        PerformanceBackgroundWork = async (tracker, sp, ct) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var logger = sp.GetRequiredService<ILogger<ScheduledBackgroundService<IPerformanceTracker>>>();
            // Wait for NewPlayerSeasonEvents to be consumed
            await Task.Delay(TimeSpan.FromSeconds(config.GetRequiredValue<int>("PerformanceTracker:StartDelay")));

            // Service name
            const string s = nameof(IPerformanceTracker);

            // The seasons to track
            var seasons = config.GetRequiredValue<ushort[]>("PerformanceTracker:Seasons");
            foreach (var season in seasons)
            {
                logger.LogInformation($"{s} - {season}");
                var result = await tracker.TrackPlayerPerformance(SeasonYear.Create(season), ct);
                logger.LogInformation($"{s} - Total player seasons = {result.TotalPlayerSeasons}");
                logger.LogInformation($"{s} - Total player season updates = {result.TotalPlayerSeasonUpdates}");
                logger.LogInformation(
                    $"{s} - Total up-to-date player seasons = {result.TotalUpToDatePlayerSeasons}");
            }
        };

    /// <summary>
    /// The <see cref="IDomainEvent"/> types that will be published in this domain mapped to their corresponding
    /// RabbitMQ exchanges
    /// </summary>
    private static readonly Dictionary<Type, string> DomainEventPublisherTypes = new()
    {
        { typeof(PlayerBattedInGameEvent), "PlayerBattedInGame" },
        { typeof(PlayerFieldedInGameEvent), "PlayerFieldedInGame" },
        { typeof(PlayerPitchedInGameEvent), "PlayerPitchedInGame" },
        { typeof(BattingImprovementEvent), "BattingImprovement" },
        { typeof(BattingDeclineEvent), "BattingDecline" },
        { typeof(PitchingImprovementEvent), "PitchingImprovement" },
        { typeof(PitchingDeclineEvent), "PitchingDecline" },
        { typeof(FieldingImprovementEvent), "FieldingImprovement" },
        { typeof(FieldingDeclineEvent), "FieldingDecline" }
    };

    /// <summary>
    /// The <see cref="IDomainEvent"/> types that will be consumed in this domain mapped to their corresponding
    /// RabbitMQ exchanges
    /// </summary>
    private static readonly Dictionary<Type, string> DomainEventConsumerTypes = new()
    {
        { typeof(NewPlayerSeasonEvent), "PlayerActivated" },
    };

    /// <summary>
    /// Builds the <see cref="IHost"/> for this domain
    /// </summary>
    /// <param name="builder"><see cref="IHostBuilder"/></param>
    /// <param name="args">Command-line arguments</param>
    /// <returns>The <see cref="IHostBuilder"/> with the performance tracker configured on it</returns>
    public static IHostBuilder ConfigurePerformanceTracker(this IHostBuilder builder, string[] args)
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
                typeof(PlayerBattedInGameEvent).Assembly,
                typeof(NewPlayerSeasonEvent).Assembly
            });

            // Player performance tracking dependencies
            services.AddSingleton<ICalendar, Calendar>();
            services.AddPerformanceAssessment(context.Configuration);
            services.AddPerformanceMapping();
            services.AddPerformancePlayerSeasonScorekeeper();
            services.AddPerformanceTracker(context.Configuration);
            services.AddPerformanceEntityFrameworkCoreRepositories(context.Configuration);

            // Consumer for NewPlayerSeason events
            services.AddHostedService<KeepAliveBackgroundService<RabbitMqDomainEventConsumer<NewPlayerSeasonEvent>>>();

            // Background service for tracking player performance
            services.AddHostedService<ScheduledBackgroundService<IPerformanceTracker>>(sp =>
                new ScheduledBackgroundService<IPerformanceTracker>(
                    sp.GetRequiredService<IServiceScopeFactory>(),
                    PerformanceBackgroundWork,
                    TimeSpan.ParseExact(
                        context.Configuration.GetRequiredValue<string>("PerformanceTracker:Interval"), "g",
                        CultureInfo.InvariantCulture)
                ));
        });

        return builder;
    }
}