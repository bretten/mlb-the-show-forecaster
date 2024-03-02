﻿using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

/// <summary>
/// Batting stats
/// </summary>
public readonly record struct BattingStatsDto(
    [property: JsonPropertyName("summary")]
    string Summary,
    [property: JsonPropertyName("gamesPlayed")]
    int GamesPlayed,
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
    [property: JsonPropertyName("groundIntoTriplePlay")]
    int GroundIntoTriplePlay,
    [property: JsonPropertyName("numberOfPitches")]
    int NumberOfPitches,
    [property: JsonPropertyName("plateAppearances")]
    int PlateAppearances,
    [property: JsonPropertyName("totalBases")]
    int TotalBases,
    [property: JsonPropertyName("rbi")]
    int Rbi,
    [property: JsonPropertyName("leftOnBase")]
    int LeftOnBase,
    [property: JsonPropertyName("sacBunts")]
    int SacBunts,
    [property: JsonPropertyName("sacFlies")]
    int SacFlies,
    [property: JsonPropertyName("babip")]
    string Babip,
    [property: JsonPropertyName("groundOutsToAirouts")]
    string GroundOutsToAirOuts,
    [property: JsonPropertyName("catchersInterference")]
    int CatchersInterference,
    [property: JsonPropertyName("atBatsPerHomeRun")]
    string AtBatsPerHomeRun
);