using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Reports.Exceptions;

/// <summary>
/// Thrown when <see cref="StringReplacementForecastReportGenerator"/> cannot find a <see cref="PlayerCard"/> corresponding
/// to the <see cref="PlayerCardForecast"/> it is reporting on
/// </summary>
public sealed class NoPlayerCardForForecastReportException : Exception
{
    public NoPlayerCardForForecastReportException(string? message) : base(message)
    {
    }
}