using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Api.Responses;

/// <summary>
/// Represents a player's season stats
/// </summary>
/// <param name="Season">The player's season</param>
/// <param name="MlbId">The player's MLB ID</param>
/// <param name="BattingScore">The player's batting performance score</param>
/// <param name="PitchingScore">The player's pitching performance score</param>
/// <param name="FieldingScore">The player's fielding performance score</param>
public readonly record struct PlayerSeasonPerformanceResponse(
    ushort Season,
    int MlbId,
    decimal BattingScore,
    decimal PitchingScore,
    decimal FieldingScore)
{
    /// <summary>
    /// Creates a <see cref="PlayerSeasonPerformanceResponse"/> from a <see cref="PlayerStatsBySeason"/>
    /// </summary>
    /// <param name="playerStatsBySeason"><see cref="PlayerStatsBySeason"/></param>
    /// <returns><see cref="PlayerSeasonPerformanceResponse"/></returns>
    public static PlayerSeasonPerformanceResponse From(PlayerStatsBySeason playerStatsBySeason)
    {
        return new PlayerSeasonPerformanceResponse(playerStatsBySeason.SeasonYear.Value,
            playerStatsBySeason.PlayerMlbId.Value,
            BattingScore: playerStatsBySeason.BattingScore.Value,
            PitchingScore: playerStatsBySeason.PitchingScore.Value,
            FieldingScore: playerStatsBySeason.FieldingScore.Value
        );
    }
};