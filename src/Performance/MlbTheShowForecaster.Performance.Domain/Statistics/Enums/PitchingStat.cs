using System.ComponentModel.DataAnnotations;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.Enums.Attributes;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.Enums;

/// <summary>
/// Represents the different types of pitching stats
/// </summary>
public enum PitchingStat
{
    [Display(Name = "Wins")] Wins,

    [Display(Name = "Losses"), LowerStatIsBetter(true)]
    Losses,

    [Display(Name = "GamesStarted")] GamesStarted,

    [Display(Name = "GamesFinished")] GamesFinished,

    [Display(Name = "CompleteGames")] CompleteGames,

    [Display(Name = "Shutouts")] Shutouts,

    [Display(Name = "Holds")] Holds,

    [Display(Name = "Saves")] Saves,

    [Display(Name = "BlownSaves"), LowerStatIsBetter(true)]
    BlownSaves,

    [Display(Name = "SaveOpportunities")] SaveOpportunities,

    [Display(Name = "InningsPitched")] InningsPitched,

    [Display(Name = "Hits"), LowerStatIsBetter(true)]
    Hits,

    [Display(Name = "Doubles"), LowerStatIsBetter(true)]
    Doubles,

    [Display(Name = "Triples"), LowerStatIsBetter(true)]
    Triples,

    [Display(Name = "HomeRuns"), LowerStatIsBetter(true)]
    HomeRuns,

    [Display(Name = "Runs"), LowerStatIsBetter(true)]
    Runs,

    [Display(Name = "EarnedRuns"), LowerStatIsBetter(true)]
    EarnedRuns,

    [Display(Name = "Strikeouts")] Strikeouts,

    [Display(Name = "BaseOnBalls"), LowerStatIsBetter(true)]
    BaseOnBalls,

    [Display(Name = "IntentionalWalks"), LowerStatIsBetter(true)]
    IntentionalWalks,

    [Display(Name = "HitBatsmen"), LowerStatIsBetter(true)]
    HitBatsmen,

    [Display(Name = "Outs")] Outs,

    [Display(Name = "GroundOuts")] GroundOuts,

    [Display(Name = "AirOuts")] AirOuts,

    [Display(Name = "GroundIntoDoublePlays")]
    GroundIntoDoublePlays,

    [Display(Name = "NumberOfPitches"), LowerStatIsBetter(true)]
    NumberOfPitches,

    [Display(Name = "Strikes")] Strikes,

    [Display(Name = "WildPitches"), LowerStatIsBetter(true)]
    WildPitches,

    [Display(Name = "Balks"), LowerStatIsBetter(true)]
    Balks,

    [Display(Name = "BattersFaced")] BattersFaced,

    [Display(Name = "AtBats")] AtBats,

    [Display(Name = "StolenBases"), LowerStatIsBetter(true)]
    StolenBases,

    [Display(Name = "CaughtStealing")] CaughtStealing,

    [Display(Name = "Pickoffs")] Pickoffs,

    [Display(Name = "InheritedRunners")] InheritedRunners,

    [Display(Name = "InheritedRunnersScored"), LowerStatIsBetter(true)]
    InheritedRunnersScored,

    [Display(Name = "CatcherInterferences"), LowerStatIsBetter(true)]
    CatcherInterferences,

    [Display(Name = "SacrificeBunts"), LowerStatIsBetter(true)]
    SacrificeBunts,

    [Display(Name = "SacrificeFlies"), LowerStatIsBetter(true)]
    SacrificeFlies,

    [Display(Name = "QualityStart")] QualityStart,

    [Display(Name = "EarnedRunAverage"), LowerStatIsBetter(true)]
    EarnedRunAverage,

    [Display(Name = "OpponentsBattingAverage"), LowerStatIsBetter(true)]
    OpponentsBattingAverage,

    [Display(Name = "OpponentsOnBasePercentage"), LowerStatIsBetter(true)]
    OpponentsOnBasePercentage,

    [Display(Name = "TotalBases"), LowerStatIsBetter(true)]
    TotalBases,

    [Display(Name = "Slugging"), LowerStatIsBetter(true)]
    Slugging,

    [Display(Name = "OpponentsOnBasePlusSlugging"), LowerStatIsBetter(true)]
    OpponentsOnBasePlusSlugging,

    [Display(Name = "PitchesPerInning"), LowerStatIsBetter(true)]
    PitchesPerInning,

    [Display(Name = "StrikePercentage")] StrikePercentage,

    [Display(Name = "WalksPlusHitsPerInningPitched"), LowerStatIsBetter(true)]
    WalksPlusHitsPerInningPitched,

    [Display(Name = "StrikeoutToWalkRatio")]
    StrikeoutToWalkRatio,

    [Display(Name = "HitsPer9"), LowerStatIsBetter(true)]
    HitsPer9,

    [Display(Name = "StrikeoutsPer9"), LowerStatIsBetter(true)]
    StrikeoutsPer9,

    [Display(Name = "BaseOnBallsPer9"), LowerStatIsBetter(true)]
    BaseOnBallsPer9,

    [Display(Name = "RunsScoredPer9"), LowerStatIsBetter(true)]
    RunsScoredPer9,

    [Display(Name = "HomeRunsPer9"), LowerStatIsBetter(true)]
    HomeRunsPer9,

    [Display(Name = "StolenBasePercentage"), LowerStatIsBetter(true)]
    StolenBasePercentage,
}