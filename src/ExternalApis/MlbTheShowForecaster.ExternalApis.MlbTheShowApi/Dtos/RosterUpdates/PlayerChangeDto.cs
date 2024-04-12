using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Converters;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.RosterUpdates;

/// <summary>
/// Represents changes for a player in a Roster Update
/// </summary>
/// <param name="Uuid">The ID of the <see cref="MlbCardDto"/> associated with the player</param>
/// <param name="Name">The player's name</param>
/// <param name="Team">The player's team</param>
public abstract record PlayerChangeDto(
    [property: JsonPropertyName("obfuscated_id")]
    [property: JsonConverter(typeof(UuidJsonConverter))]
    UuidDto Uuid,
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("team")]
    string Team
);