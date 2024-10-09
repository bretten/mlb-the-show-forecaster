namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;

/// <summary>
/// Represents performance metrics for a specified date
///
/// Stat metrics may be zero if the player has a card on the marketplace, but has not yet played a game before the
/// specified date
/// </summary>
/// <param name="Date">The date</param>
/// <param name="BattingScore">The player's batting performance score</param>
/// <param name="SignificantBattingParticipation">True if the player had significant batting participation</param>
/// <param name="PitchingScore">The player's pitching performance score</param>
/// <param name="SignificantPitchingParticipation">True if the player had significant pitching participation</param>
/// <param name="FieldingScore">The player's fielding performance score</param>
/// <param name="SignificantFieldingParticipation">True if the player had significant fielding participation</param>
/// <param name="BattingAverage">AVG</param>
/// <param name="OnBasePercentage">OBP</param>
/// <param name="Slugging">SLG</param>
/// <param name="EarnedRunAverage">ERA</param>
/// <param name="OpponentsBattingAverage">OBA</param>
/// <param name="StrikeoutsPer9">K/9</param>
/// <param name="BaseOnBallsPer9">BB/9</param>
/// <param name="HomeRunsPer9">HR/9</param>
/// <param name="FieldingPercentage">Fielding %</param>
public readonly record struct TrendMetricsByDate(
    DateOnly Date,
    int BuyPrice,
    int SellPrice,
    decimal? BattingScore,
    bool SignificantBattingParticipation,
    decimal? PitchingScore,
    bool SignificantPitchingParticipation,
    decimal? FieldingScore,
    bool SignificantFieldingParticipation,
    decimal? BattingAverage,
    decimal? OnBasePercentage,
    decimal? Slugging,
    decimal? EarnedRunAverage,
    decimal? OpponentsBattingAverage,
    decimal? StrikeoutsPer9,
    decimal? BaseOnBallsPer9,
    decimal? HomeRunsPer9,
    decimal? FieldingPercentage);