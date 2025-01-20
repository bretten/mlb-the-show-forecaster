using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs.Io;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs.Io;
using Microsoft.AspNetCore.Mvc;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Api.Controllers;

/// <summary>
/// Allows jobs to be started and have their status tracked
/// </summary>
[ApiController]
[Route("[controller]s")]
public class JobController : Controller
{
    /// <summary>
    /// Manages the jobs available in this controller
    /// </summary>
    private readonly IJobManager _jobManager;

    /// <summary>
    /// Job ID for <see cref="IPlayerCardTracker"/>
    /// </summary>
    private const string PlayerCardTracker = nameof(PlayerCardTrackerJob);

    /// <summary>
    /// Job ID for <see cref="ICardPriceTracker"/>
    /// </summary>
    private const string CardPriceTracker = nameof(CardPriceTrackerJob);

    /// <summary>
    /// Job ID for <see cref="IRosterUpdateOrchestrator"/>
    /// </summary>
    private const string RosterUpdateOrchestrator = nameof(RosterUpdaterJob);

    /// <summary>
    /// Job ID for <see cref="ITrendReporter"/>
    /// </summary>
    private const string TrendReporter = nameof(TrendReporterJob);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="jobManager">Manages the jobs available in this controller</param>
    public JobController(IJobManager jobManager)
    {
        _jobManager = jobManager;
    }

    /// <summary>
    /// Starts the specified job
    /// </summary>
    /// <param name="jobId">The job to run</param>
    /// <param name="season">The season to run the job for</param>
    /// <param name="ct">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns><see cref="IActionResult"/></returns>
    [HttpPost("start")]
    public IActionResult Start(string jobId, int season, CancellationToken ct)
    {
        var s = new SeasonJobInput(SeasonYear.Create((ushort)season));
        switch (jobId)
        {
            case PlayerCardTracker:
                _ = Task.Run(async () => await _jobManager.Run<PlayerCardTrackerJob, PlayerCardTrackerJobResult>(s, ct),
                    ct);
                break;
            case CardPriceTracker:
                _ = Task.Run(async () => await _jobManager.Run<CardPriceTrackerJob, CardPriceTrackerJobResult>(s, ct),
                    ct);
                break;
            case RosterUpdateOrchestrator:
                _ = Task.Run(async () => await _jobManager.Run<RosterUpdaterJob, RosterUpdaterJobResult>(s, ct), ct);
                break;
            case TrendReporter:
                _ = Task.Run(async () => await _jobManager.Run<TrendReporterJob, TrendReporterJobResult>(s, ct), ct);
                break;
            default:
                return BadRequest($"Invalid job id: {jobId}");
        }

        return NoContent();
    }
}