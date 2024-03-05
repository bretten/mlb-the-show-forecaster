using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Converters;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

/// <summary>
/// Represents a collection of stats by games
/// </summary>
/// <param name="Group">The type of stat: hitting, pitching, or fielding</param>
/// <param name="Splits">The stats by games</param>
[JsonConverter(typeof(StatJsonConverter))]
public readonly record struct StatsDto(
    [property: JsonPropertyName("group")]
    StatsGroupDto Group,
    [property: JsonPropertyName("splits")]
    IEnumerable<GameStatsDto> Splits
);