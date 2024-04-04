namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

/// <summary>
/// Represents a collection of ratings and player changes from the external source
/// </summary>
/// <param name="Date">The date of the update</param>
/// <param name="RatingChanges">Player card rating changes</param>
/// <param name="PositionChanges">Player cards with new primary positions</param>
/// <param name="NewPlayers">Newly added player cards</param>
public readonly record struct RosterUpdate(
    DateOnly Date,
    IReadOnlyList<PlayerRatingChange> RatingChanges,
    IReadOnlyList<PlayerPositionChange> PositionChanges,
    IReadOnlyList<PlayerAddition> NewPlayers
);