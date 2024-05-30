using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using com.brettnamba.MlbTheShowForecaster.Common.Converters;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;

/// <summary>
/// Represents the different reasons for a <see cref="PlayerCardHistoricalRating"/>
/// </summary>
[TypeConverter(typeof(EnumDisplayNameConverter))]
public enum PlayerCardHistoricalRatingType
{
    /// <summary>
    /// A player card had a change to its baseline rating and attributes
    /// </summary>
    [Display(Name = "BASE")] Baseline,

    /// <summary>
    /// A player card had a temporary change to its rating and attributes
    /// </summary>
    [Display(Name = "TEMP")] Temporary,

    /// <summary>
    /// A player card had a significant, temporary increase to its rating and attributes
    /// </summary>
    [Display(Name = "BOOS")] Boost
}