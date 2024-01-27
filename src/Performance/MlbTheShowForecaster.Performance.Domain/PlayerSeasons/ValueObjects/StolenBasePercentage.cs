using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

/// <summary>
/// Stolen base percentage
/// = SB / SB attempts
/// = SB / (SB + CaughtStealing)
///
/// <para>Percentage of bases stolen successfully</para>
/// </summary>
public sealed class StolenBasePercentage : CalculatedStat
{
    /// <summary>
    /// The number of stolen bases
    /// </summary>
    public NaturalNumber StolenBases { get; }

    /// <summary>
    /// The number of times caught stealing
    /// </summary>
    public NaturalNumber CaughtStealing { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="stolenBases">The number of stolen bases</param>
    /// <param name="caughtStealing">The number of times caught stealing</param>
    private StolenBasePercentage(NaturalNumber stolenBases, NaturalNumber caughtStealing)
    {
        StolenBases = stolenBases;
        CaughtStealing = caughtStealing;
    }

    /// <summary>
    /// Creates <see cref="StolenBasePercentage"/>
    /// </summary>
    /// <param name="stolenBases">The number of stolen bases</param>
    /// <param name="caughtStealing">The number of times caught stealing</param>
    /// <returns><see cref="StolenBasePercentage"/></returns>
    public static StolenBasePercentage Create(NaturalNumber stolenBases, NaturalNumber caughtStealing)
    {
        return new StolenBasePercentage(stolenBases, caughtStealing);
    }

    /// <summary>
    /// Calculates stolen base percentage
    /// </summary>
    /// <returns>Stolen base percentage</returns>
    protected override decimal Calculate()
    {
        return (decimal)StolenBases.Value / (StolenBases.Value + CaughtStealing.Value);
    }
}