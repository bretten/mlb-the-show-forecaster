using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;

namespace com.brettnamba.MlbTheShowForecaster.Common.Infrastructure.Tests.Messaging.RabbitMq.TestClasses;

public sealed record EventType1 : IDomainEvent;

public sealed record EventType2 : IDomainEvent;

public sealed record EventType3 : IDomainEvent;

public sealed record EventType4 : IDomainEvent;

public sealed class DomainEventConsumer1 : IDomainEventConsumer<EventType1>
{
    public Action? Callback { get; set; }

    public Task Handle(EventType1 e, CancellationToken cancellationToken)
    {
        Callback?.Invoke();
        return Task.CompletedTask;
    }
}

public sealed class DomainEventConsumer2 : IDomainEventConsumer<EventType2>
{
    public Action? Callback { get; set; }

    public Task Handle(EventType2 e, CancellationToken cancellationToken)
    {
        Callback?.Invoke();
        return Task.CompletedTask;
    }
}

public sealed class DomainEventConsumer3 : IDomainEventConsumer<EventType3>
{
    public Action? Callback { get; set; }

    public Task Handle(EventType3 e, CancellationToken cancellationToken)
    {
        Callback?.Invoke();
        return Task.CompletedTask;
    }
}

/**
 * DeadLetterEvent is used to test dead letter exchanges and queues
 */
public sealed record DeadLetterEvent1() : IDomainEvent;

public sealed record DeadLetterEvent2() : IDomainEvent;

public sealed class DeadLetterConsumer1 : IDomainEventConsumer<DeadLetterEvent1>
{
    public Task Handle(DeadLetterEvent1 e, CancellationToken cancellationToken = default)
    {
        throw new Exception();
    }
}

public sealed class DeadLetterConsumer2 : IDomainEventConsumer<DeadLetterEvent2>
{
    public Task Handle(DeadLetterEvent2 e, CancellationToken cancellationToken = default)
    {
        throw new Exception();
    }
}