namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

/// <summary>
/// Thrown when <see cref="CardExternalId"/> cannot be instantiated due to an invalid value
/// </summary>
public sealed class InvalidCardExternalIdException : Exception
{
    public InvalidCardExternalIdException(string? message) : base(message)
    {
    }
}