using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.PerformanceChanges;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Pitching;

/// <summary>
/// Published when there is a decline in pitching performance
/// </summary>
public sealed record PitchingDeclineEvent(IPerformanceChange Change) : IPerformanceChangeEvent;