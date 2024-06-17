using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization;

/// <summary>
/// Defines the criteria by which to normalize and score stats
/// </summary>
public sealed record MinMaxNormalizationCriteria
{
    /// <summary>
    /// The required amount (as a percentage) for a performance score to change by for it to be considered significant.
    /// If the score has not increased or decreased past this threshold, it is negligible
    /// </summary>
    public decimal ScorePercentageChangeThreshold { get; }

    /// <summary>
    /// Criteria for batting
    /// </summary>
    public IReadOnlyList<MinMaxBattingStatCriteria> BattingCriteria { get; }

    /// <summary>
    /// Criteria for pitching
    /// </summary>
    public IReadOnlyList<MinMaxPitchingStatCriteria> PitchingCriteria { get; }

    /// <summary>
    /// Criteria for fielding
    /// </summary>
    public IReadOnlyList<MinMaxFieldingStatCriteria> FieldingCriteria { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="scorePercentageChangeThreshold">The required amount (as a percentage) for a performance score to change by for it to be considered significant</param>
    /// <param name="battingCriteria">Criteria for batting</param>
    /// <param name="pitchingCriteria">Criteria for pitching</param>
    /// <param name="fieldingCriteria">Criteria for fielding</param>
    public MinMaxNormalizationCriteria(decimal scorePercentageChangeThreshold,
        IReadOnlyList<MinMaxBattingStatCriteria> battingCriteria,
        IReadOnlyList<MinMaxPitchingStatCriteria> pitchingCriteria,
        IReadOnlyList<MinMaxFieldingStatCriteria> fieldingCriteria)
    {
        ScorePercentageChangeThreshold = scorePercentageChangeThreshold;
        BattingCriteria = battingCriteria;
        PitchingCriteria = pitchingCriteria;
        FieldingCriteria = fieldingCriteria;
        Verify();
    }

    /// <summary>
    /// Verifies that all criteria are valid
    /// </summary>
    private void Verify()
    {
        var battingWeightSum = WeightSum(BattingCriteria);
        var pitchingWeightSum = WeightSum(PitchingCriteria);
        var fieldingWeightSum = WeightSum(FieldingCriteria);
        if (battingWeightSum != 1 || pitchingWeightSum != 1 || fieldingWeightSum != 1)
        {
            throw new InvalidMinMaxNormalizationCriteriaException(battingWeightSum, pitchingWeightSum,
                fieldingWeightSum);
        }
    }

    /// <summary>
    /// Sums the weights
    /// </summary>
    private static decimal WeightSum(IEnumerable<MinMaxStatCriteria>? weights)
    {
        return weights?.Sum(x => x.Weight) ?? 0;
    }
};