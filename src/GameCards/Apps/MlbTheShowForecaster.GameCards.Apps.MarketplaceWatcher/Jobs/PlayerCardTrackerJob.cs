using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs.Io;
using Microsoft.Extensions.Logging;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Jobs;

/// <summary>
/// Job that creates and updates all player cards
/// </summary>
public sealed class PlayerCardTrackerJob : BaseJob<SeasonJobInput, PlayerCardTrackerJobResult>, IDisposable
{
    /// <summary>
    /// Creates and updates all player cards
    /// </summary>
    private readonly IPlayerCardTracker _playerCardTracker;

    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<PlayerCardTrackerJob> _logger;

    /// <summary>
    /// Service name
    /// </summary>
    private const string S = nameof(IPlayerCardTracker);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerCardTracker">Creates and updates all player cards</param>
    /// <param name="logger">Logger</param>
    public PlayerCardTrackerJob(IPlayerCardTracker playerCardTracker, ILogger<PlayerCardTrackerJob> logger)
    {
        _playerCardTracker = playerCardTracker;
        _logger = logger;
    }

    /// <inheritdoc />
    public override async Task<PlayerCardTrackerJobResult> Execute(SeasonJobInput input,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"{S} - {input.Year.Value}");
        var result = await _playerCardTracker.TrackPlayerCards(input.Year, cancellationToken);
        _logger.LogInformation($"{S} - Total catalog cards = {result.TotalCatalogCards}");
        _logger.LogInformation($"{S} - Total new catalog cards = {result.TotalNewCatalogCards}");
        _logger.LogInformation($"{S} - Total updated player cards = {result.TotalUpdatedPlayerCards}");
        _logger.LogInformation($"{S} - Total unchanged player cards = {result.TotalUnchangedPlayerCards}");

        return new PlayerCardTrackerJobResult(result);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        _playerCardTracker.Dispose();
    }
}