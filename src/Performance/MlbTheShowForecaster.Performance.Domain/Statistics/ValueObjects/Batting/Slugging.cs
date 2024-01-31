using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;

/// <summary>
/// Slugging (SLG)
/// = [(1 x 1B) + (2 x 2B) + (3 x 3B) + (4 x HR)] / AB
/// = TB / AB
///
/// <para>A measure of a player's hitting power</para>
/// </summary>
public sealed class Slugging : CalculatedStat
{
    /// <summary>
    /// Total bases
    /// </summary>
    public TotalBases TotalBases { get; }

    /// <summary>
    /// The number of at-bats
    /// </summary>
    public NaturalNumber AtBats { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="totalBases">Total bases</param>
    /// <param name="atBats">The number of at-bats</param>
    private Slugging(TotalBases totalBases, NaturalNumber atBats)
    {
        TotalBases = totalBases;
        AtBats = atBats;
    }

    /// <summary>
    /// Calculates slugging
    /// </summary>
    /// <returns>Slugging</returns>
    protected override decimal Calculate()
    {
        return TotalBases.Value / AtBats.Value;
    }

    /// <summary>
    /// Creates <see cref="Slugging"/>
    /// </summary>
    /// <param name="totalBases">Total bases</param>
    /// <param name="atBats">The number of at-bats</param>
    /// <returns><see cref="Slugging"/></returns>
    public static Slugging Create(TotalBases totalBases, uint atBats)
    {
        return new Slugging(totalBases, NaturalNumber.Create(atBats));
    }
}