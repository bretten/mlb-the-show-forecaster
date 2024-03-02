using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;

/// <summary>
/// A player's current team
/// </summary>
/// <param name="Id">The team's MLB ID</param>
public readonly record struct CurrentTeamDto(
    [property: JsonPropertyName("id")]
    int Id
);