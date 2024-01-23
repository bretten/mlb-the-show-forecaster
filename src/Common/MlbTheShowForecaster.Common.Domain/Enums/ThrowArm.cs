using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using com.brettnamba.MlbTheShowForecaster.Common.Converters;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;

/// <summary>
/// The arm that the player throws with
/// </summary>
[TypeConverter(typeof(EnumDisplayNameConverter))]
public enum ThrowArm
{
    /// <summary>
    /// The player throws with the right arm
    /// </summary>
    [Display(Name = "R")] Right,

    /// <summary>
    /// The player throws with the left arm
    /// </summary>
    [Display(Name = "L")] Left,

    /// <summary>
    /// The player can throw with both arms
    /// </summary>
    [Display(Name = "S")] Switch
}