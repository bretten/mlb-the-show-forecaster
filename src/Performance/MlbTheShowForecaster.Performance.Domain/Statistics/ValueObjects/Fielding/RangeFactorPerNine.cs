using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;

/// <summary>
/// Range factor per 9 innings (RF/9)
/// = 9 * (PO + A) / IP
///
/// <para>Used to determine the amount of field that the player can cover</para>
/// </summary>
public sealed class RangeFactorPerNine : CalculatedStat
{
    /// <summary>
    /// The number of outs on a play where the fielder touched the ball excluding when this player does the actual putout
    /// </summary>
    public NaturalNumber Assists { get; }

    /// <summary>
    /// The number of times the fielder tags, forces, or appeals a runner and they are called out
    /// </summary>
    public NaturalNumber PutOuts { get; }

    /// <summary>
    /// The number of innings this player fielded at this position
    /// </summary>
    public InningsCount Innings { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="assists">The number of outs on a play where the fielder touched the ball excluding when this player does the actual putout</param>
    /// <param name="putOuts">The number of times the fielder tags, forces, or appeals a runner and they are called out</param>
    /// <param name="innings">The number of innings this player fielded at this position</param>
    private RangeFactorPerNine(NaturalNumber assists, NaturalNumber putOuts, InningsCount innings)
    {
        Assists = assists;
        PutOuts = putOuts;
        Innings = innings;
    }

    /// <summary>
    /// Calculates range factor per 9 innings
    /// </summary>
    /// <returns>Range factor per 9 innings</returns>
    protected override decimal Calculate()
    {
        return 9 * ((PutOuts.Value + Assists.Value) / Innings.Value);
    }

    /// <summary>
    /// Creates <see cref="RangeFactorPerNine"/>
    /// </summary>
    /// <param name="assists">The number of outs on a play where the fielder touched the ball excluding when this player does the actual putout</param>
    /// <param name="putOuts">The number of times the fielder tags, forces, or appeals a runner and they are called out</param>
    /// <param name="innings">The number of innings this player fielded at this position</param>
    /// <returns><see cref="RangeFactorPerNine"/></returns>
    public static RangeFactorPerNine Create(int assists, int putOuts, decimal innings)
    {
        return new RangeFactorPerNine(NaturalNumber.Create(assists), NaturalNumber.Create(putOuts),
            InningsCount.Create(innings));
    }
}