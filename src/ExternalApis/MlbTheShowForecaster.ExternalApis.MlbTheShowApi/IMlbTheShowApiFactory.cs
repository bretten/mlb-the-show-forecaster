using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi;

/// <summary>
/// Defines a factory for the <see cref="IMlbTheShowApi"/>
/// </summary>
public interface IMlbTheShowApiFactory
{
    /// <summary>
    /// Should get a <see cref="IMlbTheShowApi"/> client for the specified year
    /// </summary>
    /// <param name="year">The specified year</param>
    /// <returns><see cref="IMlbTheShowApi"/> for the specified year</returns>
    /// <exception cref="UnsupportedMlbTheShowYearException">Thrown when the specified year is not supported by MLB The Show</exception>
    IMlbTheShowApi GetClient(Year year);
}