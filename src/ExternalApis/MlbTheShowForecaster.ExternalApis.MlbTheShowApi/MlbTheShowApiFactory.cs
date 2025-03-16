using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Exceptions;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi;

/// <summary>
/// <see cref="IMlbTheShowApi"/> client factory
/// </summary>
public sealed class MlbTheShowApiFactory : IMlbTheShowApiFactory
{
    /// <summary>
    /// Gets a <see cref="IMlbTheShowApi"/> client for the specified year
    /// </summary>
    /// <param name="year">The specified year</param>
    /// <returns><see cref="IMlbTheShowApi"/> for the specified year</returns>
    /// <exception cref="UnsupportedMlbTheShowYearException">Thrown when the specified year is not supported by MLB The Show</exception>
    public IMlbTheShowApi GetClient(Year year)
    {
        return year switch
        {
            Year.Season2021 => GetClient(Constants.BaseUrl2021),
            Year.Season2022 => GetClient(Constants.BaseUrl2022),
            Year.Season2023 => GetClient(Constants.BaseUrl2023),
            Year.Season2024 => GetClient(Constants.BaseUrl2024),
            Year.Season2025 => GetClient(Constants.BaseUrl2025),
            _ => throw new UnsupportedMlbTheShowYearException($"MLB The Show does not support the year {year}")
        };
    }

    /// <summary>
    /// Creates the <see cref="IMlbTheShowApi"/> client for the specified base URL
    /// </summary>
    /// <param name="baseUrl">The base URL</param>
    /// <returns><see cref="IMlbTheShowApi"/> with the specified base URL</returns>
    private static IMlbTheShowApi GetClient(string baseUrl)
    {
        return RestService.For<IMlbTheShowApi>(Resiliency.ResilientClient(baseUrl),
            new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(
                    new JsonSerializerOptions()
                    {
                        Converters = { new JsonStringEnumConverter() }
                    }
                )
            }
        );
    }
}