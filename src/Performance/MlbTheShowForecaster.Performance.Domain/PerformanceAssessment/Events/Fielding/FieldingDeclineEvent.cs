using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Events.Fielding;

/// <summary>
/// Published when there is a decline in fielding performance
/// </summary>
public sealed record FieldingDeclineEvent(SeasonYear Year, MlbId MlbId, PerformanceScoreComparison Comparison)
    : IPerformanceChangeEvent;