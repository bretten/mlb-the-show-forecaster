using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

/// <summary>
/// Represents a stat group
/// </summary>
/// <param name="DisplayName">The name of the stat group: hitting, pitching, or fielding</param>
public readonly record struct StatsGroupDto(
    [property: JsonPropertyName("displayName")]
    string DisplayName
);