using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Events;

/// <summary>
/// Published when a <see cref="Listing"/>'s sell price decreases by a significant amount
/// </summary>
/// <param name="OriginalPrice">The original price</param>
/// <param name="NewPrice">The new price</param>
/// <param name="PercentageChange">The percentage change from the old to new price</param>
public record ListingSellPriceDecreasedEvent(
    NaturalNumber OriginalPrice,
    NaturalNumber NewPrice,
    PercentageChange PercentageChange
) : IDomainEvent;