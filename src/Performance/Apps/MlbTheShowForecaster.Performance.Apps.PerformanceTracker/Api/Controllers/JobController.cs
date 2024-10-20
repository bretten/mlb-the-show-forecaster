using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Jobs;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Jobs.Io;
using Microsoft.AspNetCore.Mvc;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Api.Controllers;

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
    /// Job ID for <see cref="IPerformanceTracker"/>
    /// </summary>
    private const string PerformanceTracker = nameof(PerformanceTrackerJob);

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
            case PerformanceTracker:
                _ = Task.Run(
                    async () => await _jobManager.Run<PerformanceTrackerJob, PerformanceTrackerJobResult>(s, ct), ct);
                break;
            default:
                return BadRequest($"Invalid job id: {jobId}");
        }

        return NoContent();
    }
}