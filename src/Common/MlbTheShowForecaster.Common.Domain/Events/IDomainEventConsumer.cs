namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;

/// <summary>
/// Consumes domain events
/// </summary>
public interface IDomainEventConsumer<in T>
{
    /// <summary>
    /// Consumes a domain event
    /// </summary>
    /// <param name="e">The domain event</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    Task Handle(T e, CancellationToken cancellationToken = default);
}