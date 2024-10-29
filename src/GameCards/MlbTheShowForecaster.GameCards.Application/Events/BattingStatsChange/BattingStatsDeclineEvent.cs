using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Events.BattingStatsChange;

/// <summary>
/// Represents a decline in an entity's batting stats
/// </summary>
/// <param name="Year">The season of the stats change</param>
/// <param name="MlbId">The <see cref="Common.Domain.ValueObjects.MlbId"/> of the entity who had the stats change</param>
/// <param name="Comparison">A comparison of the previous and new stats</param>
/// <param name="Date">The date</param>
public sealed record BattingStatsDeclineEvent(SeasonYear Year, MlbId MlbId, PercentageChange Comparison, DateOnly Date)
    : IForecastImpactEvent
{
    /// <inheritdoc />
    public CardExternalId? CardExternalId => null;
}