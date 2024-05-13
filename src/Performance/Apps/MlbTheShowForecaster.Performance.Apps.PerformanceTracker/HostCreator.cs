using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Common.Execution.Host.Services;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Participation;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker;

/// <summary>
/// Creates the host for tracking player performance
/// </summary>
public static class HostCreator
{
    /// <summary>
    /// The background work that will be done by <see cref="ScheduledBackgroundService{T}"/> for the <see cref="IPerformanceTracker"/>
    /// </summary>
    private static readonly Func<IPerformanceTracker, Task> PerformanceBackgroundWork = async tracker =>
    {
        await tracker.TrackPlayerPerformance(SeasonYear.Create(2024));
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
                    typeof(PlayerBattedInGameEvent).Assembly
                });

                // Player performance tracking dependencies
                services.AddSingleton<ICalendar, Calendar>();
                services.AddPerformanceMapping();
                services.AddPerformancePlayerSeasonScorekeeper(context.Configuration);
                services.AddPerformanceTracker(context.Configuration);
                services.AddPerformanceEntityFrameworkCoreRepositories(context.Configuration);

                // Background service for tracking player performance
                services.AddHostedService<ScheduledBackgroundService<IPerformanceTracker>>(sp =>
                    new ScheduledBackgroundService<IPerformanceTracker>(
                        sp.GetRequiredService<IServiceScopeFactory>(),
                        PerformanceBackgroundWork,
                        TimeSpan.FromSeconds(5)
                    ));
            });

        return builder.Build();
    }
}