using com.brettnamba.MlbTheShowForecaster.Common.Application.Jobs;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.Jobs.Io;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.Jobs;

/// <summary>
/// Job that gets the current status of players
/// </summary>
public sealed class PlayerStatusTrackerJob : BaseJob<SeasonJobInput, PlayerStatusTrackerJobResult>, IDisposable
{
    /// <summary>
    /// Updates the current status of all players
    /// </summary>
    private readonly IPlayerStatusTracker _playerStatusTracker;

    /// <summary>
    /// Logger
    /// </summary>
    private readonly ILogger<PlayerStatusTrackerJob> _logger;

    /// <summary>
    /// Service name
    /// </summary>
    private const string S = nameof(IPlayerStatusTracker);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerStatusTracker">Updates the current status of all players</param>
    /// <param name="logger">Logger</param>
    public PlayerStatusTrackerJob(IPlayerStatusTracker playerStatusTracker, ILogger<PlayerStatusTrackerJob> logger)
    {
        _playerStatusTracker = playerStatusTracker;
        _logger = logger;
    }

    /// <inheritdoc />
    public override async Task<PlayerStatusTrackerJobResult> Execute(SeasonJobInput input,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"{S} - {input.Year.Value}");
        var result = await _playerStatusTracker.TrackPlayers(input.Year, cancellationToken);
        _logger.LogInformation($"{S} - Total roster entries = {result.TotalRosterEntries}");
        _logger.LogInformation($"{S} - Total new players = {result.TotalNewPlayers}");
        _logger.LogInformation($"{S} - Total updated players = {result.TotalUpdatedPlayers}");
        _logger.LogInformation($"{S} - Total unchanged players = {result.TotalUnchangedPlayers}");
        return new PlayerStatusTrackerJobResult(TotalRosterEntries: result.TotalRosterEntries,
            TotalNewPlayers: result.TotalNewPlayers, TotalUpdatedPlayers: result.TotalUpdatedPlayers);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        _playerStatusTracker.Dispose();
    }
}