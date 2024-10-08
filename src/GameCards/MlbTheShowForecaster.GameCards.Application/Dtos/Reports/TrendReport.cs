using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;

/// <summary>
/// Examines a player's stat trends by day
/// </summary>
/// <param name="Year">The year of MLB The Show</param>
/// <param name="CardExternalId">The card ID from MLB The Show</param>
/// <param name="MlbId">The MLB ID of the player</param>
/// <param name="CardName">The player name</param>
/// <param name="PrimaryPosition">Player's primary position</param>
/// <param name="OverallRating">The overall rating of the card</param>
/// <param name="MetricsByDate">The player's stat trends by date</param>
/// <param name="Impacts">Any occurrences that may have influenced the stat trends</param>
public readonly record struct TrendReport(
    SeasonYear Year,
    CardExternalId CardExternalId,
    MlbId MlbId,
    CardName CardName,
    Position PrimaryPosition,
    OverallRating OverallRating,
    IReadOnlyList<TrendMetricsByDate> MetricsByDate,
    IReadOnlyList<TrendImpact> Impacts);