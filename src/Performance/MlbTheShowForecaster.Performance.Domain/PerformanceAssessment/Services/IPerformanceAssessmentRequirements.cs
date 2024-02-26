using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;

/// <summary>
/// Should ensure that an entity has seen enough playtime in order to have their stats properly assessed
/// </summary>
public interface IPerformanceAssessmentRequirements
{
    /// <summary>
    /// The required amount for a stat to change by for it to be considered significant enough for an assessment. If
    /// the stat has not increased or decreased past this threshold, it is negligible
    /// </summary>
    decimal StatPercentChangeThreshold { get; }

    /// <summary>
    /// The minimum required number of plate appearances
    /// </summary>
    NaturalNumber MinimumPlateAppearances { get; }

    /// <summary>
    /// The minimum required number of innings pitched
    /// </summary>
    InningsCount MinimumInningsPitched { get; }

    /// <summary>
    /// The minimum required number of batters faced
    /// </summary>
    NaturalNumber MinimumBattersFaced { get; }

    /// <summary>
    /// The minimum required number of fielding chances
    /// </summary>
    NaturalNumber MinimumTotalChances { get; }

    /// <summary>
    /// Checks if the minimum batting requirements have been met
    /// </summary>
    /// <param name="plateAppearances">The number of plate appearances to check</param>
    /// <returns>True if the requirements have been met, otherwise false</returns>
    bool AreBattingAssessmentRequirementsMet(NaturalNumber plateAppearances);

    /// <summary>
    /// Checks if the minimum pitching requirements have been met
    /// </summary>
    /// <param name="inningsPitched">The number of innings pitched to check</param>
    /// <param name="battersFaced">The number of batters faced to check</param>
    /// <returns>True if the requirements have been met, otherwise false</returns>
    bool ArePitchingAssessmentRequirementsMet(InningsCount inningsPitched, NaturalNumber battersFaced);

    /// <summary>
    /// Checks if the minimum fielding requirements have been met
    /// </summary>
    /// <param name="totalChances">The number of total fielding chances to check</param>
    /// <returns>True if the requirements have been met, otherwise false</returns>
    bool AreFieldingAssessmentRequirementsMet(NaturalNumber totalChances);
}