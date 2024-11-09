using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Web;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Fakes;

/// <summary>
/// Writes responses to the file system
/// </summary>
[ExcludeFromCodeCoverage]
public class ResponseWriterDelegatingHandler : DelegatingHandler
{
    /// <summary>
    /// Options
    /// </summary>
    private readonly FakeMlbTheShowApiOptions _options;

    /// <summary>
    /// When iterating over all the card pages, cards that have been selected by <see cref="CardIdsToWrite"/> will be
    /// stored here
    /// </summary>
    private static JsonArray _cardsToWrite = new JsonArray();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="innerHandler"><see cref="HttpMessageHandler"/></param>
    /// <param name="options">Options</param>
    public ResponseWriterDelegatingHandler(HttpMessageHandler innerHandler, FakeMlbTheShowApiOptions options) :
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
        var seasonStr = season.ToString("yyyy");

        // Save the response based on which type of request
        if (requestUri.Contains("/apis/item.json")) // Request an individual card
        {
            var id = GetQueryParam(request.RequestUri, "uuid");

            Write(Paths.Card(Paths.Temp, seasonStr, id), content);
        }
        else if (requestUri.Contains("/apis/items.json")) // Request all cards by page
        {
            // Gather the selected cards and write them when on the last page
            CollectAndWriteSelectedCards(content, season);

            // Write the page's cards
            var page = GetQueryParam(request.RequestUri, "page");
            Write(Paths.PagedCards(Paths.Temp, seasonStr, page), content);
        }
        else if (requestUri.Contains("/apis/roster_updates.json")) // All roster updates
        {
            Write(Paths.RosterUpdateList(Paths.Temp, seasonStr), content);
        }
        else if (requestUri.Contains("/apis/roster_update.json?id")) // Single roster update details
        {
            var id = GetQueryParam(request.RequestUri, "id");

            // If the cards are being filtered, just save roster updates for corresponding players
            var updateContent = _options.PlayerCardFilter != null && _options.PlayerCardFilterFor(season).Length == 0
                ? content
                : Filters.FilterRosterUpdate(content, _options.PlayerCardFilterFor(season));

            Write(Paths.RosterUpdate(Paths.Temp, seasonStr, id), updateContent);
        }

        return response;
    }

    /// <summary>
    /// Writes the selected cards
    /// </summary>
    /// <param name="content">Content for a page of cards</param>
    /// <param name="season">The season</param>
    private void CollectAndWriteSelectedCards(string content, int season)
    {
        if (_options.PlayerCardFilter == null)
        {
            return;
        }

        using var jDoc = JsonDocument.Parse(content);
        var page = jDoc.RootElement.GetProperty("page").GetInt32();
        var totalPages = jDoc.RootElement.GetProperty("total_pages").GetInt32();
        if (page == 1)
        {
            // First page, so reset the cards to write
            _cardsToWrite = new JsonArray();
        }

        var items = jDoc.RootElement.GetProperty("items");
        foreach (var item in items.EnumerateArray())
        {
            var id = item.GetProperty("uuid").GetString()!;
            if (!_options.PlayerCardFilterFor(season).Contains(id)) continue;
            // Clone the item since the JsonDocument will be disposed of
            var itemJson = item.GetRawText();
            _cardsToWrite.Add(JsonSerializer.Deserialize<JsonElement>(itemJson));
            // Write the individual item
            Write(Paths.Card(Paths.Temp, season.ToString(), id), itemJson);
        }

        if (page != totalPages || _options.PlayerCardFilterFor(season).Length == 0) return;

        Write(Paths.SelectedCards(Paths.Temp, season.ToString()), new JsonObject()
        {
            ["page"] = 1,
            ["per_page"] = _cardsToWrite.Count,
            ["total_pages"] = 1,
            ["items"] = _cardsToWrite
        }.ToJsonString(new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        }));
    }

    /// <summary>
    /// Writes the content to the path
    /// </summary>
    private static void Write(string path, string content)
    {
        var dirName = Path.GetDirectoryName(path)!;
        Directory.CreateDirectory(dirName);
        File.WriteAllText(path, content, Encoding.UTF8);
    }

    /// <summary>
    /// Extracts the query param value
    /// </summary>
    private static string GetQueryParam(Uri uri, string paramName)
    {
        var queryParams = HttpUtility.ParseQueryString(uri.Query);
        return queryParams.Get(paramName)!;
    }
}