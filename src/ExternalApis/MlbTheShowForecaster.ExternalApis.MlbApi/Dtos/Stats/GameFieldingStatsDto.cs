using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Games;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

/// <summary>
/// Represents fielding stats for a single game
/// </summary>
/// <param name="Season">The season of the game</param>
/// <param name="Date">The date of the game</param>
/// <param name="GameType">The type of game</param>
/// <param name="IsHome">True if the game was at home, otherwise false</param>
/// <param name="IsWin">True if the game was a win, otherwise false</param>
/// <param name="Game">Information about the game</param>
/// <param name="Stat">The game's fielding stats</param>
public sealed record GameFieldingStatsDto(
    string Season,
    DateTime Date,
    string GameType,
    bool IsHome,
    bool IsWin,
    GameDto Game,
    [property: JsonPropertyName("stat")]
    FieldingStatsDto Stat
) : GameStatsDto(Season, Date, GameType, IsHome, IsWin, Game);