using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;

/// <summary>
/// Defines a service that publishes the forecast report
/// </summary>
public interface IForecastReportPublisher
{
    /// <summary>
    /// Publishes the forecast report for the specified season and date
    /// </summary>
    /// <param name="year">The season of the report</param>
    /// <param name="date">The date of the report</param>
    /// <returns>The completed task</returns>
    Task Publish(SeasonYear year, DateOnly date);
}