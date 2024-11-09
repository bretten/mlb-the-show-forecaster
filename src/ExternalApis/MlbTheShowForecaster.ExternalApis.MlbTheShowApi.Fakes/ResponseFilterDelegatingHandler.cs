using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Fakes;

/// <summary>
/// Filters responses
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class ResponseFilterDelegatingHandler : DelegatingHandler
{
    /// <summary>
    /// Options
    /// </summary>
    private readonly FakeMlbTheShowApiOptions _options;

    /// <summary>
    /// When iterating over all the card pages, cards that have been selected by <see cref="FakeMlbTheShowApiOptions.PlayerCardFilter"/>
    /// will be stored here
    /// </summary>
    private ConcurrentDictionary<string, JsonElement> _cardsToWrite = new ConcurrentDictionary<string, JsonElement>();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="innerHandler"><inheritdoc /></param>
    /// <param name="options">Options</param>
    public ResponseFilterDelegatingHandler(HttpMessageHandler innerHandler, FakeMlbTheShowApiOptions options) :
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

        var requestUri = request.RequestUri!.ToString();
        var match = Regex.Match(requestUri, @"mlb(\d+)");
        var seasonShort = match.Success
            ? match.Groups[1].Value
            : throw new ArgumentException($"{nameof(ResponseWriterDelegatingHandler)} no MLB The Show season");
        var season = DateOnly.ParseExact(seasonShort, "yy").Year;

        // Filter the response based on which type of request
        if (requestUri.Contains("/apis/items.json")) // Request all cards by page
        {
            var responseOverride = FilterAndStoreCards(content, season);
            if (!string.IsNullOrEmpty(responseOverride))
            {
                response.Content = new StringContent(responseOverride);
            }
        }
        else if (requestUri.Contains("/apis/roster_update.json?id")) // Single roster update details
        {
            response.Content =
                new StringContent(Filters.FilterRosterUpdate(content, _options.PlayerCardFilterFor(season)));
        }

        return response;
    }

    /// <summary>
    /// Filters the cards in the response using <see cref="FakeMlbTheShowApiOptions.PlayerCardFilter"/>
    /// </summary>
    /// <param name="content">The response to filter</param>
    /// <param name="season">The season to filter</param>
    /// <returns>A response containing only the filtered items or null if they are still being requested</returns>
    private string? FilterAndStoreCards(string content, int season)
    {
        if (_options.PlayerCardFilter == null)
        {
            // There is no filter
            return null;
        }

        using var jDoc = JsonDocument.Parse(content);
        var page = jDoc.RootElement.GetProperty("page").GetInt32();
        if (page == 1)
        {
            // First page, so reset the cards to write
            _cardsToWrite = new ConcurrentDictionary<string, JsonElement>();
        }

        // If any of the cards in the response are from the filter, keep track of them
        var items = jDoc.RootElement.GetProperty("items");
        foreach (var item in items.EnumerateArray())
        {
            var id = item.GetProperty("uuid").GetString()!;
            if (!_options.PlayerCardFilterFor(season).Contains(id)) continue;
            // Clone the item since the JsonDocument will be disposed of
            var itemJson = item.GetRawText();
            _cardsToWrite.TryAdd(id, JsonSerializer.Deserialize<JsonElement>(itemJson));
        }

        // Check if we have found all the cards specified in the filter
        if (!_cardsToWrite.Keys.ToHashSet().SetEquals(_options.PlayerCardFilterFor(season)))
        {
            return null;
        }

        // Reaching this point means all cards in the filter have been found
        var filteredItems = new JsonArray();
        foreach (var item in _cardsToWrite)
        {
            filteredItems.Add(item.Value);
        }

        // Return the filtered cards as a single page of items
        return new JsonObject()
        {
            ["page"] = 1,
            ["per_page"] = _cardsToWrite.Count,
            ["total_pages"] = 1,
            ["items"] = filteredItems
        }.ToJsonString(new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}