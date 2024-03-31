namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;

/// <summary>
/// Thrown when <see cref="IPlayerCardTracker"/> found no player cards, which is not a real world case
/// </summary>
public sealed class PlayerCardTrackerFoundNoCardsException : Exception
{
    public PlayerCardTrackerFoundNoCardsException(string? message) : base(message)
    {
    }
}