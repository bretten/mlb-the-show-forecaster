namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Services.Exceptions;

/// <summary>
/// Thrown when <see cref="MlbApiPlayerStats"/> cannot find any players for the season
/// </summary>
public sealed class NoPlayerSeasonsFoundException : Exception
{
    public NoPlayerSeasonsFoundException(string? message) : base(message)
    {
    }
}