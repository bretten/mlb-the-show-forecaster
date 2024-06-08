using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;

/// <summary>
/// Represents a score from <see cref="IPerformanceAssessor"/>
/// <para>The value is a normalized score on a scale of [0, 1], with 1 being the best possible score.</para>
/// </summary>
public sealed class PerformanceScore : ValueObject
{
    /// <summary>
    /// The raw score value
    /// </summary>
    private readonly decimal _value;

    /// <summary>
    /// The underlying score value
    /// </summary>
    public decimal Value => Math.Round(_value, FractionalDigitCount, MidpointRounding.AwayFromZero);

    /// <summary>
    /// The number of fractional digits to round the decimal value to
    /// </summary>
    private const int FractionalDigitCount = 4;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The underlying score value</param>
    /// <exception cref="InvalidPerformanceScoreException">Thrown when the specified value is out of range</exception>
    private PerformanceScore(decimal value)
    {
        if (value < 0 || value > 1)
        {
            throw new InvalidPerformanceScoreException($"Invalid {nameof(PerformanceScore)}: {value}");
        }

        _value = value;
    }

    /// <summary>
    /// Creates a <see cref="PerformanceScore"/>
    /// </summary>
    /// <param name="score">The score</param>
    /// <returns><see cref="PerformanceScore"/></returns>
    public static PerformanceScore Create(decimal score)
    {
        return new PerformanceScore(score);
    }
}