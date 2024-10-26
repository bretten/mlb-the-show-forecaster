using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

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
    /// <param name="mlbId">MLB ID of a player</param>
    private static string StatsPath(string mlbId) => Path.Combine("temp", "stats", mlbId, ".json");

    /// <summary>
    /// Path for writing all players
    /// </summary>
    private static readonly string PlayersPath = Path.Combine("temp", "players", "all_players.json");

    /// <summary>
    /// Used to match the stats URL and the ID of the player
    /// </summary>
    private const string StatsPattern = @"^https:\/\/statsapi\.mlb\.com\/api\/v1\/people\/(\d+)\?hydrate=stats.*";

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var requestUri = request.RequestUri!.ToString();

        // Save the response based on which type of request
        if (requestUri.Contains("hydrate=stats")) // Stats request
        {
            var match = Regex.Match(requestUri, StatsPattern);
            var id = match.Success
                ? match.Groups[1].Value
                : throw new ArgumentException($"{nameof(ResponseWriterDelegatingHandler)} no stat ID");

            if (SelectStatIds.Contains(int.Parse(id)) || SelectStatIds.Count == 0)
            {
                Write(StatsPath(id), content);
            }
        }
        else if (requestUri.Contains("/v1/sports/1/players")) // Players by season request
        {
            Write(PlayersPath, FilterPlayers(content));
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
    /// Which IDs will be saved
    /// </summary>
    private static readonly List<int> SelectStatIds = new List<int>()
    {
    };
}