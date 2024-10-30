using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Fakes;

/// <summary>
/// Filters responses
/// </summary>
[ExcludeFromCodeCoverage]
public static class Filters
{
    /// <summary>
    /// Filters the roster update to have only players in <see cref="FakeMlbTheShowApiOptions.PlayerCardFilter"/>
    /// </summary>
    public static string FilterRosterUpdate(string content, string[]? cardIds)
    {
        if (cardIds == null)
        {
            // There is no filter
            return content;
        }

        using var jDoc = JsonDocument.Parse(content);
        var changes = jDoc.RootElement.GetProperty("attribute_changes");
        var filteredChanges = new JsonArray();
        foreach (var p in changes.EnumerateArray())
        {
            var id = p.GetProperty("obfuscated_id").GetString()!;
            if (!cardIds.Contains(id)) continue;
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
}