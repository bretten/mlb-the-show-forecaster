using System.Diagnostics.CodeAnalysis;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Fakes;

/// <summary>
/// Fake <see cref="IMlbTheShowApiFactory"/>
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class FakeMlbTheShowApiFactory : IMlbTheShowApiFactory
{
    /// <summary>
    /// Options
    /// </summary>
    private readonly FakeMlbTheShowApiOptions _options;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options">Options</param>
    public FakeMlbTheShowApiFactory(FakeMlbTheShowApiOptions options)
    {
        _options = options;
    }

    /// <inheritdoc />
    public IMlbTheShowApi GetClient(Year year)
    {
        return new FakeMlbTheShowApi(year, _options);
    }
}