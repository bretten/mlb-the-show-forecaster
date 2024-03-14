using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

/// <summary>
/// Represents the overall rating of a card
/// </summary>
public sealed class OverallRating : ValueObject
{
    /// <summary>
    /// The minimum allowed value
    /// </summary>
    private const int MinValue = 40;

    /// <summary>
    /// The maximum allowed value
    /// </summary>
    private const int MaxValue = 99;

    /// <summary>
    /// The underlying rating value
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">The rating</param>
    private OverallRating(int value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a <see cref="OverallRating"/>
    /// </summary>
    /// <param name="rating">The rating</param>
    /// <returns><see cref="OverallRating"/></returns>
    /// <exception cref="OverallRatingOutOfRangeException">Thrown if the specified rating value is not within the valid range</exception>
    public static OverallRating Create(int rating)
    {
        if (rating < MinValue || rating > MaxValue)
        {
            throw new OverallRatingOutOfRangeException(
                $"The overall rating of {rating} is not between {MinValue} and {MaxValue}");
        }

        return new OverallRating(rating);
    }
}