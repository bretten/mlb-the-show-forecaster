using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdateListingsPricesAndOrders;

/// <summary>
/// Command that updates prices and orders for the specified <see cref="Listing"/>s
/// </summary>
/// <param name="Year">The year that the <see cref="Listings"/> belong to</param>
/// <param name="Listings">The <see cref="Listing"/>s to update prices and orders for</param>
/// <param name="BatchSize">The number of prices and orders to update at once</param>
internal readonly record struct UpdateListingsPricesAndOrdersCommand(
    SeasonYear Year,
    Dictionary<CardExternalId, Listing> Listings,
    int BatchSize) : ICommand;