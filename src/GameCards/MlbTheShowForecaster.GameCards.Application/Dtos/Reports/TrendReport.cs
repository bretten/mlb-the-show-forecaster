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
/// <param name="Orders1H">The number of orders in the past hour</param>
/// <param name="Orders24H">The number of orders in the past 24 hours</param>
/// <param name="BuyPrice">The current buy price, or the highest bid</param>
/// <param name="BuyPriceChange24H">The percentage change of the buy price over the past 24 hours</param>
/// <param name="SellPrice">The current sell price, or the lowest ask</param>
/// <param name="SellPriceChange24H">The percentage change of the sell price over the past 24 hours</param>
/// <param name="Score">The player's performance score</param>
/// <param name="Score">The percentage change of the player's performance score over the past two weeks</param>
public readonly record struct TrendReport(
    SeasonYear Year,
    CardExternalId CardExternalId,
    MlbId MlbId,
    CardName CardName,
    Position PrimaryPosition,
    OverallRating OverallRating,
    IReadOnlyList<TrendMetricsByDate> MetricsByDate,
    IReadOnlyList<TrendImpact> Impacts,
    bool IsBoosted,
    int Orders1H,
    int Orders24H,
    int BuyPrice,
    decimal BuyPriceChange24H,
    int SellPrice,
    decimal SellPriceChange24H,
    decimal Score,
    decimal ScoreChange2W,
    int Demand);