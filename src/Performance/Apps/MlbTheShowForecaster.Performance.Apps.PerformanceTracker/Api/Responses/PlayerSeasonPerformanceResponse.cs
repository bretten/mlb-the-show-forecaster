using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Api.Responses;

/// <summary>
/// Represents a player's season performance
/// </summary>
/// <param name="Season">The player's season</param>
/// <param name="MlbId">The player's MLB ID</param>
/// <param name="MetricsByDate">Performance metrics by date</param>
public readonly record struct PlayerSeasonPerformanceResponse(
    ushort Season,
    int MlbId,
    IReadOnlyList<PerformanceMetricsByDate> MetricsByDate);