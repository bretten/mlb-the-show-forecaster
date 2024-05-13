namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq.Exceptions;

/// <summary>
/// Thrown when there is no exchange or queue setup for a consumer's domain event type
/// </summary>
public sealed class RabbitMqExchangeNotDefinedException : Exception
{
    public RabbitMqExchangeNotDefinedException(string? message) : base(message)
    {
    }
}