using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.Retry;
using Polly.Timeout;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi;

/// <summary>
/// Provides resiliency for the client
/// </summary>
public static class Resiliency
{
    /// <summary>
    /// Returns a resilient HTTP client
    ///
    /// Delay and timeouts are battle tested against the API so don't need to be configurable
    /// </summary>
    /// <param name="baseUrl">Base URL</param>
    /// <param name="innerHandler">The inner HTTP message handler</param>
    /// <returns>Resilient HTTP client</returns>
    public static HttpClient ResilientClient(string baseUrl, HttpMessageHandler? innerHandler = null)
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
            InnerHandler = innerHandler ?? new HttpClientHandler(),
        };
        return new HttpClient(handler)
        {
            BaseAddress = new Uri(baseUrl),
            Timeout = TimeSpan.FromMinutes(25),
        };
    }
}