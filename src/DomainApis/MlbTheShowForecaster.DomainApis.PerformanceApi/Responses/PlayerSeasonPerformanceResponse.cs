using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.DomainApis.PerformanceApi.Responses;

/// <summary>
/// Represents a player's season performance
/// </summary>
/// <param name="Season">The player's season</param>
/// <param name="MlbId">The player's MLB ID</param>
/// <param name="MetricsByDate">Performance metrics by date</param>
public readonly record struct PlayerSeasonPerformanceResponse(
    [property: JsonPropertyName("season")] ushort Season,
    [property: JsonPropertyName("mlbId")] int MlbId,
    [property: JsonPropertyName("metricsByDate")]
    IReadOnlyList<PerformanceMetricsByDate> MetricsByDate
);