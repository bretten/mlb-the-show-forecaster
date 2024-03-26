using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

/// <summary>
/// Represents a Listing from the MLB The Show
/// </summary>
/// <param name="ListingName">The name of the Listing</param>
/// <param name="BestSellPrice">The current, best sell price</param>
/// <param name="BestBuyPrice">The current, best buy price</param>
/// <param name="CardExternalId">The external ID (MLB The Show UUID) of the card</param>
public readonly record struct CardListing(
    string ListingName,
    NaturalNumber BestSellPrice,
    NaturalNumber BestBuyPrice,
    CardExternalId CardExternalId
);