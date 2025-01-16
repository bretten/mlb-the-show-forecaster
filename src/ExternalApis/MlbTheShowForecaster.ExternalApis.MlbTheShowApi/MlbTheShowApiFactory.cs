using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Exceptions;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.Retry;
using Polly.Timeout;
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
        var pipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
            {
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                MaxRetryAttempts = 4,
                Delay = TimeSpan.FromMinutes(5),
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<TimeoutRejectedException>()
                    .Handle<HttpRequestException>()
                    .HandleResult(response => !response.IsSuccessStatusCode)
            })
            .AddTimeout(TimeSpan.FromMinutes(20))
            .Build();
        var handler = new ResilienceHandler(pipeline)
        {
            InnerHandler = new HttpClientHandler(),
        };
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(baseUrl),
            Timeout = TimeSpan.FromMinutes(25),
        };

        return RestService.For<IMlbTheShowApi>(httpClient,
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