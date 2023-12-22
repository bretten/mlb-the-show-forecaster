using System.ComponentModel.DataAnnotations;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Players.Enums;

/// <summary>
/// MLB player positions
/// </summary>
public enum Position
{
    [Display(Name = "P")] Pitcher,
    [Display(Name = "C")] Catcher,
    [Display(Name = "1B")] FirstBase,
    [Display(Name = "2B")] SecondBase,
    [Display(Name = "3B")] ThirdBase,
    [Display(Name = "SS")] Shortstop,
    [Display(Name = "LF")] LeftField,
    [Display(Name = "CF")] CenterField,
    [Display(Name = "RF")] RightField,
    [Display(Name = "OF")] OutField,
    [Display(Name = "DH")] DesignatedHitter,
    [Display(Name = "PH")] PinchHitter,
    [Display(Name = "PR")] PinchRunner,
    [Display(Name = "TWP")] TwoWayPlayer
}