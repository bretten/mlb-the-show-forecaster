using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

/// <summary>
/// Represents the impact a card price change has on a <see cref="PlayerCardForecast"/>
/// </summary>
/// <param name="oldPrice">The old price</param>
/// <param name="newPrice">The new price</param>
/// <param name="endDate"><inheritdoc /></param>
public sealed class PriceForecastImpact(NaturalNumber oldPrice, NaturalNumber newPrice, DateOnly endDate)
    : ForecastImpact(endDate)
{
    /// <summary>
    /// The old price
    /// </summary>
    public NaturalNumber OldPrice { get; } = oldPrice;

    /// <summary>
    /// The new price
    /// </summary>
    public NaturalNumber NewPrice { get; } = newPrice;

    /// <summary>
    /// The percentage change between the old and new price
    /// </summary>
    public PercentageChange PercentageChange => PercentageChange.Create(OldPrice, NewPrice);

    /// <inheritdoc />
    protected override int ImpactCoefficient => ImpactConstants.Coefficients.FreeAgency;

    /// <inheritdoc />
    protected override bool IsAdditive => false;
}