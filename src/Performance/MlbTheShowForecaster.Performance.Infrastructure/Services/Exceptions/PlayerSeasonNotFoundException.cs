namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Services.Exceptions;

/// <summary>
/// Thrown when <see cref="MlbApiPlayerStats"/> cannot find a player's season
/// </summary>
public sealed class PlayerSeasonNotFoundException : Exception
{
    public PlayerSeasonNotFoundException(string? message) : base(message)
    {
    }
}