using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;

/// <summary>
/// Fielding percentage (FP)
/// = (PO + A) / (PO + A + E)
///
/// <para>Percentage of times a defensive player properly handles a batted or thrown ball</para>
/// </summary>
public sealed class FieldingPercentage : CalculatedStat
{
    /// <summary>
    /// The number of outs on a play where the fielder touched the ball excluding when this player does the actual putout
    /// </summary>
    public NaturalNumber Assists { get; }

    /// <summary>
    /// The number of times the fielder tags, forces, or appeals a runner and they are called out
    /// </summary>
    public NaturalNumber Putouts { get; }

    /// <summary>
    /// The number of times a fielder fails to make a play that is considered to be doable with common effort
    /// </summary>
    public NaturalNumber Errors { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="assists">The number of outs on a play where the fielder touched the ball excluding when this player does the actual putout</param>
    /// <param name="putouts">The number of times the fielder tags, forces, or appeals a runner and they are called out</param>
    /// <param name="errors">The number of times a fielder fails to make a play that is considered to be doable with common effort</param>
    private FieldingPercentage(NaturalNumber assists, NaturalNumber putouts, NaturalNumber errors)
    {
        Assists = assists;
        Putouts = putouts;
        Errors = errors;
    }

    /// <summary>
    /// Calculates fielding percentage
    /// </summary>
    /// <returns>Fielding percentage</returns>
    protected override decimal Calculate()
    {
        return (decimal)(Putouts.Value + Assists.Value) / (Putouts.Value + Assists.Value + Errors.Value);
    }

    /// <summary>
    /// Creates <see cref="FieldingPercentage"/>
    /// </summary>
    /// <param name="assists">The number of outs on a play where the fielder touched the ball excluding when this player does the actual putout</param>
    /// <param name="putouts">The number of times the fielder tags, forces, or appeals a runner and they are called out</param>
    /// <param name="errors">The number of times a fielder fails to make a play that is considered to be doable with common effort</param>
    /// <returns><see cref="FieldingPercentage"/></returns>
    public static FieldingPercentage Create(int assists, int putouts, int errors)
    {
        return new FieldingPercentage(NaturalNumber.Create(assists), NaturalNumber.Create(putouts),
            NaturalNumber.Create(errors));
    }
}