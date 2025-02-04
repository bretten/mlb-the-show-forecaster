using System.Reflection;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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
    /// <param name="domainEventPublisherTypes">Mapping of the publisher's domain event types to their exchanges</param>
    /// <param name="domainEventConsumerTypes">Mapping of the consumer domain event types to their queues</param>
    /// <param name="assembliesToScan">Assemblies to scan for domain event consumers</param>
    public static void AddRabbitMq(this IServiceCollection services, IConnectionFactory connectionFactory,
        Dictionary<Type, Publisher> domainEventPublisherTypes, Dictionary<Type, Subscriber> domainEventConsumerTypes,
        IList<Assembly> assembliesToScan)
    {
        services.TryAddSingleton(connectionFactory);
        // Register a single RabbitMQ connection. This connection will be used to create channels for the dispatcher and any consumers
        var connection = connectionFactory.CreateConnection();
        services.TryAddSingleton(connection);

        // Set up the RabbitMQ exchanges and queues
        SetupExchangesAndQueues(connection, domainEventPublisherTypes, domainEventConsumerTypes);

        // Add the channel as a transient factory so it creates a new channel for every service that requests it
        services.TryAddTransient<IModel>(sp => connection.CreateModel());

        // Register the dispatcher as transient so it gets a new channel for each instance
        services.TryAddSingleton<IDomainEventDispatcher>(sp =>
            new RabbitMqDomainEventDispatcher(sp.GetRequiredService<IModel>(), domainEventPublisherTypes));

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
                if (!domainEventConsumerTypes.TryGetValue(domainEventType, out var subscriber))
                {
                    // The domain is not subscribed to this event
                    return;
                }

                // Register a RabbitMqDomainEventConsumer as a wrapper for the current domain event type's consumer
                var rabbitMqConsumerWrapperType =
                    typeof(RabbitMqDomainEventConsumer<>).MakeGenericType(domainEventType);
                var rabbitMqConsumerWrapperLoggerType = typeof(ILogger<>).MakeGenericType(rabbitMqConsumerWrapperType);
                services.AddTransient(rabbitMqConsumerWrapperType, sp =>
                {
                    var logger = sp.GetRequiredService(rabbitMqConsumerWrapperLoggerType);
                    var channel = sp.GetRequiredService<IModel>();
                    var consumer = new AsyncEventingBasicConsumer(channel);
                    channel.BasicConsume(queue: subscriber.Queue,
                        autoAck: false,
                        consumer: consumer);

                    object[] parameters = [sp.GetRequiredService(handlerInterfaceType), channel, consumer, logger];
                    return Activator.CreateInstance(rabbitMqConsumerWrapperType, parameters)!;
                });
                // Register the underlying domain event consumer for the current domain event type
                services.TryAddTransient(handlerInterfaceType, handlerType);
            });
    }

    /// <summary>
    /// Sets up the RabbitMQ exchanges and queues for the domain events
    /// </summary>
    /// <param name="connection">The RabbitMQ connection</param>
    /// <param name="domainEventPublisherTypes">Mapping of the publisher's domain event types to their exchanges</param>
    /// <param name="domainEventConsumerTypes">Mapping of the consumer domain event types to their queues</param>
    private static void SetupExchangesAndQueues(IConnection connection,
        Dictionary<Type, Publisher> domainEventPublisherTypes, Dictionary<Type, Subscriber> domainEventConsumerTypes)
    {
        using var channel = connection.CreateModel();

        var types = domainEventPublisherTypes.Select(x => new
                { Exchange = x.Value.Exchange, Queue = x.Value.RoutingKey, RoutingKey = x.Value.RoutingKey })
            .Concat(domainEventConsumerTypes.Select(x => new
                { Exchange = x.Value.Exchange, Queue = x.Value.Queue, RoutingKey = x.Value.RoutingKey }));

        foreach (var e in types)
        {
            var exchange = e.Exchange;
            var queue = e.Queue;
            var routingKey = e.RoutingKey;

            var deadLetterExchange = GetDeadLetterName(exchange);
            var deadLetterQueue = GetDeadLetterName(queue);

            // Set up the exchange (idempotent)
            channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Topic, autoDelete: false, durable: true);
            // Set up the dead letter exchange (idempotent)
            channel.ExchangeDeclare(exchange: GetDeadLetterName(exchange), type: ExchangeType.Direct, autoDelete: false,
                durable: true);

            // Declare and bind the queue to the exchange (idempotent)
            channel.QueueDeclare(queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: new Dictionary<string, object>()
                {
                    { "x-dead-letter-exchange", deadLetterExchange },
                });
            channel.QueueBind(queue: queue,
                exchange: exchange,
                routingKey: routingKey);

            // Declare and bind the dead letter queue to the dead letter exchange (idempotent)
            channel.QueueDeclare(queue: deadLetterQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(queue: deadLetterQueue,
                exchange: deadLetterExchange,
                routingKey: routingKey);
        }
    }

    /// <summary>
    /// Gets the dead letter equivalent name for an exchange or queue
    /// </summary>
    /// <param name="exchangeOrQueue">The exchange or queue</param>
    /// <returns>The dead letter equivalent name</returns>
    private static string GetDeadLetterName(string exchangeOrQueue)
    {
        return $"{exchangeOrQueue}-poison";
    }
}