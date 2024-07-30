using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using com.brettnamba.MlbTheShowForecaster.Common.Converters;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;

/// <summary>
/// Represents the rarity of a <see cref="Card"/>
/// </summary>
[TypeConverter(typeof(EnumDisplayNameConverter))]
public enum Rarity
{
    /// <summary>
    /// Diamond, the most rare
    /// </summary>
    [Display(Name = "diamond")] Diamond = 4,

    /// <summary>
    /// Gold
    /// </summary>
    [Display(Name = "gold")] Gold = 3,

    /// <summary>
    /// Silver
    /// </summary>
    [Display(Name = "silver")] Silver = 2,

    /// <summary>
    /// Bronze
    /// </summary>
    [Display(Name = "bronze")] Bronze = 1,

    /// <summary>
    /// Common, the least rare
    /// </summary>
    [Display(Name = "common")] Common = 0
}