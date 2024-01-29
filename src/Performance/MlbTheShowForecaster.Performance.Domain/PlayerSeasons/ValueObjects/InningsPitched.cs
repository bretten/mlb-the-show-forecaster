using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

/// <summary>
/// Innings pitched = IP
///
/// <para>The number of innings a pitcher has completed. When a pitcher makes one out,
/// it counts as 1/3 IP and two outs counts as 2/3 IP. These are written as 0.1 IP and
/// 0.2 IP respectively.</para>
/// </summary>
public sealed class InningsPitched : ValueObject
{
    /// <summary>
    /// Maps shorthand partial innings pitched to their actual values
    /// </summary>
    private static readonly Dictionary<decimal, decimal> ValidPartialInningShorthandMap = new()
    {
        { 0.0m, 0.0m },
        { 0.1m, Round((decimal)1 / 3) },
        { 0.2m, Round((decimal)2 / 3) },
    };

    /// <summary>
    /// The underlying value
    /// </summary>
    public decimal Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The underlying value</param>
    /// <exception cref="InvalidInningsPitchedDecimalException">Thrown if the partial innings pitched is not valid</exception>
    private InningsPitched(decimal value)
    {
        var fullInningsPitched = Math.Truncate(value);
        var partialInningsPitched = Round(value - fullInningsPitched);

        // Partial inning value is already the actual value (1/3 or 2/3)
        if (ValidPartialInningShorthandMap.ContainsValue(partialInningsPitched))
        {
            Value = Round(value);
            return;
        }

        // Need to convert from 0.1 to 1/3 IP or 0.2 to 2/3 IP
        if (!ValidPartialInningShorthandMap.TryGetValue(partialInningsPitched, out var actualPartialInningsPitched))
        {
            throw new InvalidInningsPitchedDecimalException(
                $"Invalid partial innings pitched: {partialInningsPitched}. It can only be 0.0, 0.1 (1/3 IP), or 0.2 (2/3 IP)");
        }

        Value = Round(fullInningsPitched + actualPartialInningsPitched);
    }

    /// <summary>
    /// Creates <see cref="InningsPitched"/>
    /// </summary>
    /// <param name="inningsPitched">The number of innings pitched</param>
    /// <returns><see cref="InningsPitched"/></returns>
    public static InningsPitched Create(decimal inningsPitched)
    {
        return new InningsPitched(inningsPitched);
    }

    /// <summary>
    /// Standard rounding for innings pitched
    /// </summary>
    /// <param name="inningsPitched">The number of innings pitched</param>
    /// <returns>Rounded innings pitched</returns>
    private static decimal Round(decimal inningsPitched)
    {
        return Math.Round(inningsPitched, 3, MidpointRounding.AwayFromZero);
    }
}