namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

/// <summary>
/// Thrown when trying to instantiate <see cref="CardImageLocation"/> with an invalid value
/// </summary>
public sealed class InvalidCardImageLocationException : Exception
{
    public InvalidCardImageLocationException(string? message) : base(message)
    {
    }
}