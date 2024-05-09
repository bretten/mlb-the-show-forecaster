using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;

namespace com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

/// <summary>
/// Represents the percentage change from a <see cref="ReferenceValue"/> to a <see cref="NewValue"/> expressed on the
/// 100% scale
///
/// <para>The formula is PercentageChange = 100 * (NewValue - ReferenceValue) / ReferenceValue</para>
/// </summary>
public class PercentageChange : ValueObject
{
    /// <summary>
    /// The underlying percentage change
    /// </summary>
    private decimal? _percentageChange;

    /// <summary>
    /// The reference value that the percentage change will be calculated with respect to
    /// </summary>
    public decimal ReferenceValue { get; }

    /// <summary>
    /// The new value that determines the percentage change with respect to the <see cref="ReferenceValue"/>
    /// </summary>
    public decimal NewValue { get; }

    /// <summary>
    /// The number of fractional digits to round the decimal value to
    /// </summary>
    protected virtual int FractionalDigitCount => 2;

    /// <summary>
    /// True if a <see cref="ReferenceValue"/> of zero should be treated as one to prevent <see cref="DivideByZeroException"/>
    /// </summary>
    protected virtual bool TreatZeroReferenceValueAsOne { get; }

    /// <summary>
    /// The percentage change from the <see cref="ReferenceValue"/> to the <see cref="NewValue"/> on the 100% scale
    ///
    /// <para>In other words, a value of 0.1 is a 0.1% change and a value of 10 is a 10% change</para>
    /// </summary>
    public decimal PercentageChangeValue
    {
        get
        {
            if (!_percentageChange.HasValue)
            {
                _percentageChange = Math.Round(CalculatePercentageChange(), FractionalDigitCount,
                    MidpointRounding.AwayFromZero);
            }

            return _percentageChange.Value;
        }
    }

    /// <summary>
    /// Checks if the percentage change from <see cref="ReferenceValue"/> to <see cref="NewValue"/> was an increase
    /// and if its magnitude is greater than the specified amount
    /// </summary>
    /// <param name="threshold">The percentage change increase amount threshold</param>
    /// <returns>True if it increased by the specified amount, otherwise false</returns>
    public bool HasIncreasedBy(decimal threshold)
    {
        return PercentageChangeValue > 0 && PercentageChangeValue >= Math.Abs(threshold);
    }

    /// <summary>
    /// Checks if the percentage change from <see cref="ReferenceValue"/> to <see cref="NewValue"/> was a decrease
    /// and if its magnitude is greater than the specified amount
    /// </summary>
    /// <param name="threshold">The percentage change decrease amount threshold</param>
    /// <returns>True if it decreased by the specified amount, otherwise false</returns>
    public bool HasDecreasedBy(decimal threshold)
    {
        return PercentageChangeValue < 0 && Math.Abs(PercentageChangeValue) >= Math.Abs(threshold);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="referenceValue">The reference value that the percentage change will be calculated with respect to</param>
    /// <param name="newValue">The new value that determines the percentage change with respect to the <see cref="ReferenceValue"/></param>
    /// <param name="treatZeroReferenceValueAsOne">True if a <see cref="ReferenceValue"/> of zero should be treated as one to prevent <see cref="DivideByZeroException"/></param>
    protected PercentageChange(decimal referenceValue, decimal newValue, bool treatZeroReferenceValueAsOne = false)
    {
        ReferenceValue = referenceValue;
        NewValue = newValue;
        TreatZeroReferenceValueAsOne = treatZeroReferenceValueAsOne;
    }

    /// <summary>
    /// Calculates the percentage change from the <see cref="ReferenceValue"/> to the <see cref="NewValue"/> with
    /// the formula:
    /// <para>PercentageChange = 100 * (NewValue - ReferenceValue) / ReferenceValue</para>
    /// </summary>
    private decimal CalculatePercentageChange()
    {
        var denominator = TreatZeroReferenceValueAsOne && ReferenceValue == 0 ? 1 : ReferenceValue;
        return 100 * ((NewValue - ReferenceValue) / denominator);
    }

    /// <summary>
    /// Creates a <see cref="PercentageChange"/>
    /// </summary>
    /// <param name="referenceValue">The reference value that the percentage change will be calculated with respect to</param>
    /// <param name="newValue">The new value that determines the percentage change with respect to the <see cref="ReferenceValue"/></param>
    /// <param name="treatZeroReferenceValueAsOne">True if a <see cref="ReferenceValue"/> of zero should be treated as one to prevent <see cref="DivideByZeroException"/></param>
    /// <returns><see cref="PercentageChange"/></returns>
    public static PercentageChange Create(decimal referenceValue, decimal newValue,
        bool treatZeroReferenceValueAsOne = false)
    {
        return new PercentageChange(referenceValue, newValue, treatZeroReferenceValueAsOne);
    }

    /// <summary>
    /// Creates a <see cref="PercentageChange"/>
    /// </summary>
    /// <param name="referenceValue">The reference value that the percentage change will be calculated with respect to</param>
    /// <param name="newValue">The new value that determines the percentage change with respect to the <see cref="ReferenceValue"/></param>
    /// <param name="treatZeroReferenceValueAsOne">True if a <see cref="ReferenceValue"/> of zero should be treated as one to prevent <see cref="DivideByZeroException"/></param>
    /// <returns><see cref="PercentageChange"/></returns>
    public static PercentageChange Create(NaturalNumber referenceValue, NaturalNumber newValue,
        bool treatZeroReferenceValueAsOne = false)
    {
        return new PercentageChange(referenceValue.Value, newValue.Value, treatZeroReferenceValueAsOne);
    }
}