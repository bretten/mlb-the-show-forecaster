using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

/// <summary>
/// Represents a stat group
/// </summary>
/// <param name="DisplayName">The name of the stat group: hitting, pitching, or fielding</param>
public readonly record struct StatsGroupDto(
    [property: JsonPropertyName("displayName")]
    string DisplayName
)
{
    /// <summary>
    /// Hitting stat group
    /// </summary>
    public static StatsGroupDto HittingStatGroup { get; } = new(Constants.Parameters.Hitting);

    /// <summary>
    /// Pitching stat group
    /// </summary>
    public static StatsGroupDto PitchingStatGroup { get; } = new(Constants.Parameters.Pitching);

    /// <summary>
    /// Fielding stat group
    /// </summary>
    public static StatsGroupDto FieldingStatGroup { get; } = new(Constants.Parameters.Fielding);
};