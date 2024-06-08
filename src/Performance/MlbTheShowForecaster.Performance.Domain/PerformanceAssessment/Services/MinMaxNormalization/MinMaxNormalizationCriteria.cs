using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization;

/// <summary>
/// Defines the criteria by which to normalize and score stats
/// </summary>
public sealed record MinMaxNormalizationCriteria
{
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
    /// <param name="battingCriteria">Criteria for batting</param>
    /// <param name="pitchingCriteria">Criteria for pitching</param>
    /// <param name="fieldingCriteria">Criteria for fielding</param>
    public MinMaxNormalizationCriteria(IReadOnlyList<MinMaxBattingStatCriteria> battingCriteria,
        IReadOnlyList<MinMaxPitchingStatCriteria> pitchingCriteria,
        IReadOnlyList<MinMaxFieldingStatCriteria> fieldingCriteria)
    {
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