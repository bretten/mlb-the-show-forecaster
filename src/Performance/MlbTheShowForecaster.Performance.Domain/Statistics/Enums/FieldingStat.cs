using System.ComponentModel.DataAnnotations;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.Enums.Attributes;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.Enums;

/// <summary>
/// Represents the different types of fielding stats
/// </summary>
public enum FieldingStat
{
    [Display(Name = "GamesStarted")] GamesStarted,

    [Display(Name = "InningsPlayed")] InningsPlayed,

    [Display(Name = "Assists")] Assists,

    [Display(Name = "Putouts")] Putouts,

    [Display(Name = "Errors"), LowerStatIsBetter(true)]
    Errors,

    [Display(Name = "ThrowingErrors"), LowerStatIsBetter(true)]
    ThrowingErrors,

    [Display(Name = "DoublePlays")] DoublePlays,

    [Display(Name = "TriplePlays")] TriplePlays,

    [Display(Name = "FieldingPercentage")] FieldingPercentage,

    [Display(Name = "TotalChances")] TotalChances,

    [Display(Name = "RangeFactorPer9")] RangeFactorPer9,

    [Display(Name = "CaughtStealing")] CaughtStealing,

    [Display(Name = "StolenBases"), LowerStatIsBetter(true)]
    StolenBases,

    [Display(Name = "PassedBalls"), LowerStatIsBetter(true)]
    PassedBalls,

    [Display(Name = "CatcherInterferences"), LowerStatIsBetter(true)]
    CatcherInterferences,

    [Display(Name = "WildPitches"), LowerStatIsBetter(true)]
    WildPitches,

    [Display(Name = "Pickoffs")] Pickoffs,

    [Display(Name = "StolenBasePercentage"), LowerStatIsBetter(true)]
    StolenBasePercentage,
}