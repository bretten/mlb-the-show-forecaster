using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;

/// <summary>
/// Thrown when a string value for a card series from MLB The Show cannot be mapped to this domain's <see cref="CardSeries"/>
/// </summary>
public sealed class InvalidTheShowCardSeriesException : Exception
{
    public InvalidTheShowCardSeriesException(string? message) : base(message)
    {
    }
}