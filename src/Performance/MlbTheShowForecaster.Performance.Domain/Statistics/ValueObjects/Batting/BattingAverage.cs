using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

/// <summary>
/// Batting average (BA)
/// = H / AB
///
/// <para>Hits divided by at bats</para>
/// </summary>
public sealed class BattingAverage : CalculatedStat
{
    /// <summary>
    /// The number of hits
    /// </summary>
    public NaturalNumber Hits { get; }

    /// <summary>
    /// The number of at-ats
    /// </summary>
    public NaturalNumber AtBats { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="hits">Number of hits</param>
    /// <param name="atBats">Number of at- bats</param>
    private BattingAverage(NaturalNumber hits, NaturalNumber atBats)
    {
        Hits = hits;
        AtBats = atBats;
    }

    /// <summary>
    /// Creates <see cref="BattingAverage"/>
    /// </summary>
    /// <param name="hits">The number of hits</param>
    /// <param name="atBats">The number of at-bats</param>
    /// <returns><see cref="BattingAverage"/></returns>
    public static BattingAverage Create(NaturalNumber hits, NaturalNumber atBats)
    {
        return new BattingAverage(hits, atBats);
    }

    /// <summary>
    /// Calculates batting average
    /// </summary>
    /// <returns>Batting average</returns>
    protected override decimal Calculate()
    {
        return (decimal)Hits.Value / AtBats.Value;
    }
}