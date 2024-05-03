namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Services.Results;

/// <summary>
/// Represents the results of <see cref="IPlayerStatusTracker.TrackPlayers"/>
/// </summary>
/// <param name="TotalRosterEntries">The total number of roster entries from the external source <see cref="IPlayerRoster"/></param>
/// <param name="TotalNewPlayers">The number of players that were not in the domain</param>
/// <param name="TotalUpdatedPlayers">The number of players that were already in the domain, but had an updated status</param>
public readonly record struct PlayerStatusTrackerResult(
    int TotalRosterEntries,
    int TotalNewPlayers,
    int TotalUpdatedPlayers)
{
    /// <summary>
    /// The number of players who were already in the domain and had no status changes
    /// </summary>
    public int TotalUnchangedPlayers => TotalRosterEntries - TotalNewPlayers - TotalUpdatedPlayers;
}