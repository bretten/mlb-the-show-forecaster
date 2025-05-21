using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.RosterEntries;

/// <summary>
/// Represents the player on a team's roster
/// </summary>
public readonly record struct RosterEntryDto(
    [property: JsonPropertyName("status")] RosterEntryStatusDto Status,
    [property: JsonPropertyName("team")] TeamDto Team,
    [property: JsonPropertyName("isActive")]
    bool IsActive,
    [property: JsonPropertyName("startDate")]
    DateOnly StartDate,
    [property: JsonPropertyName("endDate")]
    DateOnly EndDate,
    [property: JsonPropertyName("statusDate")]
    DateOnly StatusDate,
    [property: JsonPropertyName("isActiveFortyMan")]
    bool IsActiveFortyMan
);