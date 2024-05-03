using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Results;

/// <summary>
/// Represents the result of <see cref="IRosterUpdateOrchestrator.SyncRosterUpdates"/>
/// </summary>
/// <param name="Date">The date of the roster update</param>
/// <param name="TotalRatingChanges">The number of player card rating changes</param>
/// <param name="TotalPositionChanges">The number of player card position changes</param>
/// <param name="TotalNewPlayers">The total number of new player cards</param>
public readonly record struct RosterUpdateOrchestratorResult(
    DateOnly Date,
    int TotalRatingChanges,
    int TotalPositionChanges,
    int TotalNewPlayers)
{
    /// <summary>
    /// Creates a new <see cref="RosterUpdateOrchestratorResult"/>
    /// </summary>
    /// <param name="rosterUpdate"><see cref="RosterUpdate"/></param>
    /// <returns><see cref="RosterUpdateOrchestratorResult"/></returns>
    public static RosterUpdateOrchestratorResult Create(RosterUpdate rosterUpdate)
    {
        return new RosterUpdateOrchestratorResult(Date: rosterUpdate.Date,
            TotalRatingChanges: rosterUpdate.RatingChanges.Count,
            TotalPositionChanges: rosterUpdate.PositionChanges.Count,
            TotalNewPlayers: rosterUpdate.NewPlayers.Count
        );
    }
};