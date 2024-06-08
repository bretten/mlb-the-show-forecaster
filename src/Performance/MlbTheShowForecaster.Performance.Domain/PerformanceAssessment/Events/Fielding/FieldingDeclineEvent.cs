using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.PerformanceChanges;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Fielding;

/// <summary>
/// Published when there is a decline in fielding performance
/// </summary>
public sealed record FieldingDeclineEvent(IPerformanceChange Change) : IPerformanceChangeEvent;