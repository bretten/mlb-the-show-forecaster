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
    public override Demand Demand
    {
        get
        {
            if (IsNegligible)
            {
                return Demand.Stable();
            }

            // A rarity upgrade means high demand, whereas a downgrade means people will try to get rid of the card
            return RarityImproved ? Demand.High() : Demand.BigLoss();
        }
    }

    /// <summary>
    /// If the rating changed, but the <see cref="Rarity"/> did not change, then the impact is minimal
    /// </summary>
    private bool IsNegligible => !RarityImproved && !RarityDeclined;
}