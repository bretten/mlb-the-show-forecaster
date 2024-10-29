using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PositionChange;

/// <summary>
/// Raised when a <see cref="PlayerCard"/> is assigned a new fielding <see cref="Position"/>
/// </summary>
/// <param name="Year">The year of MLB The Show</param>
/// <param name="CardExternalId">The card ID from MLB The Show</param>
/// <param name="NewPosition">The new fielding position</param>
/// <param name="OldPosition">The old fielding position</param>
/// <param name="Date">The date</param>
public sealed record PositionChangeEvent(
    SeasonYear Year,
    CardExternalId CardExternalId,
    Position NewPosition,
    Position OldPosition,
    DateOnly Date) : IForecastImpactEvent
{
    /// <inheritdoc />
    public MlbId? MlbId => null;
}