using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.EventStores;

/// <summary>
/// Defines an append-only event store for <see cref="Listing"/> prices and orders
/// </summary>
public interface IListingEventStore
{
    /// <summary>
    /// Appends new Listing prices and orders to the event store
    /// </summary>
    /// <param name="year">The year of the listing</param>
    /// <param name="cardListing"><see cref="CardListing"/> with prices and orders</param>
    /// <returns>The completed task</returns>
    Task AppendNewPricesAndOrders(SeasonYear year, CardListing cardListing);

    /// <summary>
    /// Polls the event store for any prices that have not yet been consumed yet
    /// </summary>
    /// <param name="year">The year of the listing</param>
    /// <param name="count">The number of entries to poll</param>
    /// <returns><see cref="NewPriceEvents"/></returns>
    Task<NewPriceEvents> PollNewPrices(SeasonYear year, int count);

    /// <summary>
    /// Polls the event store for any orders that have not yet been consumed yet
    /// </summary>
    /// <param name="year">The year of the listing</param>
    /// <param name="count">The number of entries to poll</param>
    /// <returns><see cref="NewOrderEvents"/></returns>
    Task<NewOrderEvents> PollNewOrders(SeasonYear year, int count);

    /// <summary>
    /// Checkpoints the last price that was consumed in the event store
    /// </summary>
    /// <param name="year">The year of the listing</param>
    /// <param name="lastAcknowledgedId">The ID of the last acknowledged price</param>
    /// <returns>The completed task</returns>
    Task AcknowledgePrices(SeasonYear year, string lastAcknowledgedId);

    /// <summary>
    /// Checkpoints the last order that was consumed in the event store
    /// </summary>
    /// <param name="year">The year of the listing</param>
    /// <param name="lastAcknowledgedId">The ID of the last acknowledged order</param>
    /// <returns>The completed task</returns>
    Task AcknowledgeOrders(SeasonYear year, string lastAcknowledgedId);

    /// <summary>
    /// Gets the most recent state of a Listing without rebuilding it from the event store's history or progressing
    /// the checkpoint
    /// </summary>
    /// <param name="year">The year of the listing</param>
    /// <param name="cardExternalId">The <see cref="CardExternalId"/> to get the price for</param>
    /// <returns><see cref="CardListing"/></returns>
    Task<CardListing> PeekListing(SeasonYear year, CardExternalId cardExternalId);
}