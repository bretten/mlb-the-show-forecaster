using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.PerformanceChanges;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Batting;

/// <summary>
/// Published when there is a decline in batting performance
/// </summary>
public sealed record BattingDeclineEvent(IPerformanceChange Change) : IPerformanceChangeEvent;