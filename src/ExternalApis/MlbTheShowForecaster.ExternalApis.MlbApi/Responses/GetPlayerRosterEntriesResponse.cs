using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.RosterEntries;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Responses;

/// <summary>
/// Response for <see cref="GetPlayerRosterEntriesRequest"/>
/// </summary>
/// <param name="Players">The players and their </param>
public sealed record GetPlayerRosterEntriesResponse(
    [property: JsonPropertyName("people")] List<PlayerRosterEntryDto>? Players
);