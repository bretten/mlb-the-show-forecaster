namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Improvement.Pitching;

/// <summary>
/// Published when ERA improves
/// </summary>
/// <param name="CurrentStat">The current stat value</param>
/// <param name="PreviousStat">The previous stat value</param>
public sealed record EraImprovementEvent(StatSnapshot CurrentStat, StatSnapshot PreviousStat)
    : StatImprovementEvent(CurrentStat, PreviousStat)
{
    /// <summary>
    /// Calculates the percentage of improvement over the previous value. Since a lower ERA is better, take the absolute value
    /// </summary>
    protected override decimal CalculatePercentageImprovement() =>
        Math.Abs(100 * ((CurrentStat.Value - PreviousStat.Value) / PreviousStat.Value));
}