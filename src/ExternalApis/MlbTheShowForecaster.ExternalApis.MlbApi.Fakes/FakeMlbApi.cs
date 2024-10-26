using System.Text.Json;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Responses;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Fakes;

/// <summary>
/// Fake <see cref="IMlbApi"/>
/// </summary>
public sealed class FakeMlbApi : IMlbApi
{
    /// <inheritdoc />
    public Task<GetPlayersBySeasonResponse> GetPlayersBySeason(GetPlayersBySeasonRequest request)
    {
        var json = File.ReadAllText(PlayersFor(request.Season));
        var response = JsonSerializer.Deserialize<GetPlayersBySeasonResponse>(json)!;
        return Task.FromResult(response);
    }

    /// <inheritdoc />
    public Task<GetPlayerSeasonStatsByGameResponse> GetPlayerSeasonStatsByGameInternal(
        GetPlayerSeasonStatsByGameRequest request)
    {
        var json = File.ReadAllText(StatsFor(request.Season, request.PlayerMlbId));
        var response = JsonSerializer.Deserialize<GetPlayerSeasonStatsByGameResponse>(json)!;
        return Task.FromResult(response);
    }

    /// <summary>
    /// Path to JSON file that has fake data for <see cref="GetPlayersBySeason"/>
    /// </summary>
    private static string PlayersFor(int season)
    {
        return Path.Combine("mlb_api_fakes", "players", season.ToString(), "all_players.json");
    }

    /// <summary>
    /// Path to JSON file that has fake data for <see cref="GetPlayerSeasonStatsByGameInternal"/>
    /// </summary>
    private static string StatsFor(int season, int mlbId)
    {
        return Path.Combine("mlb_api_fakes", "stats", season.ToString(), $"{mlbId}.json");
    }
}