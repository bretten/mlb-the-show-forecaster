using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

/// <summary>
/// Represents a MLB statistic that is calculated from multiple individual statistics in batting, pitching or fielding.
/// Since the calculation components are countable scores such as hits and home runs, the underlying value is a decimal.
/// </summary>
public abstract class CalculatedStat : ValueObject
{
    /// <summary>
    /// The underlying value of the stat
    /// </summary>
    public decimal Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The stat value</param>
    protected CalculatedStat(decimal value)
    {
        if (value < 0)
        {
            throw new CalculatedStatBelowZeroException($"Calculated stat must not be negative but {value} was given");
        }

        Value = value;
    }

    /// <summary>
    /// Returns the value rounded to the specified number of decimals
    /// </summary>
    /// <param name="decimals">The number of decimals in the return value</param>
    /// <returns>The rounded value</returns>
    public decimal AsRounded(int decimals = 3)
    {
        return Math.Round(Value, decimals, MidpointRounding.AwayFromZero);
    }
}