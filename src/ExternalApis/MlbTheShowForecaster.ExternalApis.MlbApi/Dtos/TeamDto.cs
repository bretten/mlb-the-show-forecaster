using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;

/// <summary>
/// Represents a team
/// </summary>
/// <param name="Id">The MLB ID of the team</param>
/// <param name="Name">The team name</param>
public readonly record struct TeamDto(
    [property: JsonPropertyName("id")]
    int Id,
    [property: JsonPropertyName("name")]
    string Name
);