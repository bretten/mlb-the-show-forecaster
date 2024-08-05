using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetImpactedPlayerCardForecasts;

/// <summary>
/// Gets any <see cref="PlayerCardForecast"/>s that are predicted to have demand changes on the specified date
/// due to <see cref="ForecastImpact"/>s
/// </summary>
/// <param name="Date">Date (inclusive) when any <see cref="PlayerCardForecast"/>s still remain influenced by <see cref="ForecastImpact"/>s</param>
internal readonly record struct GetImpactedPlayerCardForecastsQuery(DateOnly Date)
    : IQuery<IEnumerable<PlayerCardForecast>>;