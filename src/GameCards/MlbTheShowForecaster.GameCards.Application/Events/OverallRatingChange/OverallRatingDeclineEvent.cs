using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.OverallRatingChange;

/// <summary>
/// Raised when a <see cref="PlayerCard"/>'s <see cref="OverallRating"/> declines
/// </summary>
/// <param name="Year">The year of MLB The Show</param>
/// <param name="CardExternalId">The card ID from MLB The Show</param>
/// <param name="NewOverallRating">The new overall rating</param>
/// <param name="PreviousOverallRating">The previous overall rating being replaced</param>
/// <param name="Date">The date</param>
public sealed record OverallRatingDeclineEvent(
    SeasonYear Year,
    CardExternalId CardExternalId,
    OverallRating NewOverallRating,
    OverallRating PreviousOverallRating,
    DateOnly Date) : IForecastImpactEvent
{
    /// <inheritdoc />
    public MlbId? MlbId => null;
}