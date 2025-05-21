using System.Diagnostics.CodeAnalysis;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Fakes;

/// <summary>
/// Paths for fake data
/// </summary>
[ExcludeFromCodeCoverage]
public static class Paths
{
    /// <summary>
    /// Root dir for reading fake data
    /// </summary>
    public const string Fakes = "mlb_api_fakes";

    /// <summary>
    /// Root dir for writing fake data
    /// </summary>
    public const string Temp = "temp";

    /// <summary>
    /// Path for writing individual player stats
    /// </summary>
    /// <param name="root">The root path</param>
    /// <param name="season">The season</param>
    /// <param name="mlbId">MLB ID of a player</param>
    public static string PlayerStats(string root, string season, string mlbId) =>
        Path.Combine(root, "stats", season, $"{mlbId}.json");

    /// <summary>
    /// Path for writing all players
    /// </summary>
    /// <param name="root">The root path</param>
    /// <param name="season">The season</param>
    public static string SeasonPlayers(string root, string season) =>
        Path.Combine(root, "players", season, "all_players.json");

    /// <summary>
    /// Path for writing individual player roster entries
    /// </summary>
    /// <param name="root">The root path</param>
    /// <param name="mlbId">MLB ID of a player</param>
    public static string PlayerRosterEntries(string root, string mlbId) =>
        Path.Combine(root, "roster_entries", $"{mlbId}.json");
}