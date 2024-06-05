using System.ComponentModel.DataAnnotations;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.Enums.Attributes;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.Enums;

/// <summary>
/// Represents the different types of batting stats
/// </summary>
public enum BattingStat
{
    [Display(Name = "PlateAppearances")] PlateAppearances,

    [Display(Name = "AtBats")] AtBats,

    [Display(Name = "Runs")] Runs,

    [Display(Name = "Hits")] Hits,

    [Display(Name = "Doubles")] Doubles,

    [Display(Name = "Triples")] Triples,

    [Display(Name = "HomeRuns")] HomeRuns,

    [Display(Name = "RunsBattedIn")] RunsBattedIn,

    [Display(Name = "BaseOnBalls")] BaseOnBalls,

    [Display(Name = "IntentionalWalks")] IntentionalWalks,

    [Display(Name = "Strikeouts"), LowerStatIsBetter(true)]
    Strikeouts,

    [Display(Name = "StolenBases")] StolenBases,

    [Display(Name = "CaughtStealing"), LowerStatIsBetter(true)]
    CaughtStealing,

    [Display(Name = "HitByPitches")] HitByPitches,

    [Display(Name = "SacrificeBunts")] SacrificeBunts,

    [Display(Name = "SacrificeFlies")] SacrificeFlies,

    [Display(Name = "NumberOfPitchesSeen")]
    NumberOfPitchesSeen,

    [Display(Name = "LeftOnBase"), LowerStatIsBetter(true)]
    LeftOnBase,

    [Display(Name = "GroundOuts"), LowerStatIsBetter(true)]
    GroundOuts,

    [Display(Name = "GroundIntoDoublePlays"), LowerStatIsBetter(true)]
    GroundIntoDoublePlays,

    [Display(Name = "GroundIntoTriplePlays"), LowerStatIsBetter(true)]
    GroundIntoTriplePlays,

    [Display(Name = "AirOuts"), LowerStatIsBetter(true)]
    AirOuts,

    [Display(Name = "CatcherInterferences")]
    CatcherInterferences,

    [Display(Name = "BattingAverage")] BattingAverage,

    [Display(Name = "OnBasePercentage")] OnBasePercentage,

    [Display(Name = "BattingAverageOnBallsInPlay")]
    BattingAverageOnBallsInPlay,

    [Display(Name = "TotalBases")] TotalBases,

    [Display(Name = "Slugging")] Slugging,

    [Display(Name = "OnBasePlusSlugging")] OnBasePlusSlugging,

    [Display(Name = "StolenBasePercentage")]
    StolenBasePercentage
}