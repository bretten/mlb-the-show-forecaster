using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;

/// <summary>
/// Represents a comparison between an old and new <see cref="PerformanceScore"/>
/// </summary>
public class PerformanceScoreComparison : PercentageChange
{
    /// <summary>
    /// If there is a reference value of 0, we want to treat that as a non-infinite increase. For example, a score
    /// changing from 0 to 0.01 is different from it changing from 0 to 0.1. This difference needs to be measured
    /// </summary>
    public override bool TreatZeroReferenceValueAsOne => true;

    /// <summary>
    /// The required amount (as a percentage) for a performance score to change by for it to be considered significant.
    /// If the score has not increased or decreased past this threshold, it is negligible
    /// </summary>
    private decimal PercentageChangeThreshold { get; }

    /// <summary>
    /// True if the score has increased by a significant amount
    /// </summary>
    public bool IsSignificantIncrease => HasIncreasedBy(PercentageChangeThreshold);

    /// <summary>
    /// True if the score has decreased by a significant amount
    /// </summary>
    public bool IsSignificantDecrease => HasDecreasedBy(PercentageChangeThreshold);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="oldScore">The old score</param>
    /// <param name="newScore">The new score</param>
    /// <param name="percentageChangeThreshold">The required amount (as a percentage) for a performance score to change by for it to be considered significant</param>
    private PerformanceScoreComparison(PerformanceScore oldScore, PerformanceScore newScore,
        decimal percentageChangeThreshold) : base(oldScore.Value, newScore.Value)
    {
        PercentageChangeThreshold = percentageChangeThreshold;
    }

    /// <summary>
    /// Creates a <see cref="PerformanceScoreComparison"/>
    /// </summary>
    /// <param name="oldScore">The old score</param>
    /// <param name="newScore">The new score</param>
    /// <param name="percentageChangeThreshold">The required amount (as a percentage) for a performance score to change by for it to be considered significant</param>
    /// <returns><see cref="PerformanceScoreComparison"/></returns>
    public static PerformanceScoreComparison Create(PerformanceScore oldScore, PerformanceScore newScore,
        decimal percentageChangeThreshold)
    {
        return new PerformanceScoreComparison(oldScore: oldScore, newScore: newScore, percentageChangeThreshold);
    }
}