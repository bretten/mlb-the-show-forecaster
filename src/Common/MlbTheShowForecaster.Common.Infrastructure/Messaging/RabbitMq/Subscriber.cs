namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;

/// <summary>
/// Defines properties for a RabbitMQ subscriber
/// </summary>
/// <param name="Exchange">The RabbitMQ exchange that the subscriber lives on</param>
/// <param name="Queue">The RabbitMQ queue that this subscriber is subscribed to</param>
/// <param name="RoutingKey">The RabbitMQ routing key that this subscriber is watching</param>
public sealed record Subscriber(string Exchange, string Queue, string RoutingKey)
{
    public Subscriber(string exchange, string queueAndRoutingKey) : this(exchange, queueAndRoutingKey,
        queueAndRoutingKey)
    {
    }
};