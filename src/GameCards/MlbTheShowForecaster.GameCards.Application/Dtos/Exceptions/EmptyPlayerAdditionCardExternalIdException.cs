using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Exceptions;

/// <summary>
/// Thrown when a <see cref="PlayerAddition"/> has an empty <see cref="CardExternalId"/> and cannot be processed by
/// the domain
/// </summary>
public sealed class EmptyPlayerAdditionCardExternalIdException : Exception
{
    public EmptyPlayerAdditionCardExternalIdException(string? message) : base(message)
    {
    }
}