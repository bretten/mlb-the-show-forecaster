using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.Common.Application.RealTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Execution.Host.Services;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Jobs;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.Jobs;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.RealTime;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Events;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;
using Dependencies = com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Jobs.Dependencies;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker;

/// <summary>
/// Creates the host for tracking player statuses
/// </summary>
public static class PlayerTrackerHostExtensions
{
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

            // Add messaging
            AddMessaging(context, services);

            // Player status tracking dependencies
            services.AddPlayerTeamProvider();
            services.AddPlayerStatusMapping();
            services.AddPlayerStatusTracker(context.Configuration);
            services.AddPlayerStatusEntityFrameworkCoreRepositories(context.Configuration);
            services.AddPlayerSearchService(context.Configuration);
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
            typeof(PlayerActivatedEvent).Assembly
        });
    }

    /// <summary>
    /// Register jobs and the job manager
    /// </summary>
    private static void AddJobs(HostBuilderContext context, IServiceCollection services)
    {
        services.TryAddScoped<PlayerStatusTrackerJob>();

        services.AddJobManager(context.Configuration);
        services.AddHostedService<ScheduledBackgroundService<IJobManager>>(sp =>
            new ScheduledBackgroundService<IJobManager>(sp.GetRequiredService<IServiceScopeFactory>(),
                Dependencies.JobManagerWork, Dependencies.JobManagerInterval(context.Configuration)));
    }
}