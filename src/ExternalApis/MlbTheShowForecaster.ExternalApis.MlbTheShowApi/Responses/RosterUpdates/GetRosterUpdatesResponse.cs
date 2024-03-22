using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.RosterUpdates;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.RosterUpdates;

/// <summary>
/// Response for getting all Roster Updates
/// </summary>
/// <param name="RosterUpdates">All of the Roster Updates</param>
public sealed record GetRosterUpdatesResponse(
    [property: JsonPropertyName("roster_updates")]
    IEnumerable<RosterUpdateDto> RosterUpdates
);