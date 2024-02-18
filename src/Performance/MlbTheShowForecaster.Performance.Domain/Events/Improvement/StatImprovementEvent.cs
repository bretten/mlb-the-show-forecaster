using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Improvement;

/// <summary>
/// Models an improvement to a stat by comparing its current value to a previous value
/// </summary>
/// <param name="CurrentStat">The current stat value</param>
/// <param name="PreviousStat">The previous stat value</param>
public abstract record StatImprovementEvent(StatSnapshot CurrentStat, StatSnapshot PreviousStat) : IDomainEvent
{
    /// <summary>
    /// The underlying percentage improvement
    /// </summary>
    private decimal? _percentageImprovement;

    /// <summary>
    /// The percentage of improvement over the previous value
    /// </summary>
    public decimal PercentageImprovement
    {
        get
        {
            if (!_percentageImprovement.HasValue)
            {
                _percentageImprovement = Math.Round(CalculatePercentageImprovement(), 2, MidpointRounding.AwayFromZero);
            }

            return _percentageImprovement.Value;
        }
    }

    /// <summary>
    /// Calculates the percentage of improvement over the previous value
    /// </summary>
    protected virtual decimal CalculatePercentageImprovement() =>
        100 * ((CurrentStat.Value - PreviousStat.Value) / PreviousStat.Value);
}