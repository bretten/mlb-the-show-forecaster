using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

/// <summary>
/// Represents a rating and attribute change for a player in MLB The Show
/// </summary>
/// <param name="Date">The date of the change</param>
/// <param name="CardExternalId">The ID of the player card</param>
/// <param name="NewRating">The player's new rating</param>
/// <param name="NewRarity">The player card's new rarity</param>
/// <param name="OldRating">The player's previous rating</param>
/// <param name="OldRarity">The player card's previous rarity</param>
/// <param name="AttributeChanges">Changes to the player's attributes</param>
public readonly record struct PlayerRatingChange(
    DateOnly Date,
    CardExternalId CardExternalId,
    OverallRating NewRating,
    Rarity NewRarity,
    OverallRating OldRating,
    Rarity OldRarity,
    MlbPlayerAttributeChanges AttributeChanges
)
{
    /// <summary>
    /// True if the overall player rating improved, otherwise false
    /// </summary>
    public bool Improved => NewRating.Value > OldRating.Value;

    /// <summary>
    /// Returns true if this rating change has already been applied to the specified <see cref="PlayerCard"/>
    /// </summary>
    /// <param name="playerCard">The <see cref="PlayerCard"/> to check</param>
    /// <returns>True if this rating change has already been applied to the specified <see cref="PlayerCard"/></returns>
    public bool IsApplied(PlayerCard playerCard) => playerCard.IsRatingAppliedFor(Date);
};