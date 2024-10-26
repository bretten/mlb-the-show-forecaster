using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Web;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Fakes;

/// <summary>
/// Writes responses to the file system
/// </summary>
[ExcludeFromCodeCoverage]
public class ResponseWriterDelegatingHandler : DelegatingHandler
{
    /// <summary>
    /// When iterating over all the card pages, cards that have been selected by <see cref="CardIdsToWrite"/> will be
    /// stored here
    /// </summary>
    private static JsonArray _cardsToWrite = new JsonArray();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="innerHandler"><see cref="HttpMessageHandler"/></param>
    public ResponseWriterDelegatingHandler(HttpMessageHandler innerHandler) : base(innerHandler)
    {
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
        var season = DateOnly.ParseExact(seasonShort, "yy").ToString("yyyy");

        // Save the response based on which type of request
        if (requestUri.Contains("/apis/item.json")) // Request an individual card
        {
            var id = GetQueryParam(request.RequestUri, "uuid");

            Write(Paths.Card(Paths.Temp, season, id), content);
        }
        else if (requestUri.Contains("/apis/items.json")) // Request all cards by page
        {
            // Gather the selected cards and write them when on the last page
            CollectAndWriteSelectedCards(content, season);

            // Write the page's cards
            var page = GetQueryParam(request.RequestUri, "page");
            Write(Paths.PagedCards(Paths.Temp, season, page), content);
        }
        else if (requestUri.Contains("/apis/roster_updates.json")) // All roster updates
        {
            Write(Paths.RosterUpdateList(Paths.Temp, season), content);
        }
        else if (requestUri.Contains("/apis/roster_update.json?id")) // Single roster update details
        {
            var id = GetQueryParam(request.RequestUri, "id");

            // If the cards are being filtered, just save roster updates for corresponding players
            var updateContent = CardIdsToWrite.Count == 0 ? content : FilterRosterUpdate(content);

            Write(Paths.RosterUpdate(Paths.Temp, season, id), updateContent);
        }

        return response;
    }

    /// <summary>
    /// Writes the selected cards
    /// </summary>
    /// <param name="content">Content for a page of cards</param>
    /// <param name="season">The season</param>
    private static void CollectAndWriteSelectedCards(string content, string season)
    {
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
            if (!CardIdsToWrite.Contains(id)) continue;
            // Clone the item since the JsonDocument will be disposed of
            var itemJson = item.GetRawText();
            _cardsToWrite.Add(JsonSerializer.Deserialize<JsonElement>(itemJson));
            // Write the individual item
            Write(Paths.Card(Paths.Temp, season, id), itemJson);
        }

        if (page != totalPages || CardIdsToWrite.Count == 0) return;

        Write(Paths.SelectedCards(Paths.Temp, season), new JsonObject()
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
    /// Filters the roster update to have only players in <see cref="CardIdsToWrite"/>
    /// </summary>
    private static string FilterRosterUpdate(string content)
    {
        using var jDoc = JsonDocument.Parse(content);
        var changes = jDoc.RootElement.GetProperty("attribute_changes");
        var filteredChanges = new JsonArray();
        foreach (var p in changes.EnumerateArray())
        {
            var id = p.GetProperty("obfuscated_id").GetString()!;
            if (!CardIdsToWrite.Contains(id)) continue;
            filteredChanges.Add(p);
        }

        return new JsonObject()
        {
            ["attribute_changes"] = filteredChanges,
            ["position_changes"] = new JsonArray(),
            ["newly_added"] = new JsonArray()
        }.ToJsonString(new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
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

    /// <summary>
    /// Cards IDs (<see cref="MlbCardDto.Uuid"/>) that will be written by <see cref="CollectAndWriteSelectedCards"/>
    /// </summary>
    private static readonly List<string> CardIdsToWrite = new List<string>()
    {
    };
}