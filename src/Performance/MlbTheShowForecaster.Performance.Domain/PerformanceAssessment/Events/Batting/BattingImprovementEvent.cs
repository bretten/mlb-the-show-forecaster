using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.PerformanceChanges;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Batting;

/// <summary>
/// Published when there is an improvement in batting performance
/// </summary>
public sealed record BattingImprovementEvent(IPerformanceChange Change) : IPerformanceChangeEvent;