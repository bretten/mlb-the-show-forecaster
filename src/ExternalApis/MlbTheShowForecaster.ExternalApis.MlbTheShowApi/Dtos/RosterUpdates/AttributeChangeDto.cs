using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.RosterUpdates;

/// <summary>
/// Represents a player's attribute change in a Roster Update
/// </summary>
/// <param name="Name">The name of the attribute</param>
/// <param name="CurrentValue">The new value of the attribute</param>
/// <param name="Direction">Whether the attribute increased or decreased</param>
/// <param name="Delta">The amount the attribute changed</param>
/// <param name="Color">For display purposes, the color associated with the change</param>
public sealed record AttributeChangeDto(
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("current_value")]
    string CurrentValue,
    [property: JsonPropertyName("direction")]
    string Direction,
    [property: JsonPropertyName("delta")]
    string Delta,
    [property: JsonPropertyName("color")]
    string Color
);