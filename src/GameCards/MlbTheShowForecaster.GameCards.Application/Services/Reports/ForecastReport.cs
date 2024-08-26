using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;

/// <summary>
/// Represents a forecast report
/// </summary>
public sealed record ForecastReport(SeasonYear Year, IReadOnlyList<PlayerCardForecast> Forecasts, string Html);