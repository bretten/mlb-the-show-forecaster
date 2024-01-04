using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Responses;

/// <summary>
/// Response for <see cref="GetPlayersBySeasonRequest"/>
/// </summary>
public sealed class GetPlayersBySeasonResponse
{
    /// <summary>
    /// The players for the requested season
    /// </summary>
    [JsonPropertyName("people")]
    public List<Player>? Players { get; init; }
}