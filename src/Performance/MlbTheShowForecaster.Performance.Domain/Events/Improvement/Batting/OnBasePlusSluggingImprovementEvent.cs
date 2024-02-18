namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Improvement.Batting;

/// <summary>
/// Published when there is an improvement in OPS
/// </summary>
/// <param name="CurrentStat">The current stat value</param>
/// <param name="PreviousStat">The previous stat value</param>
public sealed record OnBasePlusSluggingImprovementEvent(StatSnapshot CurrentStat, StatSnapshot PreviousStat)
    : StatImprovementEvent(CurrentStat, PreviousStat);