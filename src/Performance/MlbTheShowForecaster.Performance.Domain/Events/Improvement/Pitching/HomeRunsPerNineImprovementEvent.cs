namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Improvement.Pitching;

/// <summary>
/// Published when HR/9 improves
/// </summary>
/// <param name="CurrentStat">The current stat value</param>
/// <param name="PreviousStat">The previous stat value</param>
public sealed record HomeRunsPerNineImprovementEvent(StatSnapshot CurrentStat, StatSnapshot PreviousStat)
    : StatImprovementEvent(CurrentStat, PreviousStat);