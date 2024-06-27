using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Api.Responses;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Api.Controllers;

/// <summary>
/// Exposes performance data
/// </summary>
[ApiController]
public sealed class PerformanceController : Controller
{
    /// <summary>
    /// Repo for player stats
    /// </summary>
    private readonly IPlayerStatsBySeasonRepository _playerStatsBySeasonRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerStatsBySeasonRepository">Repo for player stats</param>
    public PerformanceController(IPlayerStatsBySeasonRepository playerStatsBySeasonRepository)
    {
        _playerStatsBySeasonRepository = playerStatsBySeasonRepository;
    }

    /// <summary>
    /// Finds a player's season performance
    /// </summary>
    /// <param name="seasonQuery">The season</param>
    /// <param name="playerMlbIdQuery">The player's MLB ID</param>
    /// <returns>The player's season performance, or returns a 404 status code if not found</returns>
    [HttpGet("performance")]
    public async Task<ActionResult> FindPlayerSeasonPerformance([FromQuery(Name = "season")] ushort seasonQuery,
        [FromQuery(Name = "playerMlbId")] int playerMlbIdQuery)
    {
        var season = SeasonYear.Create(seasonQuery);
        var mlbId = MlbId.Create(playerMlbIdQuery);

        var playerStatsBySeason = await _playerStatsBySeasonRepository.GetBy(season, mlbId);
        if (playerStatsBySeason == null)
        {
            return new NotFoundResult();
        }

        return new JsonResult(PlayerSeasonPerformanceResponse.From(playerStatsBySeason));
    }
}