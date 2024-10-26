using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Fakes;

/// <summary>
/// Fake <see cref="IMlbTheShowApiFactory"/>
/// </summary>
public sealed class FakeMlbTheShowApiFactory : IMlbTheShowApiFactory
{
    /// <inheritdoc />
    public IMlbTheShowApi GetClient(Year year)
    {
        return new FakeMlbTheShowApi(year);
    }
}