using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

/// <summary>
/// Hits allowed per 9 innings pitched (H/9)
/// = 9 * (H / IP)
///
/// <para>The average number of hits allowed per 9 innings pitched</para>
/// </summary>
public sealed class HitsPerNine : CalculatedStat
{
    /// <summary>
    /// The number of hits allowed
    /// </summary>
    public NaturalNumber HitsAllowed { get; }

    /// <summary>
    /// The number of innings pitched
    /// </summary>
    public InningsCount InningsPitched { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="hitsAllowed">The number of hits allowed</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    private HitsPerNine(NaturalNumber hitsAllowed, InningsCount inningsPitched)
    {
        HitsAllowed = hitsAllowed;
        InningsPitched = inningsPitched;
    }

    /// <summary>
    /// Calculates hits per 9 innings pitched
    /// </summary>
    /// <returns>Hits per 9 innings pitched</returns>
    protected override decimal Calculate()
    {
        return 9 * (HitsAllowed.Value / InningsPitched.Value);
    }

    /// <summary>
    /// Creates <see cref="HitsPerNine"/>
    /// </summary>
    /// <param name="hitsAllowed">The number of hits allowed</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    /// <returns><see cref="HitsPerNine"/></returns>
    public static HitsPerNine Create(int hitsAllowed, decimal inningsPitched)
    {
        return new HitsPerNine(NaturalNumber.Create(hitsAllowed), InningsCount.Create(inningsPitched));
    }
}