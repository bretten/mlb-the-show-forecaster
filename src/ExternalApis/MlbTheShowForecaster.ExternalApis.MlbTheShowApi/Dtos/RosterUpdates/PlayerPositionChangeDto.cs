using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.RosterUpdates;

/// <summary>
/// Represents a player whose primary position changed
/// </summary>
/// <param name="ObfuscatedId">The ID of the <see cref="MlbCardDto"/> associated with the player</param>
/// <param name="Name">The player's name</param>
/// <param name="Team">The player's team</param>
/// <param name="Item"><see cref="MlbCardDto"/></param>
/// <param name="Position">The player's new position</param>
public sealed record PlayerPositionChangeDto(
    ObfuscatedIdDto ObfuscatedId,
    string Name,
    string Team,
    [property: JsonPropertyName("item")]
    MlbCardDto Item,
    [property: JsonPropertyName("pos")]
    string Position
) : PlayerChangeDto(ObfuscatedId, Name, Team);