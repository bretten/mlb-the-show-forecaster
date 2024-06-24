namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Services.Exceptions;

/// <summary>
/// Thrown when <see cref="IPlayerSearchService"/> finds multiple players and is unable to determine the correct one
/// </summary>
public sealed class PlayerSearchCouldNotBeRefinedException : Exception
{
    public PlayerSearchCouldNotBeRefinedException(string? message) : base(message)
    {
    }
}