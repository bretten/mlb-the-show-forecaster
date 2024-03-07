using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Converters;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;

/// <summary>
/// Represents a collection of stats by games
/// </summary>
/// <param name="Group">The type of stat: hitting, pitching, or fielding</param>
/// <param name="Splits">The stats by games</param>
[JsonConverter(typeof(StatJsonConverter))]
public readonly record struct StatsDto(
    [property: JsonPropertyName("group")]
    StatsGroupDto Group,
    [property: JsonPropertyName("splits")]
    IEnumerable<GameStatsDto> Splits
)
{
    /// <summary>
    /// A collection of hitting stats by games
    /// </summary>
    /// <param name="splits">Hitting stats by games</param>
    /// <returns>A collection of hitting stats by games</returns>
    public static StatsDto HittingStats(IEnumerable<GameHittingStatsDto>? splits = null) =>
        new(StatsGroupDto.HittingStatGroup, splits ?? new List<GameHittingStatsDto>());

    /// <summary>
    /// A collection of hitting stats by games
    /// </summary>
    /// <param name="splits">Hitting stats by games</param>
    /// <returns>A collection of hitting stats by games</returns>
    public static StatsDto HittingStats(params GameHittingStatsDto[] splits) => HittingStats(splits.AsEnumerable());

    /// <summary>
    /// A collection of pitching stats by games
    /// </summary>
    /// <param name="splits">Pitching stats by games</param>
    /// <returns>A collection of pitching stats by games</returns>
    public static StatsDto PitchingStats(IEnumerable<GamePitchingStatsDto>? splits = null) =>
        new(StatsGroupDto.PitchingStatGroup, splits ?? new List<GamePitchingStatsDto>());

    /// <summary>
    /// A collection of pitching stats by games
    /// </summary>
    /// <param name="splits">Pitching stats by games</param>
    /// <returns>A collection of pitching stats by games</returns>
    public static StatsDto PitchingStats(params GamePitchingStatsDto[] splits) => PitchingStats(splits.AsEnumerable());

    /// <summary>
    /// A collection of fielding stats by games
    /// </summary>
    /// <param name="splits">Fielding stats by games</param>
    /// <returns>A collection of fielding stats by games</returns>
    public static StatsDto FieldingStats(IEnumerable<GameFieldingStatsDto>? splits = null) =>
        new(StatsGroupDto.FieldingStatGroup, splits ?? new List<GameFieldingStatsDto>());

    /// <summary>
    /// A collection of fielding stats by games
    /// </summary>
    /// <param name="splits">Fielding stats by games</param>
    /// <returns>A collection of fielding stats by games</returns>
    public static StatsDto FieldingStats(params GameFieldingStatsDto[] splits) => FieldingStats(splits.AsEnumerable());
};