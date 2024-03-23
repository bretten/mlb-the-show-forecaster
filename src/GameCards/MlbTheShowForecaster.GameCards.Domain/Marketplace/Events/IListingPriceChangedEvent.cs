using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Events;

/// <summary>
/// Defines what a <see cref="Listing"/> price change <see cref="IDomainEvent"/> should be
/// </summary>
public interface IListingPriceChangedEvent : IDomainEvent
{
    /// <summary>
    /// The card ID from MLB The Show
    /// </summary>
    CardExternalId CardExternalId { get; }

    /// <summary>
    /// The original price
    /// </summary>
    NaturalNumber OriginalPrice { get; }

    /// <summary>
    /// The new price
    /// </summary>
    NaturalNumber NewPrice { get; }

    /// <summary>
    /// The percentage change from the old to new price
    /// </summary>
    PercentageChange PercentageChange { get; }
}