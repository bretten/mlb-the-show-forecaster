using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.RosterUpdates;

/// <summary>
/// Represents a player whose attributes changed
/// </summary>
/// <param name="ObfuscatedId">The ID of the <see cref="MlbCardDto"/> associated with the player</param>
/// <param name="Name">The player's name</param>
/// <param name="Team">The player's team</param>
/// <param name="Item"><see cref="MlbCardDto"/></param>
/// <param name="CurrentRank">The new rank of the <see cref="MlbCardDto"/></param>
/// <param name="CurrentRarity">The new rarity of the <see cref="MlbCardDto"/></param>
/// <param name="OldRank">The old rank of the <see cref="MlbCardDto"/></param>
/// <param name="OldRarity">The old rarity of the <see cref="MlbCardDto"/></param>
/// <param name="Changes">A collection of attribute changes</param>
public sealed record PlayerAttributeChangeDto(
    ObfuscatedIdDto ObfuscatedId,
    string Name,
    string Team,
    [property: JsonPropertyName("item")]
    MlbCardDto Item,
    [property: JsonPropertyName("current_rank")]
    int CurrentRank,
    [property: JsonPropertyName("current_rarity")]
    string CurrentRarity,
    [property: JsonPropertyName("old_rank")]
    int OldRank,
    [property: JsonPropertyName("old_rarity")]
    string OldRarity,
    [property: JsonPropertyName("changes")]
    IEnumerable<AttributeChangeDto> Changes
) : PlayerChangeDto(ObfuscatedId, Name, Team);