namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

/// <summary>
/// Thrown when <see cref="CardName"/> is provided an empty value
/// </summary>
public sealed class EmptyCardNameException : Exception
{
    public EmptyCardNameException(string? message) : base(message)
    {
    }
}