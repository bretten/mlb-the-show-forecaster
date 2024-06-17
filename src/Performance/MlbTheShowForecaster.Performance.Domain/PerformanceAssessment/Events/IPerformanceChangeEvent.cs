using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.PerformanceChanges;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events;

/// <summary>
/// Published when something has a change in its performance
/// </summary>
public interface IPerformanceChangeEvent : IDomainEvent
{
    /// <summary>
    /// A comparison of the previous and new performance
    /// </summary>
    public IPerformanceChange Change { get; }
}