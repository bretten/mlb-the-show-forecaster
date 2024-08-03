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

    /// <inheritdoc />
    protected override int ImpactCoefficient => ImpactConstants.Coefficients.Boost;

    /// <inheritdoc />
    protected override bool IsAdditive => true;

    /// <summary>
    /// The demand during the peak of the boost is at the standard level, but when the boost is about to end,
    /// the marketplace sees a lot of people trying to sell off the card, so demand is diminished
    /// </summary>
    /// <param name="date"><inheritdoc /></param>
    /// <returns><inheritdoc /></returns>
    public override int DemandOn(DateOnly date)
    {
        var daysUntilBoostEnds = EndDate.DayNumber - date.DayNumber;

        return daysUntilBoostEnds switch
        {
            // No days left
            < 0 => 0,
            // People try to sell off the boosted cards when the boost is about to end, so demand is diminished
            <= 1 => Demand / 2,
            // Boost is active
            _ => Demand
        };
    }
}