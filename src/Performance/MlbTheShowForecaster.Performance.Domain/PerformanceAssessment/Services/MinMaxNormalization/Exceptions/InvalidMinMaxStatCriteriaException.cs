namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization.
    Exceptions;

/// <summary>
/// Thrown when a <see cref="MinMaxStatCriteria"/> is not properly configured
/// </summary>
public sealed class InvalidMinMaxStatCriteriaException : Exception
{
    public InvalidMinMaxStatCriteriaException(string? message) : base(message)
    {
    }
}