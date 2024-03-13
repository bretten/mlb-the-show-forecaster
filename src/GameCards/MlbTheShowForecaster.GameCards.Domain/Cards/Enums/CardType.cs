using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;

/// <summary>
/// Represents the different types of <see cref="Card"/>s
/// </summary>
public enum CardType
{
    /// <summary>
    /// A player card
    /// </summary>
    MlbCard,

    /// <summary>
    /// A stadium card
    /// </summary>
    Stadium,

    /// <summary>
    /// An equipment card
    /// </summary>
    Equipment,

    /// <summary>
    /// A sponsorship card
    /// </summary>
    Sponsorship
}