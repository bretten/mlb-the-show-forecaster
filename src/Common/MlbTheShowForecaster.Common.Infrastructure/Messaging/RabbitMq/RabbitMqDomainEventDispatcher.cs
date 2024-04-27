using System.Text;
using System.Text.Json;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using RabbitMQ.Client;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;

/// <summary>
/// Rabbit MQ implementation of <see cref="IDomainEventDispatcher"/>
/// <para>Dispatches domain events using Rabbit MQ</para>
/// </summary>
public sealed class RabbitMqDomainEventDispatcher : IDomainEventDispatcher
{
    /// <summary>
    /// The RabbitMQ channel
    /// </summary>
    private readonly IModel _model;

    /// <summary>
    /// A mapping of <see cref="IDomainEvent"/> types to their corresponding RabbitMQ exchanges
    /// </summary>
    private readonly Dictionary<Type, string> _domainEventTypes;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="model">The RabbitMQ channel</param>
    /// <param name="domainEventTypes">A mapping of <see cref="IDomainEvent"/> types to their corresponding RabbitMQ exchanges</param>
    public RabbitMqDomainEventDispatcher(IModel model, Dictionary<Type, string> domainEventTypes)
    {
        _model = model;
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
            var exchange = _domainEventTypes[e.GetType()];
            var routingKey = _domainEventTypes[e.GetType()];

            _model.BasicPublish(exchange: exchange,
                routingKey: routingKey,
                body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(e))
            );
        }
    }
}