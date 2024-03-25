using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;

/// <summary>
/// Represents the different types of <see cref="Card"/> series
/// </summary>
public enum CardSeries
{
    /// <summary>
    /// A series of cards for current MLB players
    /// </summary>
    Live = 1337,

    /// <summary>
    /// A series of cards for MLB players in their rookie year
    /// </summary>
    Rookie = 10001
}