using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.Common.Application.RealTime;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Execution.Host.Services;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Jobs;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Jobs;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.RealTime;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Participation;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;
using Calendar = com.brettnamba.MlbTheShowForecaster.Common.DateAndTime.Calendar;
using Dependencies = com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Jobs.Dependencies;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker;

/// <summary>
/// Creates the host for tracking player performance
/// </summary>
public static class PerformanceTrackerHostExtensions
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
        { typeof(PlayerBattedInGameEvent), new Publisher(Exchange, "performance.game.batted") },
        { typeof(PlayerFieldedInGameEvent), new Publisher(Exchange, "performance.game.fielded") },
        { typeof(PlayerPitchedInGameEvent), new Publisher(Exchange, "performance.game.pitched") },
        { typeof(BattingImprovementEvent), new Publisher(Exchange, "performance.batting.improved") },
        { typeof(BattingDeclineEvent), new Publisher(Exchange, "performance.batting.declined") },
        { typeof(PitchingImprovementEvent), new Publisher(Exchange, "performance.pitching.improved") },
        { typeof(PitchingDeclineEvent), new Publisher(Exchange, "performance.pitching.declined") },
        { typeof(FieldingImprovementEvent), new Publisher(Exchange, "performance.fielding.improved") },
        { typeof(FieldingDeclineEvent), new Publisher(Exchange, "performance.fielding.declined") }
    };

    /// <summary>
    /// The <see cref="IDomainEvent"/> types that will be consumed in this domain mapped to their corresponding
    /// RabbitMQ exchanges
    /// </summary>
    private static readonly Dictionary<Type, Subscriber> DomainEventConsumerTypes = new()
    {
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

            // Add messaging
            AddMessaging(context, services);

            // Player performance tracking dependencies
            services.AddSingleton<ICalendar, Calendar>();
            services.AddPerformanceAssessment(context.Configuration);
            services.AddPerformanceMapping();
            services.AddPerformancePlayerSeasonScorekeeper();
            services.AddPerformanceTracker(context.Configuration);
            services.AddPerformanceEntityFrameworkCoreRepositories(context.Configuration);
            services.TryAddTransient<IRealTimeCommService, SignalRCommService>();

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
            typeof(PlayerBattedInGameEvent).Assembly
        });
    }

    /// <summary>
    /// Register jobs and the job manager
    /// </summary>
    private static void AddJobs(HostBuilderContext context, IServiceCollection services)
    {
        services.TryAddScoped<PerformanceTrackerJob>();

        services.AddJobManager(context.Configuration, Assembly.GetExecutingAssembly());
        services.AddHostedService<ScheduledBackgroundService<IJobManager>>(sp =>
            new ScheduledBackgroundService<IJobManager>(sp.GetRequiredService<IServiceScopeFactory>(),
                Dependencies.JobManagerWork, Dependencies.JobManagerInterval(context.Configuration)));
    }
}