using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.RosterUpdates;

/// <summary>
/// A roster update
/// </summary>
/// <param name="Id">The ID of the roster update</param>
/// <param name="Name">The name of the roster update which is a date</param>
public sealed record RosterUpdateDto(
    [property: JsonPropertyName("id")]
    int Id,
    [property: JsonPropertyName("name")]
    string Name
)
{
    /// <summary>
    /// The date of the roster update
    /// </summary>
    public DateOnly Date => DateOnly.Parse(Name);
};