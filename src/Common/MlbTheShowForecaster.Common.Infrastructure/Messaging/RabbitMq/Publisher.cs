namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq;

/// <summary>
/// Defines properties for a RabbitMQ publisher
/// </summary>
/// <param name="Exchange">The RabbitMQ exchange that the publisher broadcasts to</param>
/// <param name="RoutingKey">The RabbitMQ routing key that the publisher routes to</param>
public sealed record Publisher(string Exchange, string RoutingKey);