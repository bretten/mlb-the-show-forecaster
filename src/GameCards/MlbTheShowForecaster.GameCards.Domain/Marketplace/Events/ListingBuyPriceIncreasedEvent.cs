using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Events;

/// <summary>
/// Published when a <see cref="Listing"/>'s buy price increases by a significant amount
/// </summary>
/// <param name="CardId">The card ID from MLB The Show</param>
/// <param name="OriginalPrice">The original price</param>
/// <param name="NewPrice">The new price</param>
/// <param name="PercentageChange">The percentage change from the old to new price</param>
public sealed record ListingBuyPriceIncreasedEvent(
    CardId CardId,
    NaturalNumber OriginalPrice,
    NaturalNumber NewPrice,
    PercentageChange PercentageChange
) : IListingPriceChangedEvent;