namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization.
    Exceptions;

/// <summary>
/// Thrown when <see cref="MinMaxNormalizationCriteria"/> is invalid
/// </summary>
public sealed class InvalidMinMaxNormalizationCriteriaException : Exception
{
    public InvalidMinMaxNormalizationCriteriaException(decimal battingWeightSum, decimal pitchingWeightSum,
        decimal fieldingWeightSum) : base(GenerateMessage(battingWeightSum, pitchingWeightSum, fieldingWeightSum))
    {
    }

    private static string GenerateMessage(decimal battingWeightSum, decimal pitchingWeightSum,
        decimal fieldingWeightSum)
    {
        return
            $"The weight sum for each stat set needs to be 1. The sums were: Batting = {battingWeightSum}, Pitching = {pitchingWeightSum}, Fielding = {fieldingWeightSum}";
    }
}