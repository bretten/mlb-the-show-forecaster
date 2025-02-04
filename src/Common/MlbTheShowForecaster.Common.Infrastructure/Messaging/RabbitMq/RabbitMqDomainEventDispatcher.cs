using System.Text;
using System.Text.Json;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using RabbitMQ.Client;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;

/// <summary>
/// Rabbit MQ implementation of <see cref="IDomainEventDispatcher"/>
/// <para>Dispatches domain events using Rabbit MQ</para>
/// </summary>
public sealed class RabbitMqDomainEventDispatcher : IDomainEventDispatcher, IDisposable
{
    /// <summary>
    /// The RabbitMQ channel
    /// </summary>
    private readonly IModel _channel;

    /// <summary>
    /// A mapping of <see cref="IDomainEvent"/> types to their corresponding RabbitMQ exchanges
    /// </summary>
    private readonly Dictionary<Type, Publisher> _domainEventTypes;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="channel">The RabbitMQ channel</param>
    /// <param name="domainEventTypes">A mapping of <see cref="IDomainEvent"/> types to their corresponding RabbitMQ exchanges</param>
    public RabbitMqDomainEventDispatcher(IModel channel, Dictionary<Type, Publisher> domainEventTypes)
    {
        _channel = channel;
        _domainEventTypes = domainEventTypes;
    }

    /// <summary>
    /// Dispatches the domain events
    /// </summary>
    /// <param name="events">The domain events to dispatch</param>
    public void Dispatch(IEnumerable<IDomainEvent> events)
    {
        foreach (var e in events)
        {
            var publisher = _domainEventTypes[e.GetType()];

            _channel.BasicPublish(exchange: publisher.Exchange,
                routingKey: publisher.RoutingKey,
                body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(e, e.GetType()))
            );
        }
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        // Calling Dispose/Abort/Close twice on RabbitMQ channel leads to null ref exception. Disposal is already handled by connection
        //_channel.Dispose();
    }
}