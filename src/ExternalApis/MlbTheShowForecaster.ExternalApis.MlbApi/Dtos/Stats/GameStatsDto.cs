using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Games;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

/// <summary>
/// Represents stats for a single game
/// </summary>
/// <param name="Season">The season of the game</param>
/// <param name="Date">The date of the game</param>
/// <param name="GameType">The type of game</param>
/// <param name="IsHome">True if the game was at home, otherwise false</param>
/// <param name="IsWin">True if the game was a win, otherwise false</param>
/// <param name="Game">Information about the game</param>
public abstract record GameStatsDto(
    [property: JsonPropertyName("season")]
    string Season,
    [property: JsonPropertyName("date")]
    DateTime Date,
    [property: JsonPropertyName("gameType")]
    string GameType,
    [property: JsonPropertyName("isHome")]
    bool IsHome,
    [property: JsonPropertyName("isWin")]
    bool IsWin,
    [property: JsonPropertyName("game")]
    GameDto Game
);