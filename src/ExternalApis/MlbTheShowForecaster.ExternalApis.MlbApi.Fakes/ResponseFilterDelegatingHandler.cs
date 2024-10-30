using System.Diagnostics.CodeAnalysis;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Fakes;

/// <summary>
/// Filters responses
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class ResponseFilterDelegatingHandler : DelegatingHandler
{
    /// <summary>
    /// Options
    /// </summary>
    private readonly FakeMlbApiOptions _options;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="innerHandler"><inheritdoc /></param>
    /// <param name="options">Options</param>
    public ResponseFilterDelegatingHandler(HttpMessageHandler innerHandler, FakeMlbApiOptions options) :
        base(innerHandler)
    {
        _options = options;
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var requestUri = Uri.UnescapeDataString(request.RequestUri!.ToString());

        // Filter the response based on which type of request
        if (requestUri.Contains("/v1/sports/1/players")) // Players by season request
        {
            var filteredResponse = Filters.FilterPlayers(content, _options.PlayerFilter);
            response.Content = new StringContent(filteredResponse);
        }

        return response;
    }
}