using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
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
    /// Path for writing individual cards
    /// </summary>
    /// <param name="id">ID of the card</param>
    private static string CardPath(string id) => Path.Combine("temp", "cards", "individual", $"{id}.json");

    /// <summary>
    /// Path for writing paged cards
    /// </summary>
    /// <param name="page">The page of cards</param>
    private static string PagedCardsPath(string page) => Path.Combine("temp", "cards", "pages", $"{page}.json");

    /// <summary>
    /// Path for selected cards
    /// </summary>
    private static readonly string SelectedCardsPath = Path.Combine("temp", "cards", "selected", "selected_cards.json");

    /// <summary>
    /// Path for writing roster updates
    /// </summary>
    /// <param name="id">The roster update ID</param>
    private static string RosterUpdate(string id) => Path.Combine("temp", "roster_updates", "individual", $"{id}.json");

    /// <summary>
    /// Path for writing the roster update list
    /// </summary>
    private static readonly string RosterUpdateList = Path.Combine("temp", "roster_updates", "list", "list.json");

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

        // Save the response based on which type of request
        if (requestUri.Contains("/apis/item.json")) // Request an individual card
        {
            var id = GetQueryParam(request.RequestUri, "uuid");

            Write(CardPath(id), content);
        }
        else if (requestUri.Contains("/apis/items.json")) // Request all cards by page
        {
            // Gather the selected cards and write them when on the last page
            CollectAndWriteSelectedCards(content);

            // Write the page's cards
            var page = GetQueryParam(request.RequestUri, "page");
            Write(PagedCardsPath(page), content);
        }
        else if (requestUri.Contains("/apis/roster_updates.json")) // All roster updates
        {
            Write(RosterUpdateList, content);
        }
        else if (requestUri.Contains("/apis/roster_update.json?id")) // Single roster update details
        {
            var id = GetQueryParam(request.RequestUri, "id");

            Write(RosterUpdate(id), content);
        }

        return response;
    }

    /// <summary>
    /// Writes the selected cards
    /// </summary>
    /// <param name="content">Content for a page of cards</param>
    private static void CollectAndWriteSelectedCards(string content)
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
            _cardsToWrite.Add(item);
        }

        if (page != totalPages) return;

        Write(SelectedCardsPath, new JsonObject()
        {
            ["page"] = 1,
            ["per_page"] = _cardsToWrite.Count,
            ["total_pages"] = 1,
            ["items"] = _cardsToWrite
        }.ToJsonString());
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