using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Responses;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Fakes;

/// <summary>
/// Fake <see cref="IMlbApi"/>
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class FakeMlbApi : IMlbApi
{
    /// <inheritdoc />
    public Task<GetPlayersBySeasonResponse> GetPlayersBySeason(GetPlayersBySeasonRequest request)
    {
        var json = File.ReadAllText(Paths.SeasonPlayers(Paths.Fakes, request.Season.ToString()));
        var response = JsonSerializer.Deserialize<GetPlayersBySeasonResponse>(json)!;
        return Task.FromResult(response);
    }

    /// <inheritdoc />
    public Task<GetPlayerSeasonStatsByGameResponse> GetPlayerSeasonStatsByGameInternal(
        GetPlayerSeasonStatsByGameRequest request)
    {
        var json = File.ReadAllText(Paths.PlayerStats(Paths.Fakes, request.Season.ToString(),
            request.PlayerMlbId.ToString()));
        var response = JsonSerializer.Deserialize<GetPlayerSeasonStatsByGameResponse>(json)!;
        return Task.FromResult(response);
    }
}