using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Exceptions;

/// <summary>
/// Thrown when <see cref="PitchingResult"/> is provided an invalid combination of pitching result scenarios
/// </summary>
public sealed class InvalidPitchingResultCombinationException : Exception
{
    public InvalidPitchingResultCombinationException(string? message) : base(message)
    {
    }
}