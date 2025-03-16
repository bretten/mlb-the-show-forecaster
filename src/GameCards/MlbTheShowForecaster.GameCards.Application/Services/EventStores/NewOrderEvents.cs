using System.Collections.ObjectModel;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.EventStores;

/// <summary>
/// New order events that have yet to be consumed by the application
/// </summary>
/// <param name="Checkpoint">The new checkpoint to use if these new orders are consumed</param>
/// <param name="Orders">The new orders</param>
public sealed record NewOrderEvents(
    string Checkpoint,
    Dictionary<CardExternalId, ReadOnlyCollection<ListingOrder>> Orders);