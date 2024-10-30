using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Responses;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Fakes;

/// <summary>
/// Fake <see cref="IMlbApi"/>
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class FakeMlbApi : IMlbApi
{
    /// <summary>
    /// Options
    /// </summary>
    private readonly FakeMlbApiOptions _options;

    /// <summary>
    /// Fallback API, which can be a mock server
    /// </summary>
    private readonly IMlbApi _api;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options">Options</param>
    public FakeMlbApi(FakeMlbApiOptions options)
    {
        _options = options;
        _api = RestService.For<IMlbApi>(options.BaseAddress,
            new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(
                    new JsonSerializerOptions()
                    {
                        Converters = { new JsonStringEnumConverter() }
                    }
                ),
                HttpMessageHandlerFactory = () => new ResponseFilterDelegatingHandler(new HttpClientHandler(), options)
            }
        );
    }

    /// <inheritdoc />
    public async Task<GetPlayersBySeasonResponse> GetPlayersBySeason(GetPlayersBySeasonRequest request)
    {
        if (!_options.UseLocalFiles)
        {
            return await _api.GetPlayersBySeason(request);
        }

        var json = await File.ReadAllTextAsync(Paths.SeasonPlayers(Paths.Fakes, request.Season.ToString()));
        var response = JsonSerializer.Deserialize<GetPlayersBySeasonResponse>(json)!;
        return await Task.FromResult(response);
    }

    /// <inheritdoc />
    public async Task<GetPlayerSeasonStatsByGameResponse> GetPlayerSeasonStatsByGameInternal(
        GetPlayerSeasonStatsByGameRequest request)
    {
        if (!_options.UseLocalFiles)
        {
            return await _api.GetPlayerSeasonStatsByGame(request);
        }

        var json = await File.ReadAllTextAsync(Paths.PlayerStats(Paths.Fakes, request.Season.ToString(),
            request.PlayerMlbId.ToString()));
        var response = JsonSerializer.Deserialize<GetPlayerSeasonStatsByGameResponse>(json)!;
        return await Task.FromResult(response);
    }
}