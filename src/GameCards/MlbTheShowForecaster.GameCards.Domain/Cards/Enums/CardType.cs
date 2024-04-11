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
    [Display(Name = "mlb_card")] MlbCard,

    /// <summary>
    /// A stadium card
    /// </summary>
    [Display(Name = "stadium")] Stadium,

    /// <summary>
    /// An equipment card
    /// </summary>
    [Display(Name = "equipment")] Equipment,

    /// <summary>
    /// A sponsorship card
    /// </summary>
    [Display(Name = "sponsorship")] Sponsorship
}