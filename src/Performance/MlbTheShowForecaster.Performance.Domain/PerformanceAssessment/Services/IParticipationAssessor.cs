using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;

/// <summary>
/// Defines a service that assesses whether a set of stats for a time period has significant enough participation
/// to warrant evaluation
/// </summary>
public interface IParticipationAssessor
{
    /// <summary>
    /// Should assess if there was enough batting participation for the time period
    /// </summary>
    /// <param name="start">The start of the time period, inclusive</param>
    /// <param name="end">The end of the time period, inclusive</param>
    /// <param name="stats">The batting stats</param>
    /// <returns>True if participation was significant, otherwise false</returns>
    bool AssessBatting(DateOnly start, DateOnly end, BattingStats stats);

    /// <summary>
    /// Should assess if there was enough pitching participation for the time period
    /// </summary>
    /// <param name="start">The start of the time period, inclusive</param>
    /// <param name="end">The end of the time period, inclusive</param>
    /// <param name="stats">The batting stats</param>
    /// <returns>True if participation was significant, otherwise false</returns>
    bool AssessPitching(DateOnly start, DateOnly end, PitchingStats stats);

    /// <summary>
    /// Should assess if there was enough fielding participation for the time period
    /// </summary>
    /// <param name="start">The start of the time period, inclusive</param>
    /// <param name="end">The end of the time period, inclusive</param>
    /// <param name="stats">The batting stats</param>
    /// <returns>True if participation was significant, otherwise false</returns>
    bool AssessFielding(DateOnly start, DateOnly end, FieldingStats stats);
}