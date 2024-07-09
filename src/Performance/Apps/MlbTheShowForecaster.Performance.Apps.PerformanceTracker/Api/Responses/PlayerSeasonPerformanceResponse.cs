namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Api.Responses;

/// <summary>
/// Represents a player's season performance
/// </summary>
/// <param name="Season">The player's season</param>
/// <param name="MlbId">The player's MLB ID</param>
/// <param name="BattingScore">The player's batting performance score</param>
/// <param name="HadSignificantBattingParticipation">True if the player had significant batting participation</param>
/// <param name="PitchingScore">The player's pitching performance score</param>
/// <param name="HadSignificantPitchingParticipation">True if the player had significant pitching participation</param>
/// <param name="FieldingScore">The player's fielding performance score</param>
/// <param name="HadSignificantFieldingParticipation">True if the player had significant fielding participation</param>
public readonly record struct PlayerSeasonPerformanceResponse(
    ushort Season,
    int MlbId,
    decimal BattingScore,
    bool HadSignificantBattingParticipation,
    decimal PitchingScore,
    bool HadSignificantPitchingParticipation,
    decimal FieldingScore,
    bool HadSignificantFieldingParticipation);