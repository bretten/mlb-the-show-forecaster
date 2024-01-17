using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using com.brettnamba.MlbTheShowForecaster.Common.Converters;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;

/// <summary>
/// The side that a player bats on
/// </summary>
[TypeConverter(typeof(EnumDisplayNameConverter))]
public enum BatSide
{
    /// <summary>
    /// The player bats on the right side
    /// </summary>
    [Display(Name = "R")] Right,

    /// <summary>
    /// The player bats on the left side
    /// </summary>
    [Display(Name = "L")] Left,

    /// <summary>
    /// The player can bat on the right or left side
    /// </summary>
    [Display(Name = "S")] Switch
}