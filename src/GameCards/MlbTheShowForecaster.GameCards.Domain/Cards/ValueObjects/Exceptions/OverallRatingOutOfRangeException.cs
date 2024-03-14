namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

/// <summary>
/// Thrown when <see cref="OverallRating"/> is provided a value outside of the expected range
/// </summary>
public sealed class OverallRatingOutOfRangeException : Exception
{
    public OverallRatingOutOfRangeException(string? message) : base(message)
    {
    }
}