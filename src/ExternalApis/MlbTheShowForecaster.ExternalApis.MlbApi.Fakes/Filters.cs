using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Fakes;

/// <summary>
/// Filters responses
/// </summary>
[ExcludeFromCodeCoverage]
public static class Filters
{
    /// <summary>
    /// Filters players from the response and re-serializes as a response
    /// </summary>
    public static string FilterPlayers(string content, int[]? playerMlbIds)
    {
        if (playerMlbIds == null)
        {
            return content;
        }

        using var jDoc = JsonDocument.Parse(content);
        var people = jDoc.RootElement.GetProperty("people");
        var filteredPlayers = new JsonArray();
        foreach (var p in people.EnumerateArray())
        {
            var id = p.GetProperty("id").GetInt32();
            if (!playerMlbIds.Contains(id)) continue;
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
    /// Filters the response so only stats before the specified date are included
    /// </summary>
    /// <param name="content">The original response</param>
    /// <param name="date">The date</param>
    /// <returns>The filtered response</returns>
    public static string FilterStats(string content, DateOnly date)
    {
        using var jDoc = JsonDocument.Parse(content);
        // There is only one player
        var player = jDoc.RootElement.GetProperty("people").EnumerateArray().First();

        // Get the player's stats array
        var stats = player.GetProperty("stats");

        // Build a replacement stats array
        var replacementStats = new JsonArray();
        foreach (var statSet in stats.EnumerateArray())
        {
            // Each split entry represents stats for a single day
            var splits = statSet.GetProperty("splits");
            var replacementSplits = new JsonArray();
            foreach (var split in splits.EnumerateArray())
            {
                var gameDate = DateOnly.Parse(split.GetProperty("date").GetString()!);
                if (gameDate > date) continue;
                replacementSplits.Add(split);
            }

            // Replace the splits in this stat set
            var replacementStatSet = JsonNode.Parse(statSet.GetRawText())!;
            replacementStatSet["splits"] = replacementSplits;
            replacementStats.Add(replacementStatSet);
        }

        // Replace the whole stats array on the player
        var replacementPlayer = JsonNode.Parse(player.GetRawText())!;
        replacementPlayer["stats"] = replacementStats;

        return new JsonObject()
        {
            ["people"] = new JsonArray(replacementPlayer)
        }.ToJsonString(new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}