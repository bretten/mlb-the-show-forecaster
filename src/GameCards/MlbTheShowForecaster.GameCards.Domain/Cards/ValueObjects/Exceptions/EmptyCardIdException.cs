namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

/// <summary>
/// Thrown when <see cref="CardId"/> is not provided a value
/// </summary>
public sealed class EmptyCardIdException : Exception
{
    public EmptyCardIdException(string? message) : base(message)
    {
    }
}