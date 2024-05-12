using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Games;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

/// <summary>
/// Represents pitching stats for a single game
/// </summary>
/// <param name="Season">The season of the game</param>
/// <param name="Date">The date of the game</param>
/// <param name="GameType">The type of game</param>
/// <param name="IsHome">True if the game was at home, otherwise false</param>
/// <param name="IsWin">True if the game was a win, otherwise false</param>
/// <param name="Game">Information about the game</param>
/// <param name="Stat">The game's pitching stats</param>
public sealed record GamePitchingStatsDto(
    string Season,
    DateOnly Date,
    string GameType,
    bool IsHome,
    bool IsWin,
    TeamDto Team,
    GameDto Game,
    [property: JsonPropertyName("stat")]
    PitchingStatsDto Stat
) : GameStatsDto(Season, Date, GameType, IsHome, IsWin, Team, Game);