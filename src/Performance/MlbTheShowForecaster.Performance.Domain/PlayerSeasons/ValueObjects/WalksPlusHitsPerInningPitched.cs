using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

/// <summary>
/// Walks plus hits per inning pitched (WHIP)
/// = (BB + H) / IP
///
/// <para>Measures the number of base runners a pitcher allowed per inning pitched</para>
/// </summary>
public sealed class WalksPlusHitsPerInningPitched : CalculatedStat
{
    /// <summary>
    /// The number of walks
    /// </summary>
    public NaturalNumber BaseOnBalls { get; }

    /// <summary>
    /// The number of hits
    /// </summary>
    public NaturalNumber Hits { get; }

    /// <summary>
    /// The number of innings pitched
    /// </summary>
    public InningsCount InningsPitched { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="baseOnBalls">The number of walks</param>
    /// <param name="hits">The number of hits</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    private WalksPlusHitsPerInningPitched(NaturalNumber baseOnBalls, NaturalNumber hits, InningsCount inningsPitched)
    {
        BaseOnBalls = baseOnBalls;
        Hits = hits;
        InningsPitched = inningsPitched;
    }

    /// <summary>
    /// Calculates walks plus hits per inning pitched
    /// </summary>
    /// <returns>Walks plus hits per inning pitched</returns>
    protected override decimal Calculate()
    {
        return (BaseOnBalls.Value + Hits.Value) / InningsPitched.Value;
    }

    /// <summary>
    /// Creates <see cref="WalksPlusHitsPerInningPitched"/>
    /// </summary>
    /// <param name="baseOnBalls">The number of walks</param>
    /// <param name="hits">The number of hits</param>
    /// <param name="inningsPitched">The number of innings pitched</param>
    /// <returns><see cref="WalksPlusHitsPerInningPitched"/></returns>
    public static WalksPlusHitsPerInningPitched Create(uint baseOnBalls, uint hits, uint inningsPitched)
    {
        return new WalksPlusHitsPerInningPitched(NaturalNumber.Create(baseOnBalls),
            NaturalNumber.Create(hits),
            InningsCount.Create(inningsPitched));
    }
}