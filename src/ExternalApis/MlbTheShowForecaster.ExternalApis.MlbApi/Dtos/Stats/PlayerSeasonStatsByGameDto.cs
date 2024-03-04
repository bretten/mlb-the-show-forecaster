using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

/// <summary>
/// Represents a player's season stats by individual game
/// </summary>
/// <param name="Id">The MLB ID of the player</param>
/// <param name="FirstName">The player's first name</param>
/// <param name="LastName">The player's last name</param>
/// <param name="Stats">The player's hitting, pitching, and fielding stats by game</param>
public readonly record struct PlayerSeasonStatsByGameDto(
    [property: JsonPropertyName("id")]
    int Id,
    [property: JsonPropertyName("useName")]
    string FirstName,
    [property: JsonPropertyName("useLastName")]
    string LastName,
    [property: JsonPropertyName("stats")]
    List<StatsDto> Stats
);