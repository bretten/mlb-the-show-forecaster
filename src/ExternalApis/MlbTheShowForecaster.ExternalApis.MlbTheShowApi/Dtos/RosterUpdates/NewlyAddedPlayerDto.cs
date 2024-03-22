using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.RosterUpdates;

/// <summary>
/// Represents a player that was added to MLB The Show
/// </summary>
/// <param name="ObfuscatedId">The ID of the <see cref="MlbCardDto"/> associated with the player</param>
/// <param name="Name">The player's name</param>
/// <param name="Team">The player's team</param>
/// <param name="Item"><see cref="MlbCardDto"/></param>
/// <param name="Position">The player's position</param>
/// <param name="CurrentRank">The rank of the <see cref="MlbCardDto"/></param>
/// <param name="CurrentRarity">The rarity of the <see cref="MlbCardDto"/></param>
public sealed record NewlyAddedPlayerDto(
    string ObfuscatedId,
    string Name,
    string Team,
    MlbCardDto Item,
    [property: JsonPropertyName("pos")]
    string Position,
    [property: JsonPropertyName("current_rank")]
    int CurrentRank,
    [property: JsonPropertyName("current_rarity")]
    string CurrentRarity
) : PlayerChangeDto(ObfuscatedId, Name, Team, Item);