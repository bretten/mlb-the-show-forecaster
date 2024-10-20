using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Jobs.Io;
using Microsoft.Extensions.Logging;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Jobs;

/// <summary>
/// Job that gets current and historical performance metrics
/// </summary>
public sealed class PerformanceTrackerJob : BaseJob<SeasonJobInput, PerformanceTrackerJobResult>, IDisposable
{
    /// <summary>
    /// Updates the current and historical performance metrics
    /// </summary>
    private readonly IPerformanceTracker _performanceTracker;

    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<PerformanceTrackerJob> _logger;

    /// <summary>
    /// Service name
    /// </summary>
    private const string S = nameof(IPerformanceTracker);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="performanceTracker">Updates the current and historical performance metrics</param>
    /// <param name="logger">Logger</param>
    public PerformanceTrackerJob(IPerformanceTracker performanceTracker, ILogger<PerformanceTrackerJob> logger)
    {
        _performanceTracker = performanceTracker;
        _logger = logger;
    }

    /// <inheritdoc />
    public override async Task<PerformanceTrackerJobResult> Execute(SeasonJobInput input,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"{S} - {input.Year.Value}");
        var result = await _performanceTracker.TrackPlayerPerformance(input.Year, cancellationToken);
        _logger.LogInformation($"{S} - Total player seasons = {result.TotalPlayerSeasons}");
        _logger.LogInformation($"{S} - Total new player seasons = {result.TotalNewPlayerSeasons}");
        _logger.LogInformation($"{S} - Total player season updates = {result.TotalPlayerSeasonUpdates}");
        _logger.LogInformation($"{S} - Total up-to-date player seasons = {result.TotalUpToDatePlayerSeasons}");
        return new PerformanceTrackerJobResult(result);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        _performanceTracker.Dispose();
    }
}