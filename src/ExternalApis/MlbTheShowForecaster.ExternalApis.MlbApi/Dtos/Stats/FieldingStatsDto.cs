using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

/// <summary>
/// Fielding stats
/// </summary>
public readonly record struct FieldingStatsDto(
    [property: JsonPropertyName("gamesPlayed")]
    int GamesPlayed,
    [property: JsonPropertyName("gamesStarted")]
    int GamesStarted,
    [property: JsonPropertyName("caughtStealing")]
    int CaughtStealing,
    [property: JsonPropertyName("stolenBases")]
    int StolenBases,
    [property: JsonPropertyName("stolenBasePercentage")]
    string StolenBasePercentage,
    [property: JsonPropertyName("assists")]
    int Assists,
    [property: JsonPropertyName("putOuts")]
    int Putouts,
    [property: JsonPropertyName("errors")]
    int Errors,
    [property: JsonPropertyName("chances")]
    int Chances,
    [property: JsonPropertyName("fielding")]
    string Fielding,
    [property: JsonPropertyName("position")]
    PositionDto Position,
    [property: JsonPropertyName("rangeFactorPerGame")]
    string RangeFactorPerGame,
    [property: JsonPropertyName("rangeFactorPer9Inn")]
    string RangeFactorPer9Inn,
    [property: JsonPropertyName("innings")]
    string Innings,
    [property: JsonPropertyName("games")]
    int Games,
    [property: JsonPropertyName("passedBall")]
    int PassedBall,
    [property: JsonPropertyName("doublePlays")]
    int DoublePlays,
    [property: JsonPropertyName("triplePlays")]
    int TriplePlays,
    [property: JsonPropertyName("catcherERA")]
    string CatcherEra,
    [property: JsonPropertyName("catchersInterference")]
    int CatcherInterferences,
    [property: JsonPropertyName("wildPitches")]
    int WildPitches,
    [property: JsonPropertyName("throwingErrors")]
    int ThrowingErrors,
    [property: JsonPropertyName("pickoffs")]
    int Pickoffs
);