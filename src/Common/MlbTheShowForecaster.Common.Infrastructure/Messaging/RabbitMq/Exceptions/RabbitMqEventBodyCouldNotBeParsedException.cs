namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Messaging.RabbitMq.Exceptions;

/// <summary>
/// Thrown when <see cref="RabbitMqDomainEventConsumer{T}"/> cannot parse a RabbitMQ event in the way it expects to
/// </summary>
public sealed class RabbitMqEventBodyCouldNotBeParsedException : AggregateException
{
    public RabbitMqEventBodyCouldNotBeParsedException(string? message, Exception innerException) : base(message,
        innerException)
    {
    }
}