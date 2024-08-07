using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.PlayerTeamSigning;

/// <summary>
/// Published when a Player signs a contract with a Team
/// </summary>
/// <param name="Year">The year the player signed with the team</param>
/// <param name="PlayerMlbId">The MLB ID of the player</param>
public sealed record PlayerTeamSigningEvent(SeasonYear Year, MlbId PlayerMlbId) : IForecastImpactEvent
{
    /// <inheritdoc />
    public CardExternalId? CardExternalId => null;

    /// <inheritdoc />
    public MlbId? MlbId => PlayerMlbId;
}