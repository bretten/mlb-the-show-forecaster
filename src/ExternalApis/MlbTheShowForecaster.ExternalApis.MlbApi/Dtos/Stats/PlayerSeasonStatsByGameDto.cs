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
)
{
    /// <summary>
    /// Returns any hitting stats by game for this player's season
    /// </summary>
    /// <returns>Hitting stats by game for this player's season</returns>
    public IEnumerable<GameHittingStatsDto> GetHittingStats() =>
        (IEnumerable<GameHittingStatsDto>)Stats
            .FirstOrDefault(x => x.Group.DisplayName == Constants.Parameters.Hitting).Splits ??
        new List<GameHittingStatsDto>();

    /// <summary>
    /// Returns any pitching stats by game for this player's season
    /// </summary>
    /// <returns>Pitching stats by game for this player's season</returns>
    public IEnumerable<GamePitchingStatsDto> GetPitchingStats() =>
        (IEnumerable<GamePitchingStatsDto>)Stats
            .FirstOrDefault(x => x.Group.DisplayName == Constants.Parameters.Pitching).Splits ??
        new List<GamePitchingStatsDto>();

    /// <summary>
    /// Returns any fielding stats by game for this player's season
    /// </summary>
    /// <returns>Fielding stats by game for this player's season</returns>
    public IEnumerable<GameFieldingStatsDto> GetFieldingStats() =>
        (IEnumerable<GameFieldingStatsDto>)Stats
            .FirstOrDefault(x => x.Group.DisplayName == Constants.Parameters.Fielding).Splits ??
        new List<GameFieldingStatsDto>();
};