using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

/// <summary>
/// Represents a previous listing price in MLB The Show
/// </summary>
/// <param name="Date">The date of the prices</param>
/// <param name="BestSellPrice">The best sell price for the day</param>
/// <param name="BestBuyPrice">The best buy price for the day</param>
public readonly record struct CardListingPrice(
    DateOnly Date,
    NaturalNumber BestSellPrice,
    NaturalNumber BestBuyPrice
);