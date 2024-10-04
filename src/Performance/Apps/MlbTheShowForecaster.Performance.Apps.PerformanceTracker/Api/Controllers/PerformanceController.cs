using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Api.Responses;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
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
    /// Maps <see cref="PlayerStatsBySeason"/> to a DTO
    /// </summary>
    private readonly IPlayerSeasonMapper _playerSeasonMapper;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerStatsBySeasonRepository">Repo for player stats</param>
    /// <param name="playerSeasonMapper">Maps <see cref="PlayerStatsBySeason"/> to a DTO</param>
    public PerformanceController(IPlayerStatsBySeasonRepository playerStatsBySeasonRepository,
        IPlayerSeasonMapper playerSeasonMapper)
    {
        _playerStatsBySeasonRepository = playerStatsBySeasonRepository;
        _playerSeasonMapper = playerSeasonMapper;
    }

    /// <summary>
    /// Finds a player's season performance
    /// </summary>
    /// <param name="season">The season</param>
    /// <param name="playerMlbId">The player's MLB ID</param>
    /// <param name="start">Start date of the stats</param>
    /// <param name="end">End date of the stats</param>
    /// <returns>The player's season performance, or returns a 404 status code if not found</returns>
    [HttpGet("performance")]
    public async Task<ActionResult> FindPlayerSeasonPerformance([FromQuery] ushort season, [FromQuery] int playerMlbId,
        [FromQuery] DateOnly start, [FromQuery] DateOnly end)
    {
        var seasonYear = SeasonYear.Create(season);
        var mlbId = MlbId.Create(playerMlbId);

        var playerStatsBySeason = await _playerStatsBySeasonRepository.GetBy(seasonYear, mlbId);
        if (playerStatsBySeason == null)
        {
            return new NotFoundResult();
        }

        var playerSeasonPerformanceMetrics =
            _playerSeasonMapper.MapToPlayerSeasonPerformanceMetrics(playerStatsBySeason, start, end);

        return new JsonResult(new PlayerSeasonPerformanceResponse(
            Season: season,
            MlbId: playerMlbId,
            MetricsByDate: playerSeasonPerformanceMetrics.MetricsByDate
        ));
    }
}