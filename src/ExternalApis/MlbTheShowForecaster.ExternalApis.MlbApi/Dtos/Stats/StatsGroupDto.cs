using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

public sealed record StatsGroupDto(
    [property: JsonPropertyName("displayName")]
    string DisplayName
);