using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Web;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Fakes;

/// <summary>
/// Writes responses to the file system
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class ResponseWriterDelegatingHandler : DelegatingHandler
{
    /// <summary>
    /// Path for writing individual stats
    /// </summary>
    /// <param name="season">The season</param>
    /// <param name="mlbId">MLB ID of a player</param>
    private static string StatsPath(string season, string mlbId) =>
        Path.Combine("temp", "stats", season, $"{mlbId}.json");

    /// <summary>
    /// Path for writing all players
    /// </summary>
    /// <param name="season">The season</param>
    private static string PlayersPath(string season) => Path.Combine("temp", "players", season, "all_players.json");

    /// <summary>
    /// Used to match the stats URL and the ID of the player
    /// </summary>
    private const string StatsPattern = @"people/(\d+).*season=(\d+)";

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var requestUri = Uri.UnescapeDataString(request.RequestUri!.ToString());

        // Save the response based on which type of request
        if (requestUri.Contains("hydrate=stats")) // Stats request
        {
            var match = Regex.Match(requestUri, StatsPattern);
            var id = match.Success
                ? match.Groups[1].Value
                : throw new ArgumentException($"{nameof(ResponseWriterDelegatingHandler)} no stat ID");
            var season = match.Success
                ? match.Groups[2].Value
                : throw new ArgumentException($"{nameof(ResponseWriterDelegatingHandler)} no stat season");

            if (SelectStatIds.Contains(int.Parse(id)) || SelectStatIds.Count == 0)
            {
                Write(StatsPath(season, id), content);
            }
        }
        else if (requestUri.Contains("/v1/sports/1/players")) // Players by season request
        {
            var season = GetQueryParam(request.RequestUri, "season");
            Write(PlayersPath(season), FilterPlayers(content));
        }

        return response;
    }

    /// <summary>
    /// Filters players from the response and re-serializes as a response
    /// </summary>
    private static string FilterPlayers(string content)
    {
        using var jDoc = JsonDocument.Parse(content);
        var people = jDoc.RootElement.GetProperty("people");
        var filteredPlayers = new JsonArray();
        foreach (var p in people.EnumerateArray())
        {
            var id = p.GetProperty("id").GetInt32();
            if (!SelectStatIds.Contains(id)) continue;
            filteredPlayers.Add(p);
        }

        return new JsonObject()
        {
            ["people"] = filteredPlayers
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
    /// Which IDs will be saved
    /// </summary>
    private static readonly List<int> SelectStatIds = new List<int>()
    {
    };
}