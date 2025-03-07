using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.EventStores;

/// <summary>
/// New price events that have yet to be consumed by the application
/// </summary>
/// <param name="Checkpoint">The new checkpoint to use if these new prices are consumed</param>
/// <param name="Prices">The new prices</param>
public sealed record NewPriceEvents(string Checkpoint, Dictionary<ListingHistoricalPrice, CardExternalId> Prices);