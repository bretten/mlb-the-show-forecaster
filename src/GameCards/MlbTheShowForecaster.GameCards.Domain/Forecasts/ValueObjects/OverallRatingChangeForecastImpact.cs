using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums.Extensions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;

/// <summary>
/// Represents the impact of a player rating change on a <see cref="PlayerCardForecast"/>
/// </summary>
/// <param name="oldRating">The player's old rating</param>
/// <param name="newRating">The player's new rating</param>
/// <param name="endDate"><inheritdoc /></param>
public sealed class OverallRatingChangeForecastImpact(
    OverallRating oldRating,
    OverallRating newRating,
    DateOnly endDate)
    : ForecastImpact(endDate)
{
    /// <summary>
    /// The player's old rating
    /// </summary>
    public OverallRating OldRating { get; } = oldRating;

    /// <summary>
    /// The player's new rating
    /// </summary>
    public OverallRating NewRating { get; } = newRating;

    /// <summary>
    /// True if the <see cref="NewRating"/>'s <see cref="Rarity"/> is better than the <see cref="OldRating"/>
    /// </summary>
    public bool RarityImproved => NewRating.Rarity.GreaterThan(OldRating.Rarity);

    /// <summary>
    /// True if the <see cref="NewRating"/>'s <see cref="Rarity"/> is worse than the <see cref="OldRating"/>
    /// </summary>
    public bool RarityDeclined => NewRating.Rarity.LessThan(OldRating.Rarity);

    /// <inheritdoc />
    protected override int ImpactCoefficient => ImpactConstants.Coefficients.OverallRatingChange;

    /// <inheritdoc />
    protected override bool IsAdditive => RarityImproved;

    /// <summary>
    /// If the rating changed, but the <see cref="Rarity"/> did not change, then the impact is minimal
    /// </summary>
    protected override bool IsNegligible => !RarityImproved && !RarityDeclined;
}