using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

/// <summary>
/// Pitching stats
/// </summary>
public readonly record struct PitchingStatsDto(
    [property: JsonPropertyName("summary")]
    string Summary,
    [property: JsonPropertyName("gamesPlayed")]
    int GamesPlayed,
    [property: JsonPropertyName("gamesStarted")]
    int GamesStarted,
    [property: JsonPropertyName("groundOuts")]
    int GroundOuts,
    [property: JsonPropertyName("airOuts")]
    int AirOuts,
    [property: JsonPropertyName("runs")]
    int Runs,
    [property: JsonPropertyName("doubles")]
    int Doubles,
    [property: JsonPropertyName("triples")]
    int Triples,
    [property: JsonPropertyName("homeRuns")]
    int HomeRuns,
    [property: JsonPropertyName("strikeOuts")]
    int StrikeOuts,
    [property: JsonPropertyName("baseOnBalls")]
    int BaseOnBalls,
    [property: JsonPropertyName("intentionalWalks")]
    int IntentionalWalks,
    [property: JsonPropertyName("hits")]
    int Hits,
    [property: JsonPropertyName("hitByPitch")]
    int HitByPitch,
    [property: JsonPropertyName("avg")]
    string Avg,
    [property: JsonPropertyName("atBats")]
    int AtBats,
    [property: JsonPropertyName("obp")]
    string Obp,
    [property: JsonPropertyName("slg")]
    string Slg,
    [property: JsonPropertyName("ops")]
    string Ops,
    [property: JsonPropertyName("caughtStealing")]
    int CaughtStealing,
    [property: JsonPropertyName("stolenBases")]
    int StolenBases,
    [property: JsonPropertyName("stolenBasePercentage")]
    string StolenBasePercentage,
    [property: JsonPropertyName("groundIntoDoublePlay")]
    int GroundIntoDoublePlay,
    [property: JsonPropertyName("numberOfPitches")]
    int NumberOfPitches,
    [property: JsonPropertyName("era")]
    string Era,
    [property: JsonPropertyName("inningsPitched")]
    string InningsPitched,
    [property: JsonPropertyName("wins")]
    int Wins,
    [property: JsonPropertyName("losses")]
    int Losses,
    [property: JsonPropertyName("saves")]
    int Saves,
    [property: JsonPropertyName("saveOpportunities")]
    int SaveOpportunities,
    [property: JsonPropertyName("holds")]
    int Holds,
    [property: JsonPropertyName("blownSaves")]
    int BlownSaves,
    [property: JsonPropertyName("earnedRuns")]
    int EarnedRuns,
    [property: JsonPropertyName("whip")]
    string Whip,
    [property: JsonPropertyName("battersFaced")]
    int BattersFaced,
    [property: JsonPropertyName("outs")]
    int Outs,
    [property: JsonPropertyName("gamesPitched")]
    int GamesPitched,
    [property: JsonPropertyName("completeGames")]
    int CompleteGames,
    [property: JsonPropertyName("shutouts")]
    int Shutouts,
    [property: JsonPropertyName("strikes")]
    int Strikes,
    [property: JsonPropertyName("strikePercentage")]
    string StrikePercentage,
    [property: JsonPropertyName("hitBatsmen")]
    int HitBatsmen,
    [property: JsonPropertyName("balks")]
    int Balks,
    [property: JsonPropertyName("wildPitches")]
    int WildPitches,
    [property: JsonPropertyName("pickoffs")]
    int Pickoffs,
    [property: JsonPropertyName("totalBases")]
    int TotalBases,
    [property: JsonPropertyName("groundOutsToAirouts")]
    string GroundOutsToAirOuts,
    [property: JsonPropertyName("winPercentage")]
    string WinPercentage,
    [property: JsonPropertyName("pitchesPerInning")]
    string PitchesPerInning,
    [property: JsonPropertyName("gamesFinished")]
    int GamesFinished,
    [property: JsonPropertyName("strikeoutWalkRatio")]
    string StrikeoutWalkRatio,
    [property: JsonPropertyName("strikeoutsPer9Inn")]
    string StrikeoutsPer9Inn,
    [property: JsonPropertyName("walksPer9Inn")]
    string WalksPer9Inn,
    [property: JsonPropertyName("hitsPer9Inn")]
    string HitsPer9Inn,
    [property: JsonPropertyName("runsScoredPer9")]
    string RunsScoredPer9,
    [property: JsonPropertyName("homeRunsPer9")]
    string HomeRunsPer9,
    [property: JsonPropertyName("inheritedRunners")]
    int InheritedRunners,
    [property: JsonPropertyName("inheritedRunnersScored")]
    int InheritedRunnersScored,
    [property: JsonPropertyName("catchersInterference")]
    int CatchersInterference,
    [property: JsonPropertyName("sacBunts")]
    int SacBunts,
    [property: JsonPropertyName("sacFlies")]
    int SacFlies
);