using System.Diagnostics.CodeAnalysis;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Fakes;

/// <summary>
/// Options for <see cref="FakeMlbApi"/>
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class FakeMlbApiOptions
{
    /// <summary>
    /// Filters out all players aside from the MLB IDs in this list
    /// </summary>
    public int[]? PlayerFilter { get; init; }

    /// <summary>
    /// True to fake the API responses using local files, otherwise will use the fallback API <see cref="BaseAddress"/>
    /// </summary>
    public bool UseLocalFiles { get; init; }

    /// <summary>
    /// Address of the fallback API, which can be a mock server
    /// </summary>
    public string BaseAddress { get; init; } = null!;

    /// <summary>
    /// Dates that determine which data is returned from the fake API on consecutive requests. An empty or null value
    /// means all data is returned.
    ///
    /// A value of ["2024-03-28", "2024-08-28"] means:
    /// - 1st request => data from the beginning of the season to 2024-03-28 is returned
    /// - 2nd request => data from the beginning of the season to 2024-08-28 is returned
    /// - 3rd request => all data for the season is returned
    /// </summary>
    public DateOnly[]? SnapshotDates { get; init; }
}