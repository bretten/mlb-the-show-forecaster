using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Api.Responses;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
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
    /// Assesses performance
    /// </summary>
    private readonly IPerformanceAssessor _performanceAssessor;

    /// <summary>
    /// Assesses participation
    /// </summary>
    private readonly IParticipationAssessor _participationAssessor;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerStatsBySeasonRepository">Repo for player stats</param>
    /// <param name="performanceAssessor">Assesses performance</param>
    /// <param name="participationAssessor">Assesses participation</param>
    public PerformanceController(IPlayerStatsBySeasonRepository playerStatsBySeasonRepository,
        IPerformanceAssessor performanceAssessor, IParticipationAssessor participationAssessor)
    {
        _playerStatsBySeasonRepository = playerStatsBySeasonRepository;
        _performanceAssessor = performanceAssessor;
        _participationAssessor = participationAssessor;
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

        var battingStats = playerStatsBySeason.BattingStatsFor(start, end);
        var battingScore = _performanceAssessor.AssessBatting(battingStats);
        var significantBattingParticipation = _participationAssessor.AssessBatting(start, end, battingStats);

        var pitchingStats = playerStatsBySeason.PitchingStatsFor(start, end);
        var pitchingScore = _performanceAssessor.AssessPitching(pitchingStats);
        var significantPitchingParticipation = _participationAssessor.AssessPitching(start, end, pitchingStats);

        var fieldingStats = playerStatsBySeason.FieldingStatsFor(start, end);
        var fieldingScore = _performanceAssessor.AssessFielding(fieldingStats);
        var significantFieldingParticipation = _participationAssessor.AssessFielding(start, end, fieldingStats);

        return new JsonResult(new PlayerSeasonPerformanceResponse(playerStatsBySeason.SeasonYear.Value,
            playerStatsBySeason.PlayerMlbId.Value,
            BattingScore: battingScore.Value,
            HadSignificantBattingParticipation: significantBattingParticipation,
            PitchingScore: pitchingScore.Value,
            HadSignificantPitchingParticipation: significantPitchingParticipation,
            FieldingScore: fieldingScore.Value,
            HadSignificantFieldingParticipation: significantFieldingParticipation
        ));
    }
}