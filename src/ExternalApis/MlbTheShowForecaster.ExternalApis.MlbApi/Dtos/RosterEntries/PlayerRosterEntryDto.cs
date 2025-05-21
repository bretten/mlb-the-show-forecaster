using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.RosterEntries;

/// <summary>
/// Represents a player's roster entries
/// </summary>
/// <param name="Id">The MLB ID of the player</param>
/// <param name="FirstName">The player's first name</param>
/// <param name="LastName">The player's last name</param>
/// <param name="RosterEntries">The player's roster entries</param>
public readonly record struct PlayerRosterEntryDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("useName")]
    string FirstName,
    [property: JsonPropertyName("useLastName")]
    string LastName,
    [property: JsonPropertyName("rosterEntries")]
    List<RosterEntryDto> RosterEntries
);