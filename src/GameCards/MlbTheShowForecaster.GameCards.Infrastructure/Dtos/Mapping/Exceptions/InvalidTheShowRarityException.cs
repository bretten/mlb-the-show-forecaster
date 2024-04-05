using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;

/// <summary>
/// Thrown when a string value for rarity from MLB The Show cannot be mapped to this domain's <see cref="Rarity"/>
/// </summary>
public sealed class InvalidTheShowRarityException : Exception
{
    public InvalidTheShowRarityException(string? message) : base(message)
    {
    }
}