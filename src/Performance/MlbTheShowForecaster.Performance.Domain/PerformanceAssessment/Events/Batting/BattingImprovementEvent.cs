using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Batting;

/// <summary>
/// Published when there is an improvement in batting performance
/// </summary>
public sealed record BattingImprovementEvent(SeasonYear Year, MlbId MlbId, PerformanceScoreComparison Comparison)
    : IPerformanceChangeEvent;