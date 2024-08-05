using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;

/// <summary>
/// Represents the impact the player's fielding stats has on the <see cref="PlayerCardForecast"/>
/// </summary>
/// <param name="oldScore"><inheritdoc cref="StatsForecastImpact.OldScore"/></param>
/// <param name="newScore"><inheritdoc cref="StatsForecastImpact.NewScore"/></param>
/// <param name="endDate"><inheritdoc cref="StatsForecastImpact.EndDate"/></param>
public sealed class FieldingStatsForecastImpact(decimal oldScore, decimal newScore, DateOnly endDate)
    : StatsForecastImpact(oldScore, newScore, endDate);