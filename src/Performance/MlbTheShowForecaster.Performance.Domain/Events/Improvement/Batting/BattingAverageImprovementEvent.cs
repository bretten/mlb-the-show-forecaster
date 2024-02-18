namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Improvement.Batting;

/// <summary>
/// Published when batting average improves
/// </summary>
/// <param name="CurrentStat">The current stat value</param>
/// <param name="PreviousStat">The previous stat value</param>
public sealed record BattingAverageImprovementEvent(StatSnapshot CurrentStat, StatSnapshot PreviousStat)
    : StatImprovementEvent(CurrentStat, PreviousStat);