using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Pitching;

/// <summary>
/// Published when there is an improvement in pitching performance
/// </summary>
public sealed record PitchingImprovementEvent(
    SeasonYear Year,
    MlbId MlbId,
    PerformanceScoreComparison Comparison,
    DateOnly Date)
    : IPerformanceChangeEvent;