using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Requests;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Exceptions;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Mapping;

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
    /// Maps the MLB API data to the application-level <see cref="RosterEntry"/>
    /// </summary>
    private readonly IMlbApiPlayerMapper _playerMapper;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="mlbApi">The <see cref="IMlbApi"/></param>
    /// <param name="playerMapper">Maps the MLB API data to the application-level <see cref="RosterEntry"/></param>
    public MlbApiPlayerRoster(IMlbApi mlbApi, IMlbApiPlayerMapper playerMapper)
    {
        _mlbApi = mlbApi;
        _playerMapper = playerMapper;
    }

    /// <summary>
    /// Returns roster information on all players in the MLB for the specified season year
    /// </summary>
    /// <param name="seasonYear">The season to get roster entries for</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>Roster information on all players in the MLB for the specified season year</returns>
    /// <exception cref="EmptyRosterException">Thrown if the roster is empty</exception>
    public async Task<IEnumerable<RosterEntry>> GetRosterEntries(SeasonYear seasonYear,
        CancellationToken cancellationToken = default)
    {
        var response =
            await _mlbApi.GetPlayersBySeason(new GetPlayersBySeasonRequest(seasonYear.Value, GameType.RegularSeason));

        if (response.Players == null || !response.Players.Any())
        {
            throw new EmptyRosterException($"{GetType().Name} - MLB API roster had no players for season {seasonYear}");
        }

        return response.Players.Select(x => _playerMapper.Map(x)).ToList();
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        // MLB API HTTP client is handled by Refit
    }
}