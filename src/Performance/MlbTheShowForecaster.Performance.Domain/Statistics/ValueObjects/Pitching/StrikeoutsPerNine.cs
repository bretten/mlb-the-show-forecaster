using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

/// <summary>
/// Strikeouts per 9 innings pitched (K/9 or SO/9)
/// = 9 * (K / IP)
///
/// <para>The average number of strikeouts per 9 innings pitched</para>
/// </summary>
public sealed class StrikeoutsPerNine : CalculatedStat
{
    /// <summary>
    /// The number of strikeouts
    /// </summary>
    public NaturalNumber Strikeouts { get; }

    /// <summary>
    /// The number of innings pitched
    /// </summary>
    public InningsCount InningsPitched { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="strikeouts">The number of strikeouts</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    private StrikeoutsPerNine(NaturalNumber strikeouts, InningsCount inningsPitched)
    {
        Strikeouts = strikeouts;
        InningsPitched = inningsPitched;
    }

    /// <summary>
    /// Calculates strikeouts per 9 innings pitched
    /// </summary>
    /// <returns>Strikeouts per 9 innings pitched</returns>
    protected override decimal Calculate()
    {
        return 9 * (Strikeouts.Value / InningsPitched.Value);
    }

    /// <summary>
    /// Creates <see cref="StrikeoutsPerNine"/>
    /// </summary>
    /// <param name="strikeouts">The number of strikeouts</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    /// <returns><see cref="StrikeoutsPerNine"/></returns>
    public static StrikeoutsPerNine Create(uint strikeouts, decimal inningsPitched)
    {
        return new StrikeoutsPerNine(NaturalNumber.Create(strikeouts), InningsCount.Create(inningsPitched));
    }
}