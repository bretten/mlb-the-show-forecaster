using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

/// <summary>
/// Represents the impact of a card attribute boost on a <see cref="PlayerCardForecast"/>
/// </summary>
/// <param name="boostReason">The reason the card's attributes are being boosted</param>
/// <param name="endDate"><inheritdoc /></param>
public sealed class BoostForecastImpact(string boostReason, DateOnly endDate) : ForecastImpact(endDate)
{
    /// <summary>
    /// The reason the card's attributes are being boosted
    /// </summary>
    public string BoostReason { get; } = boostReason;
}