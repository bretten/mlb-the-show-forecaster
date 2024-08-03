using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;

/// <summary>
/// Represents the impact a player entering free agency has on a <see cref="PlayerCardForecast"/>
/// </summary>
/// <param name="endDate"><inheritdoc /></param>
public sealed class PlayerFreeAgencyForecastImpact(DateOnly endDate) : ForecastImpact(endDate)
{
    /// <inheritdoc />
    protected override int ImpactCoefficient => ImpactConstants.Coefficients.FreeAgency;

    /// <inheritdoc />
    protected override bool IsAdditive => false;
}