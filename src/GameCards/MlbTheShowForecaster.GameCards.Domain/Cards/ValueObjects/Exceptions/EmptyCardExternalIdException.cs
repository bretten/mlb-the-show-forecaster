namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

/// <summary>
/// Thrown when <see cref="CardExternalId"/> is not provided a value
/// </summary>
public sealed class EmptyCardExternalIdException : Exception
{
    public EmptyCardExternalIdException(string? message) : base(message)
    {
    }
}