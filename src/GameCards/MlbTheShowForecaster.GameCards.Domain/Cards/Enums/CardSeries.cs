using System.ComponentModel.DataAnnotations;
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
    [Display(Name = "Live")] Live = 1337,

    /// <summary>
    /// A series of cards for MLB players in their rookie year
    /// </summary>
    [Display(Name = "Rookie")] Rookie = 10001
}