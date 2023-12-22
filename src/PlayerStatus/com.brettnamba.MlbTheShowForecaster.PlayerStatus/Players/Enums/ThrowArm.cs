using System.ComponentModel.DataAnnotations;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Players.Enums;

/// <summary>
/// The arm that the player throws with
/// </summary>
public enum ThrowArm
{
    [Display(Name = "R")] Right,
    [Display(Name = "L")] Left,
    [Display(Name = "S")] Switch
}