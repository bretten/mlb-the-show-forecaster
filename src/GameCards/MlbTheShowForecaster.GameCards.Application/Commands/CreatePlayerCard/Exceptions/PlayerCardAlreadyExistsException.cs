namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCard.Exceptions;

/// <summary>
/// Thrown when a <see cref="CreatePlayerCardCommand"/> is sent for a player card that already exists
/// </summary>
public sealed class PlayerCardAlreadyExistsException : Exception
{
    public PlayerCardAlreadyExistsException(string? message) : base(message)
    {
    }
}