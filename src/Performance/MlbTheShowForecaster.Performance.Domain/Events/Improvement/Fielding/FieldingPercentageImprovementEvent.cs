namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Improvement.Fielding;

/// <summary>
/// Published when fielding percentage improves
/// </summary>
/// <param name="CurrentStat">The current stat value</param>
/// <param name="PreviousStat">The previous stat value</param>
public sealed record FieldingPercentageImprovementEvent(StatSnapshot CurrentStat, StatSnapshot PreviousStat)
    : StatImprovementEvent(CurrentStat, PreviousStat);