using System.ComponentModel.DataAnnotations;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Players.Enums;

/// <summary>
/// The side that a player bats on
/// </summary>
public enum BatSide
{
    [Display(Name = "R")] Right,
    [Display(Name = "L")] Left,
    [Display(Name = "S")] Switch
}