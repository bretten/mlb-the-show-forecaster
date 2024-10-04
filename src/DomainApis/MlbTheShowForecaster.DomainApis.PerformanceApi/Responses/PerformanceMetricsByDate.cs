using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.DomainApis.PerformanceApi.Responses;

/// <summary>
/// Represents performance metrics for a specified date
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
public readonly record struct PerformanceMetricsByDate(
    [property: JsonPropertyName("date")] DateOnly Date,
    [property: JsonPropertyName("battingScore")]
    decimal BattingScore,
    [property: JsonPropertyName("significantBattingParticipation")]
    bool SignificantBattingParticipation,
    [property: JsonPropertyName("pitchingScore")]
    decimal PitchingScore,
    [property: JsonPropertyName("significantPitchingParticipation")]
    bool SignificantPitchingParticipation,
    [property: JsonPropertyName("fieldingScore")]
    decimal FieldingScore,
    [property: JsonPropertyName("significantFieldingParticipation")]
    bool SignificantFieldingParticipation,
    [property: JsonPropertyName("battingAverage")]
    decimal BattingAverage,
    [property: JsonPropertyName("onBasePercentage")]
    decimal OnBasePercentage,
    [property: JsonPropertyName("slugging")]
    decimal Slugging,
    [property: JsonPropertyName("earnedRunAverage")]
    decimal EarnedRunAverage,
    [property: JsonPropertyName("opponentsBattingAverage")]
    decimal OpponentsBattingAverage,
    [property: JsonPropertyName("strikeoutsPer9")]
    decimal StrikeoutsPer9,
    [property: JsonPropertyName("baseOnBallsPer9")]
    decimal BaseOnBallsPer9,
    [property: JsonPropertyName("homeRunsPer9")]
    decimal HomeRunsPer9,
    [property: JsonPropertyName("fieldingPercentage")]
    decimal FieldingPercentage
);