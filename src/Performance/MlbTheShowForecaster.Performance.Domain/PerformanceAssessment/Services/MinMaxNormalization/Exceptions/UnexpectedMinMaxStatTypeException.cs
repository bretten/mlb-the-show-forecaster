namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization.
    Exceptions;

/// <summary>
/// Thrown when <see cref="MinMaxNormalizationPerformanceAssessor"/> tries to score a stat with an unexpected type
/// </summary>
public sealed class UnexpectedMinMaxStatTypeException : Exception
{
    public UnexpectedMinMaxStatTypeException(string? message) : base(message)
    {
    }
}