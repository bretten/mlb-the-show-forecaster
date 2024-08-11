namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Exceptions;

/// <summary>
/// Thrown when <see cref="PlayerMatcher"/> could not match a player to the specified criteria
/// </summary>
public sealed class PlayerCouldNotBeMatchedException : Exception
{
    public PlayerCouldNotBeMatchedException(string? message) : base(message)
    {
    }
}