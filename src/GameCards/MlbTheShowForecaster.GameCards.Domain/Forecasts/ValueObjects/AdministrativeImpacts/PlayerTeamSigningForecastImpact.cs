using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;

/// <summary>
/// Represents the impact a player signing with a team has on a <see cref="PlayerCardForecast"/>
/// </summary>
/// <param name="endDate"><inheritdoc /></param>
public sealed class PlayerTeamSigningForecastImpact(DateOnly endDate) : ForecastImpact(endDate)
{
    /// <inheritdoc />
    protected override int ImpactCoefficient => ImpactConstants.Coefficients.TeamSigning;

    /// <inheritdoc />
    protected override bool IsAdditive => true;
}