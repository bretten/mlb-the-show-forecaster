using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.RosterEntries;

/// <summary>
/// The status of the roster entry
/// </summary>
/// <param name="Code">A code representing the status</param>
/// <param name="Description">A human-readable description of the status</param>
public readonly record struct RosterEntryStatusDto(
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("description")]
    string Description
);