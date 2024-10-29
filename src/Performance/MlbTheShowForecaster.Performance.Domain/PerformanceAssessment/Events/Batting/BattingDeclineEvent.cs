using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Batting;

/// <summary>
/// Published when there is a decline in batting performance
/// </summary>
public sealed record BattingDeclineEvent(
    SeasonYear Year,
    MlbId MlbId,
    PerformanceScoreComparison Comparison,
    DateOnly Date)
    : IPerformanceChangeEvent;