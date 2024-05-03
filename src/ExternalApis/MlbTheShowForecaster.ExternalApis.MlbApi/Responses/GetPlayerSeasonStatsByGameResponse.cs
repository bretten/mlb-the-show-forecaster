using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Responses;

/// <summary>
/// Response for <see cref="GetPlayerSeasonStatsByGameRequest"/>
/// </summary>
/// <param name="People">The players whose stats were retrieved</param>
public sealed record GetPlayerSeasonStatsByGameResponse(
    [property: JsonPropertyName("people")]
    IEnumerable<PlayerSeasonStatsByGameDto>? People
);