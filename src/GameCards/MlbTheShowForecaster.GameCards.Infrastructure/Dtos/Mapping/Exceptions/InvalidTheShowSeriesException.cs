using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;

/// <summary>
/// Thrown when trying to map a card series string value to <see cref="CardSeries"/>, but the string value
/// does not match any known values
/// </summary>
public sealed class InvalidTheShowSeriesException : Exception
{
    public InvalidTheShowSeriesException(string? message) : base(message)
    {
    }
}