using System.Diagnostics.CodeAnalysis;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Fakes;

/// <summary>
/// Paths for fake data
/// </summary>
[ExcludeFromCodeCoverage]
public static class Paths
{
    /// <summary>
    /// Root dir for reading fake data
    /// </summary>
    public const string Fakes = "mlb_the_show_api_fakes";

    /// <summary>
    /// Root dir for writing fake data
    /// </summary>
    public const string Temp = "temp";

    /// <summary>
    /// Path for writing individual cards
    /// </summary>
    /// <param name="root">The root path</param>
    /// <param name="season">The season</param>
    /// <param name="id">ID of the card</param>
    public static string Card(string root, string season, string id) =>
        Path.Combine(root, "cards", season, "individual", $"{id}.json");

    /// <summary>
    /// Path for writing paged cards
    /// </summary>
    /// <param name="root">The root path</param>
    /// <param name="season">The season</param>
    /// <param name="page">The page of cards</param>
    public static string PagedCards(string root, string season, string page) =>
        Path.Combine(root, "cards", season, "pages", $"{page}.json");

    /// <summary>
    /// Path for selected cards
    /// </summary>
    /// <param name="root">The root path</param>
    /// <param name="season">The season</param>
    public static string SelectedCards(string root, string season) =>
        Path.Combine(root, "cards", season, "selected", "selected_cards.json");

    /// <summary>
    /// Path for writing individual listings
    /// </summary>
    /// <param name="root">The root path</param>
    /// <param name="season">The season</param>
    /// <param name="id">ID of the listing's card</param>
    public static string Listing(string root, string season, string id) =>
        Path.Combine(root, "listings", season, "individual", $"{id}.json");

    /// <summary>
    /// Path for writing paged listings
    /// </summary>
    /// <param name="root">The root path</param>
    /// <param name="season">The season</param>
    /// <param name="page">The page of listings</param>
    public static string PagedListings(string root, string season, string page) =>
        Path.Combine(root, "listings", season, "pages", $"{page}.json");

    /// <summary>
    /// Path for writing roster updates
    /// </summary>
    /// <param name="root">The root path</param>
    /// <param name="id">The roster update ID</param>
    /// <param name="season">The season</param>
    public static string RosterUpdate(string root, string season, string id) =>
        Path.Combine(root, "roster_updates", season, "individual", $"{id}.json");

    /// <summary>
    /// Path for writing the roster update list
    /// </summary>
    /// <param name="root">The root path</param>
    /// <param name="season">The season</param>
    public static string RosterUpdateList(string root, string season) =>
        Path.Combine(root, "roster_updates", season, "list", "list.json");
}