using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

/// <summary>
/// Earned run average (ERA)
/// = 9 * (ER / IP)
///
/// <para>The average number of earned runs per 9 innings pitched</para>
/// </summary>
public sealed class EarnedRunAverage : CalculatedStat
{
    /// <summary>
    /// The number of earned runs allowed
    /// </summary>
    public NaturalNumber EarnedRuns { get; }

    /// <summary>
    /// The number of innings pitched
    /// </summary>
    public InningsCount InningsPitched { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="earnedRuns">The number of earned runs allowed</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    private EarnedRunAverage(NaturalNumber earnedRuns, InningsCount inningsPitched)
    {
        EarnedRuns = earnedRuns;
        InningsPitched = inningsPitched;
    }

    /// <summary>
    /// The number of fractional digits to round the decimal value to
    /// </summary>
    protected override int FractionalDigitCount => 2;

    /// <summary>
    /// Calculates earned run average
    /// </summary>
    /// <returns>Earned run average</returns>
    protected override decimal Calculate()
    {
        return 9 * (EarnedRuns.Value / InningsPitched.Value);
    }

    /// <summary>
    /// Creates <see cref="EarnedRunAverage"/>
    /// </summary>
    /// <param name="earnedRuns">The number of earned runs allowed</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    /// <returns><see cref="EarnedRunAverage"/></returns>
    public static EarnedRunAverage Create(int earnedRuns, decimal inningsPitched)
    {
        return new EarnedRunAverage(NaturalNumber.Create(earnedRuns), InningsCount.Create(inningsPitched));
    }
}