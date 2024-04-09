using System.ComponentModel.DataAnnotations;
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
    [Display(Name = "MlbCard")] MlbCard,

    /// <summary>
    /// A stadium card
    /// </summary>
    [Display(Name = "Stadium")] Stadium,

    /// <summary>
    /// An equipment card
    /// </summary>
    [Display(Name = "Equipment")] Equipment,

    /// <summary>
    /// A sponsorship card
    /// </summary>
    [Display(Name = "Sponsorship")] Sponsorship
}