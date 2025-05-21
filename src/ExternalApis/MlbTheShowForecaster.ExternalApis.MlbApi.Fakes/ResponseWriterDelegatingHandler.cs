using System.Diagnostics.CodeAnalysis;
using System.Text;
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
    /// Options
    /// </summary>
    private readonly FakeMlbApiOptions _options;

    /// <summary>
    /// Used to match the ID of the player in the people URL
    /// </summary>
    private const string PeopleIdPattern = @"people/(\d+)";

    /// <summary>
    /// Used to match the stats URL and the ID of the player
    /// </summary>
    private const string StatsPattern = @"people/(\d+).*season=(\d+)";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="innerHandler"><inheritdoc /></param>
    /// <param name="options">Options</param>
    public ResponseWriterDelegatingHandler(HttpMessageHandler innerHandler, FakeMlbApiOptions options) :
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

        // Save the response based on which type of request
        if (requestUri.Contains("hydrate=stats")) // Stats request
        {
            var match = Regex.Match(requestUri, StatsPattern);
            var id = match.Success
                ? match.Groups[1].Value
                : throw new ArgumentException($"{nameof(ResponseWriterDelegatingHandler)} no people ID");
            var season = match.Success
                ? match.Groups[2].Value
                : throw new ArgumentException($"{nameof(ResponseWriterDelegatingHandler)} no stat season");

            if (_options.PlayerFilter == null || _options.PlayerFilter.Contains(int.Parse(id)))
            {
                Write(Paths.PlayerStats(Paths.Temp, season, id), content);
            }
        }
        else if (requestUri.Contains("/v1/sports/1/players")) // Players by season request
        {
            var season = GetQueryParam(request.RequestUri, "season");
            Write(Paths.SeasonPlayers(Paths.Temp, season), Filters.FilterPlayers(content, _options.PlayerFilter));
        }
        else if (requestUri.Contains("hydrate=rosterEntries")) // Player roster entries
        {
            var match = Regex.Match(requestUri, PeopleIdPattern);
            var id = match.Success
                ? match.Groups[1].Value
                : throw new ArgumentException($"{nameof(ResponseWriterDelegatingHandler)} no people ID");
            if (_options.PlayerFilter == null || _options.PlayerFilter.Contains(int.Parse(id)))
            {
                Write(Paths.PlayerRosterEntries(Paths.Temp, id), content);
            }
        }

        return response;
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
}