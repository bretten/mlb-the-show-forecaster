namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Services.Exceptions;

/// <summary>
/// Thrown when <see cref="IPerformanceTracker"/> found no player seasons in the domain. It is not
/// a real world scenario and needs to be examined
/// </summary>
public sealed class PerformanceTrackerFoundNoPlayerSeasonsException : Exception
{
    public PerformanceTrackerFoundNoPlayerSeasonsException(string? message) : base(message)
    {
    }
}