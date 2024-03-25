using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;

/// <summary>
/// Represents the rarity of a <see cref="Card"/>
/// </summary>
public enum Rarity
{
    /// <summary>
    /// Diamond, the most rare
    /// </summary>
    Diamond,

    /// <summary>
    /// Gold
    /// </summary>
    Gold,

    /// <summary>
    /// Silver
    /// </summary>
    Silver,

    /// <summary>
    /// Bronze
    /// </summary>
    Bronze,

    /// <summary>
    /// Common, the least rare
    /// </summary>
    Common
}