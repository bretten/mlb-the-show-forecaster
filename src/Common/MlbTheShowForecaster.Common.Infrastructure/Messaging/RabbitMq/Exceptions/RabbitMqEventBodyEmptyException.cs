namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq.Exceptions;

/// <summary>
/// Thrown when <see cref="RabbitMqDomainEventConsumer{T}"/> tries to consume an event with an empty body
/// </summary>
public sealed class RabbitMqEventBodyEmptyException : Exception
{
    public RabbitMqEventBodyEmptyException(string? message) : base(message)
    {
    }
}