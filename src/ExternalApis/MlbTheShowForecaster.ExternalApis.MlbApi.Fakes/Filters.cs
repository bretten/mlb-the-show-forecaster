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
}