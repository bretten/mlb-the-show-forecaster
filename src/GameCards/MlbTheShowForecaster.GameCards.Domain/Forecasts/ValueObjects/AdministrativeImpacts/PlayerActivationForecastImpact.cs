﻿using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;

/// <summary>
/// Represents the impact a player being activated has on a <see cref="PlayerCardForecast"/>
/// </summary>
/// <param name="endDate"><inheritdoc /></param>
public sealed class PlayerActivationForecastImpact(DateOnly startDate, DateOnly endDate)
    : ForecastImpact(startDate, endDate)
{
    /// <inheritdoc />
    public override Demand Demand => Demand.Low();
}