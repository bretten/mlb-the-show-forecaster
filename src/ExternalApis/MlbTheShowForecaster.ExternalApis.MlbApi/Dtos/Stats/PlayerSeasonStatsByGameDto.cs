using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

public record PlayerSeasonStatsByGameDto(
    [property: JsonPropertyName("id")]
    int Id,
    [property: JsonPropertyName("useName")]
    int FirstName,
    [property: JsonPropertyName("useLastName")]
    int LastName,
    [property: JsonPropertyName("stats")]
    StatsDto Stats
);