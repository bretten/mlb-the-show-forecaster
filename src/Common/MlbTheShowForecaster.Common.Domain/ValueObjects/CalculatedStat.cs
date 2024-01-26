using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

/// <summary>
/// Represents a MLB statistic that is calculated from multiple individual statistics in batting, pitching or fielding.
/// Since the calculation components are countable scores such as hits and home runs, the underlying value is a decimal.
/// </summary>
public abstract class CalculatedStat : ValueObject
{
    /// <summary>
    /// Holds the underlying value of the stat after it is calculated
    /// </summary>
    private decimal? _value;

    /// <summary>
    /// The underlying value of the stat
    /// </summary>
    public decimal Value
    {
        get
        {
            if (!_value.HasValue)
            {
                _value = Calculate();
            }

            return _value.Value;
        }
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

    /// <summary>
    /// Calculates the stat
    /// </summary>
    /// <returns>The stat</returns>
    protected abstract decimal Calculate();
}