using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

public readonly record struct PlayerSeasonStatsByGameDto(
    [property: JsonPropertyName("id")]
    int Id,
    [property: JsonPropertyName("useName")]
    string FirstName,
    [property: JsonPropertyName("useLastName")]
    string LastName,
    [property: JsonPropertyName("stats")]
    List<StatsDto> Stats
);