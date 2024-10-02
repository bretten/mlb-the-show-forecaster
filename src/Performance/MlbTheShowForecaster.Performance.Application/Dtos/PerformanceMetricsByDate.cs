namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;

/// <summary>
/// Represents performance metrics for a specified date
/// </summary>
public readonly record struct PerformanceMetricsByDate(
    DateOnly Date,
    decimal BattingScore,
    bool SignificantBattingParticipation,
    decimal PitchingScore,
    bool SignificantPitchingParticipation,
    decimal FieldingScore,
    bool SignificantFieldingParticipation,
    decimal BattingAverage,
    decimal OnBasePercentage,
    decimal Slugging,
    decimal EarnedRunAverage,
    decimal OpponentsBattingAverage,
    decimal StrikeoutsPer9,
    decimal BaseOnBallsPer9,
    decimal HomeRunsPer9,
    decimal FieldingPercentage
);