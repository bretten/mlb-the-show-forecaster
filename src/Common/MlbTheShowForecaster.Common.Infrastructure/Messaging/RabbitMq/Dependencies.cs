using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
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
    /// <param name="publisherDomainEventsToExchanges">Mapping of the publisher's domain event types to their exchanges</param>
    /// <param name="consumerDomainEventsToExchanges">Mapping of the consumer domain event types to their exchanges</param>
    /// <param name="assembliesToScan">Assemblies to scan for domain event consumers</param>
    public static void AddRabbitMq(this IServiceCollection services,
        IConnectionFactory connectionFactory, Dictionary<Type, string> publisherDomainEventsToExchanges,
        Dictionary<Type, string> consumerDomainEventsToExchanges, IList<Assembly> assembliesToScan)
    {
        services.TryAddSingleton(connectionFactory);
        // Register a single RabbitMQ connection. This connection will be used to create channels for the dispatcher and any consumers
        var connection = connectionFactory.CreateConnection();
        services.TryAddSingleton(connection);

        // Setup the RabbitMQ exchanges
        SetupExchanges(connection, publisherDomainEventsToExchanges.Concat(consumerDomainEventsToExchanges)
            .GroupBy(x => x.Key)
            .ToDictionary(k => k.Key, v => v.First().Value));

        // Add the channel as a transient factory so it creates a new channel for every service that requests it
        services.TryAddTransient<IModel>(sp => connection.CreateModel());

        // Register the dispatcher as transient so it gets a new channel for each instance
        services.TryAddSingleton<IDomainEventDispatcher>(sp =>
            new RabbitMqDomainEventDispatcher(sp.GetRequiredService<IModel>(), publisherDomainEventsToExchanges));

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

                // Get the domain event type (implementation of IDomainEvent) for the IDomainEventConsumer
                var domainEventType = handlerInterfaceType.GetGenericArguments().First();
                // Get the exchange for the domain event type
                if (!consumerDomainEventsToExchanges.TryGetValue(domainEventType, out var queue) ||
                    string.IsNullOrWhiteSpace(queue))
                {
                    throw new RabbitMqExchangeNotDefinedException(
                        $"No RabbitMQ exchange mapping has been defined for {domainEventType.Name}");
                }

                // Register a RabbitMqDomainEventConsumer as a wrapper for the current domain event type's consumer
                var rabbitMqConsumerWrapperType =
                    typeof(RabbitMqDomainEventConsumer<>).MakeGenericType(domainEventType);
                var rabbitMqConsumerWrapperLoggerType =
                    typeof(ILogger<>).MakeGenericType(rabbitMqConsumerWrapperType);
                services.AddTransient(rabbitMqConsumerWrapperType, sp =>
                {
                    var logger = sp.GetRequiredService(rabbitMqConsumerWrapperLoggerType);
                    object[] parameters =
                        [sp.GetRequiredService(handlerInterfaceType), sp.GetRequiredService<IModel>(), queue, logger];
                    return Activator.CreateInstance(rabbitMqConsumerWrapperType, parameters)!;
                });
                // Register the underlying domain event consumer for the current domain event type
                services.TryAddTransient(handlerInterfaceType, handlerType);
            });
    }

    /// <summary>
    /// Sets up the RabbitMQ exchanges for the domain events
    /// </summary>
    /// <param name="connection">The RabbitMQ connection</param>
    /// <param name="domainEventsToExchanges">Publisher's mapping of domain event types to their exchanges</param>
    private static void SetupExchanges(IConnection connection, Dictionary<Type, string> domainEventsToExchanges)
    {
        using var channel = connection.CreateModel();
        foreach (var e in domainEventsToExchanges)
        {
            var exchange = e.Value;
            var queue = e.Value;
            var routingKey = e.Value;

            // Setup the exchange
            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Fanout, autoDelete: true);
            // Bind the queue to the exchange
            channel.QueueDeclare(queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: true,
                arguments: null);
            channel.QueueBind(queue: queue,
                exchange: exchange,
                routingKey: routingKey);
        }
    }
}