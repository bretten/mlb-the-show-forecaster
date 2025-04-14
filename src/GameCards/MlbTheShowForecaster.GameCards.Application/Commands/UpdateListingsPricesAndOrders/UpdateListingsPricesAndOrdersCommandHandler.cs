using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.EventStores;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdateListingsPricesAndOrders;

/// <summary>
/// Handles a <see cref="UpdateListingsPricesAndOrdersCommand"/>
///
/// <para>Batch inserts <see cref="ListingHistoricalPrice"/>s and <see cref="ListingOrder"/>s for the specified
/// <see cref="Listing"/>s</para>
/// </summary>
internal sealed class
    UpdateListingsPricesAndOrdersCommandHandler : ICommandHandler<UpdateListingsPricesAndOrdersCommand>
{
    /// <summary>
    /// The event store containing new prices and orders
    /// </summary>
    private readonly IListingEventStore _eventStore;

    /// <summary>
    /// The <see cref="Listing"/> repository
    /// </summary>
    private readonly IListingRepository _repository;

    /// <summary>
    /// Writes listings data to a data sink
    /// </summary>
    private readonly IListingDataSink _dataSink;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="eventStore">The event store containing new prices and orders</param>
    /// <param name="repository">The <see cref="Listing"/> repository</param>
    /// <param name="dataSink">Writes listings data to a data sink</param>
    public UpdateListingsPricesAndOrdersCommandHandler(IListingEventStore eventStore, IListingRepository repository,
        IListingDataSink dataSink)
    {
        _eventStore = eventStore;
        _repository = repository;
        _dataSink = dataSink;
    }

    /// <summary>
    /// Polls the event store for new Listing prices and orders and then batch inserts them
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Handle(UpdateListingsPricesAndOrdersCommand command,
        CancellationToken cancellationToken = default)
    {
        // Prices
        var newPriceEvents = await _eventStore.PollNewPrices(command.Year, command.BatchSize);
        await _repository.Add(command.Listings, newPriceEvents.Prices, cancellationToken);
        await _eventStore.AcknowledgePrices(command.Year, newPriceEvents.Checkpoint);

        // Orders
        var newOrderEvents = await _eventStore.PollNewOrders(command.Year, command.BatchSize);
        await _repository.Add(command.Listings, newOrderEvents.Orders, cancellationToken);
        await _eventStore.AcknowledgeOrders(command.Year, newOrderEvents.Checkpoint);

        // Orders data sink
        await _dataSink.Write(command.Year, command.BatchSize);
    }
}