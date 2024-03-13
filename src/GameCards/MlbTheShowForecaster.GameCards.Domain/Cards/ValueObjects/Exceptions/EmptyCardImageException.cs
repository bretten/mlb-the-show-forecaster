namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

/// <summary>
/// Thrown when <see cref="CardImage"/> is not provided an image
/// </summary>
public sealed class EmptyCardImageException : Exception
{
    public EmptyCardImageException(string? message) : base(message)
    {
    }
}