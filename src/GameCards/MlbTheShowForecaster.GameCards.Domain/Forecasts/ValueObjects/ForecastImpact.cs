using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

/// <summary>
/// Should define some external event that has an effect on a <see cref="PlayerCardForecast"/>
/// </summary>
/// <param name="endDate">When this impact no longer has influence over the forecast</param>
public abstract class ForecastImpact(DateOnly endDate) : ValueObject
{
    /// <summary>
    /// When this impact no longer has influence over the forecast
    /// </summary>
    public DateOnly EndDate { get; } = endDate;
}