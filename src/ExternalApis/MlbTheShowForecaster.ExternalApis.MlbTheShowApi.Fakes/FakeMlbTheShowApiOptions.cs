using System.Diagnostics.CodeAnalysis;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Fakes;

/// <summary>
/// Options for <see cref="FakeMlbTheShowApi"/>
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class FakeMlbTheShowApiOptions
{
    /// <summary>
    /// Filters out all players cards aside from the IDs in this collection. IDs should be in the digit format (no hyphens, ToString("N"))
    /// </summary>
    public string[]? PlayerCardFilter { get; init; }

    /// <summary>
    /// True to fake the API responses using local files, otherwise will use the fallback API <see cref="BaseAddress"/>
    /// </summary>
    public bool UseLocalFiles { get; init; }

    /// <summary>
    /// Address of the fallback API, which can be a mock server
    /// </summary>
    public string BaseAddress { get; init; } = null!;
}