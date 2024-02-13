using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

/// <summary>
/// Total bases (TB)
/// = (1 x 1B) + (2 x 2B) + (3 x 3B) + (4 x HR)
/// = (1 x [H - 2B - 3B - HR]) + (2 x 2B) + (3 x 3B) + (4 x HR)
/// = H + 2B + (2 x 3B) + (3 x HR)
///
/// <para>The total number of bases for a set of hits</para>
/// </summary>
public sealed class TotalBases : CalculatedStat
{
    /// <summary>
    /// The number of singles
    /// </summary>
    public NaturalNumber Singles { get; }

    /// <summary>
    /// The number of doubles
    /// </summary>
    public NaturalNumber Doubles { get; }

    /// <summary>
    /// The number of triples
    /// </summary>
    public NaturalNumber Triples { get; }

    /// <summary>
    /// The number of home runs
    /// </summary>
    public NaturalNumber HomeRuns { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="singles">The number of singles</param>
    /// <param name="doubles">The number of doubles</param>
    /// <param name="triples">The number of triples</param>
    /// <param name="homeRuns">The number of home runs</param>
    private TotalBases(NaturalNumber singles, NaturalNumber doubles, NaturalNumber triples, NaturalNumber homeRuns)
    {
        Singles = singles;
        Doubles = doubles;
        Triples = triples;
        HomeRuns = homeRuns;
    }

    /// <summary>
    /// The number of fractional digits to round the decimal value to
    /// </summary>
    protected override int FractionalDigitCount => 0;

    /// <summary>
    /// Calculates total bases
    /// </summary>
    /// <returns>Total bases</returns>
    protected override decimal Calculate()
    {
        return Singles.Value + (2 * Doubles.Value) + (3 * Triples.Value) + (4 * HomeRuns.Value);
    }

    /// <summary>
    /// Creates <see cref="TotalBases"/>
    /// </summary>
    /// <param name="singles">The number of singles</param>
    /// <param name="doubles">The number of doubles</param>
    /// <param name="triples">The number of triples</param>
    /// <param name="homeRuns">The number of home runs</param>
    /// <returns><see cref="TotalBases"/></returns>
    public static TotalBases Create(int singles, int doubles, int triples, int homeRuns)
    {
        return new TotalBases(NaturalNumber.Create(singles), NaturalNumber.Create(doubles),
            NaturalNumber.Create(triples), NaturalNumber.Create(homeRuns));
    }

    /// <summary>
    /// Creates <see cref="TotalBases"/> with the number of hits instead of singles
    /// </summary>
    /// <param name="hits">The number of hits</param>
    /// <param name="doubles">The number of doubles</param>
    /// <param name="triples">The number of triples</param>
    /// <param name="homeRuns">The number of home runs</param>
    /// <returns><see cref="TotalBases"/></returns>
    public static TotalBases CreateWithHits(int hits, int doubles, int triples, int homeRuns)
    {
        var singles = hits - doubles - triples - homeRuns;
        return new TotalBases(NaturalNumber.Create(singles), NaturalNumber.Create(doubles),
            NaturalNumber.Create(triples), NaturalNumber.Create(homeRuns));
    }
}