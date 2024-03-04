using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Responses;

public sealed record GetPlayerSeasonStatsByGameResponse(
    [property: JsonPropertyName("people")]
    List<PlayerSeasonStatsByGameDto>? People
);