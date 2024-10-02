using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;

/// <summary>
/// Represents cumulative performance metrics by date for a player's season
/// </summary>
/// <param name="Year">The season</param>
/// <param name="MlbId">The player's MLB ID</param>
/// <param name="MetricsByDate">Collection of cumulative metrics by date</param>
public readonly record struct PlayerSeasonPerformanceMetrics(
    SeasonYear Year,
    MlbId MlbId,
    IReadOnlyList<PerformanceMetricsByDate> MetricsByDate
);