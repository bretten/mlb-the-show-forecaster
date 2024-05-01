using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;

/// <summary>
/// Registers RabbitMQ dependencies
/// </summary>
public static class Dependencies
{
    /// <summary>
    /// Registers RabbitMQ dependencies
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to</param>
    /// <param name="connectionFactory">Rabbit MQ connection factory</param>
    /// <param name="domainEventsToExchanges">Mapping of domain event types to their exchanges</param>
    /// <param name="assembliesToScan">Assemblies to scan for domain event consumers</param>
    public static void AddRabbitMq(this IServiceCollection services, IConnectionFactory connectionFactory,
        Dictionary<Type, string> domainEventsToExchanges, IList<Assembly> assembliesToScan)
    {
        services.AddSingleton(connectionFactory);
        // Register a single RabbitMQ connection. This connection will be used to create channels for the dispatcher and any consumers
        var connection = connectionFactory.CreateConnection();
        services.AddSingleton(connection);

        // Add the channel as a transient factory so it creates a new channel for every service that requests it
        services.AddTransient<IModel>(sp => connection.CreateModel());

        // Register the dispatcher as transient so it gets a new channel for each instance
        services.AddTransient<IDomainEventDispatcher>(sp =>
            new RabbitMqDomainEventDispatcher(sp.GetRequiredService<IModel>(), domainEventsToExchanges));

        // Register the domain event consumers
        assembliesToScan.SelectMany(a => a.GetTypes())
            .Where(t => t.GetInterfaces()
                            .Where(i => i.IsGenericType)
                            .Any(i => i.GetGenericTypeDefinition() == typeof(IDomainEventConsumer<>)) &&
                        !t.IsAbstract && !t.IsInterface) // Only concrete IDomainEventConsumer implementations
            .ToList()
            .ForEach(handlerType => // For each concrete implementation of IDomainEventConsumer
            {
                // The type as an interface, eg. IDomainEventConsumer<IDomainEvent>
                var handlerInterfaceType = handlerType.GetInterfaces()
                    .First(x => x.GetGenericTypeDefinition() == typeof(IDomainEventConsumer<>));

                // Register the domain event consumer as transient so it gets a new channel for each instance
                services.AddTransient(handlerInterfaceType, handlerType);
            });
    }
}