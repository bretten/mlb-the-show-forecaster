using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerCardBoosted;

/// <summary>
/// Raised when a <see cref="PlayerCard"/> gets a significant rating and attribute increase
/// </summary>
/// <param name="Year">The year of MLB The Show</param>
/// <param name="CardExternalId">The card ID from MLB The Show</param>
/// <param name="NewOverallRating">The new overall rating</param>
/// <param name="BoostReason">The reason the card is being boosted</param>
/// <param name="BoostEndDate">The end date of the boost</param>
/// <param name="Date">The date</param>
public sealed record PlayerCardBoostEvent(
    SeasonYear Year,
    CardExternalId CardExternalId,
    OverallRating NewOverallRating,
    string BoostReason,
    DateOnly BoostEndDate,
    DateOnly Date) : IForecastImpactEvent
{
    /// <inheritdoc />
    public MlbId? MlbId => null;
}