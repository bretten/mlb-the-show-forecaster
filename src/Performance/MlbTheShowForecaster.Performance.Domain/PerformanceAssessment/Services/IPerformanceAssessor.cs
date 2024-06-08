using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;

/// <summary>
/// Defines a service that assesses the performance of <see cref="BattingStats"/>, <see cref="PitchingStats"/>
/// and <see cref="FieldingStats"/>
/// </summary>
public interface IPerformanceAssessor
{
    /// <summary>
    /// Should assess <see cref="BattingStats"/>
    /// </summary>
    /// <param name="stats"><see cref="BattingStats"/></param>
    /// <returns>The assessment result</returns>
    PerformanceAssessmentResult AssessBatting(BattingStats stats);

    /// <summary>
    /// Should assess <see cref="PitchingStats"/>
    /// </summary>
    /// <param name="stats"><see cref="PitchingStats"/></param>
    /// <returns>The assessment result</returns>
    PerformanceAssessmentResult AssessPitching(PitchingStats stats);

    /// <summary>
    /// Should assess <see cref="FieldingStats"/>
    /// </summary>
    /// <param name="stats"><see cref="FieldingStats"/></param>
    /// <returns>The assessment result</returns>
    PerformanceAssessmentResult AssessFielding(FieldingStats stats);
}