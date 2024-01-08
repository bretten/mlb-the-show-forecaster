using com.brettnamba.MlbTheShowForecaster.Common.Application.Mapping;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Services;

/// <summary>
/// MLB API implementation of <see cref="IPlayerRoster"/>
/// </summary>
public sealed class MlbApiPlayerRoster : IPlayerRoster
{
    /// <summary>
    /// The <see cref="IMlbApi"/>
    /// </summary>
    private readonly IMlbApi _mlbApi;

    /// <summary>
    /// <see cref="IObjectMapper"/> that maps the MLB API data to the application-relevant <see cref="RosterEntry"/>
    /// </summary>
    private readonly IObjectMapper _objectMapper;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mlbApi">The <see cref="IMlbApi"/></param>
    /// <param name="objectMapper"><see cref="IObjectMapper"/> that maps the MLB API data to the application-relevant <see cref="RosterEntry"/></param>
    public MlbApiPlayerRoster(IMlbApi mlbApi, IObjectMapper objectMapper)
    {
        _mlbApi = mlbApi;
        _objectMapper = objectMapper;
    }

    /// <summary>
    /// Returns roster information on all players in the MLB for the specified season year
    /// </summary>
    /// <param name="seasonYear">The season to get roster entries for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>Roster information on all players in the MLB for the specified season year</returns>
    /// <exception cref="EmptyRosterException">Thrown if the roster is empty</exception>
    public async Task<IEnumerable<RosterEntry>> GetRosterEntries(int seasonYear,
        CancellationToken cancellationToken = default)
    {
        var response =
            await _mlbApi.GetPlayersBySeason(new GetPlayersBySeasonRequest(seasonYear, GameType.RegularSeason));

        if (response.Players == null || !response.Players.Any())
        {
            throw new EmptyRosterException($"{GetType().Name} - MLB API roster had no players for season {seasonYear}");
        }

        return response.Players.Select(x => _objectMapper.Map<PlayerDto, RosterEntry>(x)).ToList();
    }
}