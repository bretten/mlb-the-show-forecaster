using System.ComponentModel.DataAnnotations;
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
    [Display(Name = "Diamond")] Diamond,

    /// <summary>
    /// Gold
    /// </summary>
    [Display(Name = "Gold")] Gold,

    /// <summary>
    /// Silver
    /// </summary>
    [Display(Name = "Silver")] Silver,

    /// <summary>
    /// Bronze
    /// </summary>
    [Display(Name = "Bronze")] Bronze,

    /// <summary>
    /// Common, the least rare
    /// </summary>
    [Display(Name = "Common")] Common
}