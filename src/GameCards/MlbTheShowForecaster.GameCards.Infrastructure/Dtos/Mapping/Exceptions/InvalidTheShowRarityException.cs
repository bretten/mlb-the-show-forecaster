using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;

/// <summary>
/// Thrown when trying to map a string value from MLB The Show to a <see cref="Rarity"/>, but the string value does
/// not match our domain
/// </summary>
public sealed class InvalidTheShowRarityException : Exception
{
    public InvalidTheShowRarityException(string? message) : base(message)
    {
    }
}