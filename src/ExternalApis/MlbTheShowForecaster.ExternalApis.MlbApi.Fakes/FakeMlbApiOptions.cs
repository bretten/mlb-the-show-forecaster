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
}