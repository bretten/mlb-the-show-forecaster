using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

/// <summary>
/// Innings count
///
/// <para>Used to represent innings pitched (IP) by a pitcher or innings played (INN) by a defensive player.
/// When a pitcher makes one out or a fielder plays on defense for one out, it counts as 1/3 of an inning. Two outs
/// counts as 2/3 of an inning. MLB shortens these to 0.1 IP and 0.2 IP respectively.</para>
/// </summary>
public sealed class InningsCount : ValueObject
{
    /// <summary>
    /// Maps shorthand partial innings to their actual values
    /// </summary>
    private static readonly Dictionary<decimal, decimal> ValidPartialInningShorthandMap = new()
    {
        { 0.0m, 0.0m },
        { 0.1m, Round((decimal)1 / 3) },
        { 0.2m, Round((decimal)2 / 3) },
    };

    /// <summary>
    /// The underlying innings pitched value
    /// </summary>
    public decimal Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The underlying innings pitched value</param>
    /// <exception cref="InvalidInningsCountDecimalException">Thrown if the partial innings is not valid</exception>
    private InningsCount(decimal value)
    {
        var fullInnings = Math.Truncate(value);
        var partialInnings = Round(value - fullInnings);

        // Partial inning value is already the actual value (1/3 or 2/3)
        if (ValidPartialInningShorthandMap.ContainsValue(partialInnings))
        {
            Value = Round(value);
            return;
        }

        // Need to convert from 0.1 to 1/3 innings or 0.2 to 2/3 innings
        if (!ValidPartialInningShorthandMap.TryGetValue(partialInnings, out var actualPartialInnings))
        {
            throw new InvalidInningsCountDecimalException(
                $"Invalid partial innings count: {partialInnings}. It can only be 0.0, 0.1 (1/3), or 0.2 (2/3)");
        }

        Value = Round(fullInnings + actualPartialInnings);
    }

    /// <summary>
    /// Creates <see cref="InningsCount"/>
    /// </summary>
    /// <param name="inningsCount">The number of innings</param>
    /// <returns><see cref="InningsCount"/></returns>
    public static InningsCount Create(decimal inningsCount)
    {
        return new InningsCount(inningsCount);
    }

    /// <summary>
    /// Creates <see cref="InningsCount"/>
    /// </summary>
    /// <param name="inningsCount">The number of innings</param>
    /// <returns><see cref="InningsCount"/></returns>
    public static InningsCount Create(string inningsCount)
    {
        if (!decimal.TryParse(inningsCount, out var innings))
        {
            throw new InvalidInningsCountDecimalException(
                $"Invalid partial innings count: {inningsCount}. It can only end in n.0, n.1 (1/3), or n.2 (2/3)");
        }

        return new InningsCount(innings);
    }

    /// <summary>
    /// Standard rounding for innings count
    /// </summary>
    /// <param name="inningsCount">The number of innings</param>
    /// <returns>Rounded innings count</returns>
    private static decimal Round(decimal inningsCount)
    {
        return Math.Round(inningsCount, 3, MidpointRounding.AwayFromZero);
    }
}