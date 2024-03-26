using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

/// <summary>
/// Represents a rating and attribute change for a player in MLB The Show
/// </summary>
/// <param name="CardExternalId">The ID of the player card</param>
/// <param name="NewRating">The player's new rating</param>
/// <param name="NewRarity">The player card's new rarity</param>
/// <param name="OldRating">The player's previous rating</param>
/// <param name="OldRarity">The player card's previous rarity</param>
/// <param name="AttributeChanges">A collection of changes to the player's attributes</param>
public readonly record struct PlayerRatingChange(
    CardExternalId CardExternalId,
    OverallRating NewRating,
    Rarity NewRarity,
    OverallRating OldRating,
    Rarity OldRarity,
    IReadOnlyCollection<AttributeChange> AttributeChanges
)
{
    /// <summary>
    /// True if the overall player rating improved, otherwise false
    /// </summary>
    public bool Improved => NewRating.Value > OldRating.Value;
};