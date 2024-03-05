using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;

/// <summary>
/// The player's position
/// </summary>
/// <param name="Name">The name of their position</param>
/// <param name="Abbreviation">An abbreviation of their position</param>
public readonly record struct PositionDto(
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("abbreviation")]
    string Abbreviation
);