using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using com.brettnamba.MlbTheShowForecaster.Common.Converters;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;

/// <summary>
/// MLB player positions
/// </summary>
[TypeConverter(typeof(EnumDisplayNameConverter))]
public enum Position
{
    /// <summary>
    /// Pitcher (P)
    /// </summary>
    [Display(Name = "P")] Pitcher,

    /// <summary>
    /// Catcher (C)
    /// </summary>
    [Display(Name = "C")] Catcher,

    /// <summary>
    /// First base (1B)
    /// </summary>
    [Display(Name = "1B")] FirstBase,

    /// <summary>
    /// Second base (2B)
    /// </summary>
    [Display(Name = "2B")] SecondBase,

    /// <summary>
    /// Third base (3B)
    /// </summary>
    [Display(Name = "3B")] ThirdBase,

    /// <summary>
    /// Shortstop (SS)
    /// </summary>
    [Display(Name = "SS")] Shortstop,

    /// <summary>
    /// Infield (IF)
    /// </summary>
    [Display(Name = "IF")] Infield,

    /// <summary>
    /// Left field (LF)
    /// </summary>
    [Display(Name = "LF")] LeftField,

    /// <summary>
    /// Center field (CF)
    /// </summary>
    [Display(Name = "CF")] CenterField,

    /// <summary>
    /// Right field (RF)
    /// </summary>
    [Display(Name = "RF")] RightField,

    /// <summary>
    /// Out field (OF)
    /// </summary>
    [Display(Name = "OF")] OutField,

    /// <summary>
    /// Designated hitter (DH)
    /// </summary>
    [Display(Name = "DH")] DesignatedHitter,

    /// <summary>
    /// Pinch hitter (PH)
    /// </summary>
    [Display(Name = "PH")] PinchHitter,

    /// <summary>
    /// Pinch runner (PR)
    /// </summary>
    [Display(Name = "PR")] PinchRunner,

    /// <summary>
    /// Two-way player (TWP)
    /// </summary>
    [Display(Name = "TWP")] TwoWayPlayer,

    /// <summary>
    /// Starting pitcher (SP)
    /// </summary>
    [Display(Name = "SP")] StartingPitcher,

    /// <summary>
    /// Relief pitcher (RP)
    /// </summary>
    [Display(Name = "RP")] ReliefPitcher,

    /// <summary>
    /// Closing pitcher (CP)
    /// </summary>
    [Display(Name = "CP")] ClosingPitcher,

    /// <summary>
    /// None
    /// </summary>
    [Display(Name = "NONE")] None
}