namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.Exceptions;

/// <summary>
/// Thrown when attempting to instantiate <see cref="PerformanceScore"/> with an invalid value
/// </summary>
public sealed class InvalidPerformanceScoreException : Exception
{
    public InvalidPerformanceScoreException(string? message) : base(message)
    {
    }
}