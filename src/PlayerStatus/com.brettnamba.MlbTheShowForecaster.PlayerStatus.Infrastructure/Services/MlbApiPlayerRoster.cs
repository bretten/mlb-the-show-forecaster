using com.brettnamba.MlbTheShowForecaster.Common.Application.Mapping;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Services;

public sealed class MlbApiPlayerRoster : IPlayerRoster
{
    private readonly IMlbApi _mlbApi;
    private readonly IObjectMapper _objectMapper;

    public MlbApiPlayerRoster(IMlbApi mlbApi, IObjectMapper objectMapper)
    {
        _mlbApi = mlbApi;
        _objectMapper = objectMapper;
    }

    public async Task<IEnumerable<RosterEntry>> GetRosterEntries(int seasonYear,
        CancellationToken cancellationToken = default)
    {
        var response =
            await _mlbApi.GetPlayersBySeason(new GetPlayersBySeasonRequest(seasonYear, GameType.RegularSeason));

        if (response.Players == null || !response.Players.Any())
        {
            throw new EmptyRosterException($"{GetType().Name} - MLB API roster had no players for season {seasonYear}");
        }

        return response.Players.Select(x => _objectMapper.Map<Player, RosterEntry>(x)).ToList();
    }
}