using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;

/// <summary>
/// Ensures that an entity has seen enough playtime in order to have their stats properly assessed
/// </summary>
public sealed class PerformanceAssessmentRequirements : IPerformanceAssessmentRequirements
{
    /// <summary>
    /// The required amount for a stat to change by for it to be considered significant enough for an assessment. If
    /// the stat has not increased or decreased past this threshold, it is negligible
    /// </summary>
    public decimal StatPercentChangeThreshold { get; }

    /// <summary>
    /// The minimum required number of plate appearances
    /// </summary>
    public NaturalNumber MinimumPlateAppearances { get; }

    /// <summary>
    /// The minimum required number of innings pitched
    /// </summary>
    public InningsCount MinimumInningsPitched { get; }

    /// <summary>
    /// The minimum required number of batters faced
    /// </summary>
    public NaturalNumber MinimumBattersFaced { get; }

    /// <summary>
    /// The minimum required number of fielding chances
    /// </summary>
    public NaturalNumber MinimumTotalChances { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="statPercentChangeThreshold">The required amount for a stat to change by for it to be considered significant enough for an assessment</param>
    /// <param name="minimumPlateAppearances">The minimum required number of plate appearances</param>
    /// <param name="minimumInningsPitched">The minimum required number of innings pitched</param>
    /// <param name="minimumBattersFaced">The minimum required number of batters faced</param>
    /// <param name="minimumTotalChances">The minimum required number of fielding chances</param>
    public PerformanceAssessmentRequirements(decimal statPercentChangeThreshold, NaturalNumber minimumPlateAppearances,
        InningsCount minimumInningsPitched, NaturalNumber minimumBattersFaced, NaturalNumber minimumTotalChances)
    {
        StatPercentChangeThreshold = statPercentChangeThreshold;
        MinimumPlateAppearances = minimumPlateAppearances;
        MinimumInningsPitched = minimumInningsPitched;
        MinimumBattersFaced = minimumBattersFaced;
        MinimumTotalChances = minimumTotalChances;
    }

    /// <summary>
    /// Checks if the minimum batting requirements have been met
    /// </summary>
    /// <param name="plateAppearances">The number of plate appearances to check</param>
    /// <returns>True if the requirements have been met, otherwise false</returns>
    public bool AreBattingAssessmentRequirementsMet(NaturalNumber plateAppearances)
    {
        return plateAppearances.Value >= MinimumPlateAppearances.Value;
    }

    /// <summary>
    /// Checks if the minimum pitching requirements have been met
    /// </summary>
    /// <param name="inningsPitched">The number of innings pitched to check</param>
    /// <param name="battersFaced">The number of batters faced to check</param>
    /// <returns>True if the requirements have been met, otherwise false</returns>
    public bool ArePitchingAssessmentRequirementsMet(InningsCount inningsPitched, NaturalNumber battersFaced)
    {
        return inningsPitched.Value >= MinimumInningsPitched.Value && battersFaced.Value >= MinimumBattersFaced.Value;
    }

    /// <summary>
    /// Checks if the minimum fielding requirements have been met
    /// </summary>
    /// <param name="totalChances">The number of total fielding chances to check</param>
    /// <returns>True if the requirements have been met, otherwise false</returns>
    public bool AreFieldingAssessmentRequirementsMet(NaturalNumber totalChances)
    {
        return totalChances.Value >= MinimumTotalChances.Value;
    }
}