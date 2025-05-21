using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Fakes;

/// <summary>
/// Filters responses
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class ResponseFilterDelegatingHandler : DelegatingHandler
{
    /// <summary>
    /// Options
    /// </summary>
    private readonly FakeMlbApiOptions _options;

    /// <summary>
    /// Keeps track of the date snapshot that each player's ID is on
    /// </summary>
    private readonly ConcurrentDictionary<string, int> _snapshotDateProgress = new ConcurrentDictionary<string, int>();

    /// <summary>
    /// Used to match the ID of the player in the people URL
    /// </summary>
    private const string PeopleIdPattern = @"people/(\d+)";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="innerHandler"><inheritdoc /></param>
    /// <param name="options">Options</param>
    public ResponseFilterDelegatingHandler(HttpMessageHandler innerHandler, FakeMlbApiOptions options) :
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

        var requestUri = Uri.UnescapeDataString(request.RequestUri!.ToString());

        // Filter the response based on which type of request
        if (requestUri.Contains("/v1/sports/1/players")) // Players by season request
        {
            var filteredResponse = Filters.FilterPlayers(content, _options.PlayerFilter);
            response.Content = new StringContent(filteredResponse);
        }
        else if (requestUri.Contains("hydrate=stats")) // Players by season request
        {
            if (_options.SnapshotDates == null) return response;

            var match = Regex.Match(requestUri, PeopleIdPattern);
            var id = match.Success
                ? match.Groups[1].Value
                : throw new ArgumentException($"{nameof(ResponseFilterDelegatingHandler)} no people ID");
            _snapshotDateProgress.TryAdd(id, 0);

            var currentIndex = _snapshotDateProgress[id];
            response.Content = new StringContent(Filters.FilterStats(content, GetSnapshotDate(currentIndex)));
            _snapshotDateProgress[id]++;
        }
        else if (requestUri.Contains("hydrate=rosterEntries")) // Player roster entries
        {
            response.Content = new StringContent(Filters.FilterPlayers(content, _options.PlayerFilter));
        }

        return response;
    }

    /// <summary>
    /// Gets the snapshot date
    /// </summary>
    /// <param name="i">Index</param>
    /// <returns>The date corresponding to the index</returns>
    private DateOnly GetSnapshotDate(int i)
    {
        if (i >= _options.SnapshotDates!.Length)
        {
            var lastDate = _options.SnapshotDates![^1];
            return new DateOnly(lastDate.Year, 12, 31); // End of the year
        }

        return _options.SnapshotDates![i];
    }
}