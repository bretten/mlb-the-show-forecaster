using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

/// <summary>
/// Runs allowed per 9 innings pitched (R/9)
/// = 9 * (R / IP)
///
/// <para>The average number of runs allowed per 9 innings pitched</para>
/// </summary>
public sealed class RunsScoredPerNine : CalculatedStat
{
    /// <summary>
    /// The number of runs allowed
    /// </summary>
    public NaturalNumber RunsAllowed { get; }

    /// <summary>
    /// The number of innings pitched
    /// </summary>
    public InningsCount InningsPitched { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="runsAllowed">The number of runs allowed</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    private RunsScoredPerNine(NaturalNumber runsAllowed, InningsCount inningsPitched)
    {
        RunsAllowed = runsAllowed;
        InningsPitched = inningsPitched;
    }

    /// <summary>
    /// Calculates runs scored per 9 innings
    /// </summary>
    /// <returns>Runs scored per 9 innings</returns>
    protected override decimal Calculate()
    {
        return 9 * (RunsAllowed.Value / InningsPitched.Value);
    }

    /// <summary>
    /// Creates <see cref="RunsScoredPerNine"/>
    /// </summary>
    /// <param name="runsAllowed">The number of runs allowed</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    /// <returns><see cref="RunsScoredPerNine"/></returns>
    public static RunsScoredPerNine Create(uint runsAllowed, decimal inningsPitched)
    {
        return new RunsScoredPerNine(NaturalNumber.Create(runsAllowed), InningsCount.Create(inningsPitched));
    }
}