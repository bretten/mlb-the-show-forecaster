using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Responses;

/// <summary>
/// Response for <see cref="GetPlayersBySeasonRequest"/>
/// </summary>
/// <param name="Players">The players for the requested season</param>
public sealed record GetPlayersBySeasonResponse(
    [property: JsonPropertyName("people")]
    List<PlayerDto>? Players
);