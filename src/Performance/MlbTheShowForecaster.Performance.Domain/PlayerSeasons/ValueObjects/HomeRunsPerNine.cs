using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

/// <summary>
/// Home runs allowed per 9 innings pitched (HR/9)
/// = 9 * (HR / IP)
///
/// <para>The average number of home runs allowed per 9 innings pitched</para>
/// </summary>
public sealed class HomeRunsPerNine : CalculatedStat
{
    /// <summary>
    /// The number of home runs allowed
    /// </summary>
    public NaturalNumber HomeRunsAllowed { get; }

    /// <summary>
    /// The number of innings pitched
    /// </summary>
    public InningsPitched InningsPitched { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="homeRunsAllowed">The number of home runs allowed</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    private HomeRunsPerNine(NaturalNumber homeRunsAllowed, InningsPitched inningsPitched)
    {
        HomeRunsAllowed = homeRunsAllowed;
        InningsPitched = inningsPitched;
    }

    /// <summary>
    /// Calculates home runs allowed per 9 innings pitched
    /// </summary>
    /// <returns>Home runs allowed per 9 innings pitched</returns>
    protected override decimal Calculate()
    {
        return 9 * (HomeRunsAllowed.Value / InningsPitched.Value);
    }

    /// <summary>
    /// Creates <see cref="HomeRunsPerNine"/>
    /// </summary>
    /// <param name="homeRunsAllowed">The number of home runs allowed</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    /// <returns><see cref="HomeRunsPerNine"/></returns>
    public static HomeRunsPerNine Create(uint homeRunsAllowed, decimal inningsPitched)
    {
        return new HomeRunsPerNine(NaturalNumber.Create(homeRunsAllowed), InningsPitched.Create(inningsPitched));
    }
}