﻿using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;

/// <summary>
/// Represents the impact a player being deactivated has on a <see cref="PlayerCardForecast"/>
/// </summary>
/// <param name="endDate"><inheritdoc /></param>
public sealed class PlayerDeactivationForecastImpact(DateOnly startDate, DateOnly endDate)
    : ForecastImpact(startDate, endDate)
{
    /// <inheritdoc />
    public override Demand Demand => Demand.Loss();
}