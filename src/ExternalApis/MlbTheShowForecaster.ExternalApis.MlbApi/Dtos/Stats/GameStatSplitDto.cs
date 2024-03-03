using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

public abstract record GameStatSplitDto(
    [property: JsonPropertyName("season")]
    string Season,
    [property: JsonPropertyName("date")]
    DateTime Date,
    [property: JsonPropertyName("gameType")]
    string GameType,
    [property: JsonPropertyName("isHome")]
    bool IsHome,
    [property: JsonPropertyName("isWin")]
    bool IsWin
);

public record GameStatSplitHittingDto(
    string Season,
    DateTime Date,
    string GameType,
    bool IsHome,
    bool IsWin,
    [property: JsonPropertyName("stat")]
    BattingStatsDto Stat
) : GameStatSplitDto(Season, Date, GameType, IsHome, IsWin);

public record GameStatSplitPitchingDto(
    string Season,
    DateTime Date,
    string GameType,
    bool IsHome,
    bool IsWin,
    [property: JsonPropertyName("stat")]
    PitchingStatsDto Stat
) : GameStatSplitDto(Season, Date, GameType, IsHome, IsWin);

public record GameStatSplitFieldingDto(
    string Season,
    DateTime Date,
    string GameType,
    bool IsHome,
    bool IsWin,
    [property: JsonPropertyName("stat")]
    FieldingStatsDto Stat
) : GameStatSplitDto(Season, Date, GameType, IsHome, IsWin);