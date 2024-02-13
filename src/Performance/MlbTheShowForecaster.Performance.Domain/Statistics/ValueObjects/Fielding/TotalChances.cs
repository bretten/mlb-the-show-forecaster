using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;

/// <summary>
/// Total chances (TC)
/// = A + PO + E
///
/// <para>The number of plays in which a defensive player has participated</para>
/// </summary>
public sealed class TotalChances : CalculatedStat
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
    /// The number of times a fielder fails to make a play that is considered to be doable with common effort
    /// </summary>
    public NaturalNumber Errors { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="assists">The number of outs on a play where the fielder touched the ball excluding when this player does the actual putout</param>
    /// <param name="putOuts">The number of times the fielder tags, forces, or appeals a runner and they are called out</param>
    /// <param name="errors">The number of times a fielder fails to make a play that is considered to be doable with common effort</param>
    private TotalChances(NaturalNumber assists, NaturalNumber putOuts, NaturalNumber errors)
    {
        Assists = assists;
        PutOuts = putOuts;
        Errors = errors;
    }

    /// <summary>
    /// The number of fractional digits to round the decimal value to
    /// </summary>
    protected override int FractionalDigitCount => 0;

    /// <summary>
    /// Calculates total chances
    /// </summary>
    /// <returns>Total chances</returns>
    protected override decimal Calculate()
    {
        return Assists.Value + PutOuts.Value + Errors.Value;
    }

    /// <summary>
    /// Creates <see cref="TotalChances"/>
    /// </summary>
    /// <param name="assists">The number of outs on a play where the fielder touched the ball excluding when this player does the actual putout</param>
    /// <param name="putOuts">The number of times the fielder tags, forces, or appeals a runner and they are called out</param>
    /// <param name="errors">The number of times a fielder fails to make a play that is considered to be doable with common effort</param>
    /// <returns><see cref="TotalChances"/></returns>
    public static TotalChances Create(int assists, int putOuts, int errors)
    {
        return new TotalChances(NaturalNumber.Create(assists), NaturalNumber.Create(putOuts),
            NaturalNumber.Create(errors));
    }
}