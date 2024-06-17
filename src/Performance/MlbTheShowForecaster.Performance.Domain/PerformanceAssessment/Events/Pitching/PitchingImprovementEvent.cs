using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.PerformanceChanges;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Pitching;

/// <summary>
/// Published when there is an improvement in pitching performance
/// </summary>
public sealed record PitchingImprovementEvent(IPerformanceChange Change) : IPerformanceChangeEvent;