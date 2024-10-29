using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs.Io;
using Microsoft.Extensions.Logging;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs;

/// <summary>
/// Job that checks for new player card rating changes and applies them
/// </summary>
public sealed class RosterUpdaterJob : BaseJob<SeasonJobInput, RosterUpdaterJobResult>, IDisposable
{
    /// <summary>
    /// Updates player cards with any rating changes
    /// </summary>
    private readonly IRosterUpdateOrchestrator _rosterUpdateOrchestrator;

    /// <summary>
    /// Rebuilds the history of player card rating changes
    /// </summary>
    private readonly IPlayerRatingHistoryService _historyService;

    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<RosterUpdaterJob> _logger;

    /// <summary>
    /// Service name
    /// </summary>
    private const string S = nameof(IRosterUpdateOrchestrator);

    /// <summary>
    /// Service name
    /// </summary>
    private const string H = nameof(IPlayerRatingHistoryService);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rosterUpdateOrchestrator">Updates player cards with any rating changes</param>
    /// <param name="historyService">Rebuilds the history of player card rating changes</param>
    /// <param name="logger"></param>
    public RosterUpdaterJob(IRosterUpdateOrchestrator rosterUpdateOrchestrator,
        IPlayerRatingHistoryService historyService, ILogger<RosterUpdaterJob> logger)
    {
        _rosterUpdateOrchestrator = rosterUpdateOrchestrator;
        _historyService = historyService;
        _logger = logger;
    }

    /// <inheritdoc />
    public override async Task<RosterUpdaterJobResult> Execute(SeasonJobInput input,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"{S} - {input.Year.Value}");

        // Sync the player card historical ratings before running roster updates
        var historyResult = await _historyService.SyncHistory(input.Year, cancellationToken);
        _logger.LogInformation($"{H} - Total cards updated = {historyResult.UpdatedPlayerCards.Count()}");
        foreach (var historicalUpdate in historyResult.UpdatedPlayerCards)
        {
            var updatedCard = historicalUpdate.Key;
            foreach (var update in historicalUpdate.Value)
            {
                _logger.LogInformation(
                    $"{H} - Updated {updatedCard.Name.Value}, {updatedCard.ExternalId.Value}, {update.StartDate.ToString("O")}");
            }
        }

        var results = (await _rosterUpdateOrchestrator.SyncRosterUpdates(input.Year, cancellationToken)).ToList();
        foreach (var result in results)
        {
            _logger.LogInformation($"{S} - Date = {result.Date}");
            _logger.LogInformation($"{S} - Total rating changes = {result.TotalRatingChanges}");
            _logger.LogInformation($"{S} - Total position changes = {result.TotalPositionChanges}");
            _logger.LogInformation($"{S} - Total new players = {result.TotalNewPlayers}");
        }

        return new RosterUpdaterJobResult(TotalHistoricalUpdatesApplied: historyResult.UpdatedPlayerCards.Count,
            TotalRosterUpdatesApplied: results.Count);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        _rosterUpdateOrchestrator.Dispose();
        _historyService.Dispose();
    }
}