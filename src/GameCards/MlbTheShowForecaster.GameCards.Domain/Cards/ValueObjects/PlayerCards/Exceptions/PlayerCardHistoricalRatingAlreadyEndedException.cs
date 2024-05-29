namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards.Exceptions;

/// <summary>
/// Thrown when <see cref="PlayerCardHistoricalRating.End"/> is called on a <see cref="PlayerCardHistoricalRating"/>
/// that has already ended
/// </summary>
public sealed class PlayerCardHistoricalRatingAlreadyEndedException : Exception
{
    public PlayerCardHistoricalRatingAlreadyEndedException(string? message) : base(message)
    {
    }
}