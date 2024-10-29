using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerActivation;

/// <summary>
/// Published when a Player is activated
/// </summary>
/// <param name="Year">The year the player was activated</param>
/// <param name="PlayerMlbId">The MLB ID of the player</param>
/// <param name="Date">The date</param>
public sealed record PlayerActivationEvent(SeasonYear Year, MlbId PlayerMlbId, DateOnly Date) : IForecastImpactEvent
{
    /// <inheritdoc />
    public CardExternalId? CardExternalId => null;

    /// <inheritdoc />
    public MlbId? MlbId => PlayerMlbId;
}