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
    /// <returns>The completed task</returns>
    Task Handle(T e);
}