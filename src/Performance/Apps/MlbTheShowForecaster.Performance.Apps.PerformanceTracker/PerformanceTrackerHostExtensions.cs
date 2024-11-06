using System.Globalization;
using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.Common.Application.RealTime;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Execution.Host.Services;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Configuration;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Jobs;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Jobs.Io;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.RealTime;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Participation;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;
using Calendar = com.brettnamba.MlbTheShowForecaster.Common.DateAndTime.Calendar;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker;

/// <summary>
/// Creates the host for tracking player performance
/// </summary>
public static class PerformanceTrackerHostExtensions
{
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
        services.TryAddSingleton<IJobManager>(sp =>
        {
            var interval =
                ParseInterval(context.Configuration.GetRequiredValue<string>("PerformanceTracker:Interval"));
            var seasons = context.Configuration.GetRequiredValue<ushort[]>("PerformanceTracker:Seasons");
            var jobSchedules = new List<JobSchedule>();
            foreach (var season in seasons)
            {
                var input = new SeasonJobInput(SeasonYear.Create(season));
                jobSchedules.Add(new JobSchedule(JobType: typeof(PerformanceTrackerJob), JobInput: input,
                    Interval: interval));
            }

            var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            var commService = sp.GetRequiredService<IRealTimeCommService>();
            var logger = sp.GetRequiredService<ILogger<ScopedSingleInstanceJobManager>>();
            return new ScopedSingleInstanceJobManager(scopeFactory, jobSchedules, commService, logger);
        });

        var jobManagerInterval = ParseInterval(context.Configuration.GetRequiredValue<string>("Jobs:Interval"));

        services.AddHostedService<ScheduledBackgroundService<IJobManager>>(sp =>
            new ScheduledBackgroundService<IJobManager>(sp.GetRequiredService<IServiceScopeFactory>(), JobManagerWork,
                jobManagerInterval));
    }

    /// <summary>
    /// The job manager will run scheduled jobs on an interval
    /// </summary>
    private static readonly Func<IJobManager, IServiceProvider, CancellationToken, Task>
        JobManagerWork = async (jobManager, sp, ct) => { await jobManager.RunScheduled(ct); };

    private static TimeSpan ParseInterval(string interval)
    {
        return TimeSpan.ParseExact(interval, "g", CultureInfo.InvariantCulture);
    }
}