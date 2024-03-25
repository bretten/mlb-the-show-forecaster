using Refit;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.RosterUpdates;

/// <summary>
/// Request for getting a Roster Update
/// </summary>
/// <param name="Id">The ID of the Roster Update</param>
public sealed record GetRosterUpdateRequest(
    [property: AliasAs("id")]
    int Id
);