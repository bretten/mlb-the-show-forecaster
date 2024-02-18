namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Improvement.Pitching;

/// <summary>
/// Published when H/9 improves
/// </summary>
/// <param name="CurrentStat"></param>
/// <param name="PreviousStat"></param>
public sealed record HitsPerNineImprovementEvent(StatSnapshot CurrentStat, StatSnapshot PreviousStat)
    : StatImprovementEvent(CurrentStat, PreviousStat);