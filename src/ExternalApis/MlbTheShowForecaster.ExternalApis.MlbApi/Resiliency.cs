using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.Retry;
using Polly.Timeout;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi;

/// <summary>
/// Provides resiliency for the client
/// </summary>
public static class Resiliency
{
    /// <summary>
    /// Returns a resilient HTTP message handler
    ///
    /// Delay and timeouts are battle tested against the API so don't need to be configurable
    /// </summary>
    /// <param name="innerHandler">The inner HTTP message handler</param>
    /// <returns>Resilient HTTP message handler</returns>
    public static HttpMessageHandler ResilientHandler(HttpMessageHandler? innerHandler = null)
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
        return new ResilienceHandler(pipeline)
        {
            InnerHandler = innerHandler ?? new HttpClientHandler(),
        };
    }
}