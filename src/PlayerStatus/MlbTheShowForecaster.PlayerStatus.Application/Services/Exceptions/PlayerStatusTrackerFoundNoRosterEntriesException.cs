namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services.Exceptions;

/// <summary>
/// Thrown when <see cref="IPlayerStatusTracker"/> could not find any player statuses which is not a real world scenario
/// and should be examined
/// </summary>
public sealed class PlayerStatusTrackerFoundNoRosterEntriesException : Exception
{
    public PlayerStatusTrackerFoundNoRosterEntriesException(string? message) : base(message)
    {
    }
}