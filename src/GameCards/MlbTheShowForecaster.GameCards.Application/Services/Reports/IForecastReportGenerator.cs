using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;

/// <summary>
/// Defines a service that generates a report for a collection of <see cref="PlayerCardForecast"/>
/// </summary>
public interface IForecastReportGenerator
{
    /// <summary>
    /// Generates a report for a collection of <see cref="PlayerCardForecast"/>
    /// </summary>
    /// <param name="year">The season</param>
    /// <param name="forecasts"><see cref="PlayerCardForecast"/>s to report on</param>
    /// <param name="date">The date of the report</param>
    /// <returns><see cref="ForecastReport"/></returns>
    Task<ForecastReport> Generate(SeasonYear year, IEnumerable<PlayerCardForecast> forecasts, DateOnly date);
}