using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

/// <summary>
/// Represents a natural number that includes 0 (0, 1, 2, 3, ...). To be used with countable stats like home runs
/// </summary>
public sealed class NaturalNumber : ValueObject
{
    /// <summary>
    /// The underlying natural number value (includes zero)
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The natural number</param>
    private NaturalNumber(int value)
    {
        if (value < 0)
        {
            throw new NaturalNumberCannotBeLessThanZeroException(
                $"Natural number must not be less than zero. {value} was given");
        }

        Value = value;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The natural number</param>
    private NaturalNumber(long value) : this((int)value)
    {
        if (value < 0)
        {
            throw new NaturalNumberCannotBeLessThanZeroException(
                $"Natural number must not be less than zero. {value} was given");
        }
    }

    /// <summary>
    /// Creates a <see cref="NaturalNumber"/>
    /// </summary>
    /// <param name="value">The underlying value</param>
    /// <returns>The <see cref="NaturalNumber"/></returns>
    public static NaturalNumber Create(int value)
    {
        return new NaturalNumber(value);
    }

    /// <summary>
    /// Creates a <see cref="NaturalNumber"/>
    /// </summary>
    /// <param name="value">The underlying value</param>
    /// <returns>The <see cref="NaturalNumber"/></returns>
    public static NaturalNumber Create(long value)
    {
        if (value > int.MaxValue)
        {
            throw new NaturalNumberOverflowException(
                $"A number larger than int.MaxValue was provided and no earthly, relevant stat should reach this number");
        }

        return new NaturalNumber(value);
    }
}