using System.Diagnostics.CodeAnalysis;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Fakes;

/// <summary>
/// Options for <see cref="FakeMlbTheShowApi"/>
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class FakeMlbTheShowApiOptions
{
    /// <summary>
    /// Filters out all players cards aside from the IDs in this collection. The key should be a year and the value
    /// should a collection of player card IDs in the digit format (no hyphens, ToString("N"))
    /// </summary>
    public Dictionary<int, string[]>? PlayerCardFilter { get; init; }

    /// <summary>
    /// True to fake the API responses using local files, otherwise will use the fallback API <see cref="BaseAddress"/>
    /// </summary>
    public bool UseLocalFiles { get; init; }

    /// <summary>
    /// Address of the fallback API, which can be a mock server
    /// </summary>
    public string BaseAddress { get; init; } = null!;

    /// <summary>
    /// Gets the player card filter for the specified year
    /// </summary>
    /// <param name="year">The year</param>
    /// <returns>The player card filter</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the filter does not exist for the specified year</exception>
    public string[] PlayerCardFilterFor(int year) => PlayerCardFilter?.ContainsKey(year) ?? false
        ? PlayerCardFilter[year]
        : throw new KeyNotFoundException($"No player card filter for season {year}");
}