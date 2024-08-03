using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;

/// <summary>
/// Represents the impact a player being activated has on a <see cref="PlayerCardForecast"/>
/// </summary>
/// <param name="endDate"><inheritdoc /></param>
public sealed class PlayerActivationForecastImpact(DateOnly endDate) : ForecastImpact(endDate)
{
    /// <inheritdoc />
    protected override int ImpactCoefficient => ImpactConstants.Coefficients.Activation;

    /// <inheritdoc />
    protected override bool IsAdditive => true;
}