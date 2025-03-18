using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Exceptions;

/// <summary>
/// Thrown when a <see cref="ICardMarketplace"/> client encounters a timezone that it cannot parse
/// </summary>
public sealed class UnknownTimezoneException : Exception
{
    public UnknownTimezoneException(string dateString) : base($"Unknown time zone: {dateString}")
    {
    }
}