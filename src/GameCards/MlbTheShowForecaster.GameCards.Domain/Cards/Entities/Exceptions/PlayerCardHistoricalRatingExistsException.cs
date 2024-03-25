namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities.Exceptions;

/// <summary>
/// Thrown when a <see cref="PlayerCard"/> already has a historical rating for a specific date
/// </summary>
public sealed class PlayerCardHistoricalRatingExistsException : Exception
{
    public PlayerCardHistoricalRatingExistsException(string? message) : base(message)
    {
    }
}