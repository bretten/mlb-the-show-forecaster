using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.RosterUpdates;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.RosterUpdates;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.RosterUpdates;

/// <summary>
/// Response for a <see cref="GetRosterUpdateRequest"/>
/// </summary>
/// <param name="PlayerAttributeChanges">A collection of <see cref="PlayerAttributeChanges"/></param>
/// <param name="PlayerPositionChanges">A collection of <see cref="PlayerPositionChanges"/></param>
/// <param name="NewlyAddedPlayers">A collection of <see cref="NewlyAddedPlayers"/></param>
public sealed record GetRosterUpdateResponse(
    [property: JsonPropertyName("attribute_changes")]
    IEnumerable<PlayerAttributeChangeDto> PlayerAttributeChanges,
    [property: JsonPropertyName("position_changes")]
    IEnumerable<PlayerPositionChangeDto> PlayerPositionChanges,
    [property: JsonPropertyName("newly_added")]
    IEnumerable<NewlyAddedPlayerDto> NewlyAddedPlayers
);