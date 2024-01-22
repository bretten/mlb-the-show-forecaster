namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;

/// <summary>
/// Dispatches domain events
/// </summary>
public interface IDomainEventDispatcher
{
    /// <summary>
    /// Dispatches a collection of domain events to listeners
    /// </summary>
    /// <param name="events">The domain events</param>
    void Dispatch(IEnumerable<IDomainEvent> events);
}