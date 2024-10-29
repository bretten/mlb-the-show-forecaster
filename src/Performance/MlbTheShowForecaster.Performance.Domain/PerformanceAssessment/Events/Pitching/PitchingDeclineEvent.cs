using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Pitching;

/// <summary>
/// Published when there is a decline in pitching performance
/// </summary>
public sealed record PitchingDeclineEvent(
    SeasonYear Year,
    MlbId MlbId,
    PerformanceScoreComparison Comparison,
    DateOnly Date)
    : IPerformanceChangeEvent;