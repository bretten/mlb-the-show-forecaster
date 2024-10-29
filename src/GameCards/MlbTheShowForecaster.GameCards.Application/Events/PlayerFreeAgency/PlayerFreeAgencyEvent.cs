using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerFreeAgency;

/// <summary>
/// Published when a Player enters free agency
/// </summary>
/// <param name="Year">The year the player entered free agency</param>
/// <param name="PlayerMlbId">The MLB ID of the player</param>
/// <param name="Date">The date</param>
public sealed record PlayerFreeAgencyEvent(SeasonYear Year, MlbId PlayerMlbId, DateOnly Date) : IForecastImpactEvent
{
    /// <inheritdoc />
    public CardExternalId? CardExternalId => null;

    /// <inheritdoc />
    public MlbId? MlbId => PlayerMlbId;
}