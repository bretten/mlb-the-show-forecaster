using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdateListing.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Repositories;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdateListing;

/// <summary>
/// Handles a <see cref="UpdateListingCommand"/>
///
/// <para>Updates a <see cref="Listing"/> by logging any new historical prices for the Listing and updating its
/// current prices</para>
/// </summary>
internal sealed class UpdateListingCommandHandler : ICommandHandler<UpdateListingCommand>
{
    /// <summary>
    /// The unit of work that encapsulates all actions for updating a <see cref="Listing"/>
    /// </summary>
    private readonly IUnitOfWork<IMarketplaceWork> _unitOfWork;

    /// <summary>
    /// Publishes all domain events that were raised by the <see cref="Listing"/>
    /// </summary>
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    /// <summary>
    /// The <see cref="Listing"/> repository
    /// </summary>
    private readonly IListingRepository _listingRepository;

    /// <summary>
    /// Maps incoming DTOs to the <see cref="Listing"/> and related entities
    /// </summary>
    private readonly IListingMapper _listingMapper;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="unitOfWork">The unit of work that encapsulates all actions for updating a <see cref="Listing"/></param>
    /// <param name="domainEventDispatcher">Publishes all domain events that were raised by the <see cref="Listing"/></param>
    /// <param name="listingMapper">Maps incoming DTOs to the <see cref="Listing"/> and related entities</param>
    public UpdateListingCommandHandler(IUnitOfWork<IMarketplaceWork> unitOfWork,
        IDomainEventDispatcher domainEventDispatcher, IListingMapper listingMapper)
    {
        _unitOfWork = unitOfWork;
        _domainEventDispatcher = domainEventDispatcher;
        _listingMapper = listingMapper;
        _listingRepository = unitOfWork.GetContributor<IListingRepository>();
    }

    /// <summary>
    /// Updates an existing <see cref="Listing"/> in the domain by logging any new historical prices and updating the
    /// current prices
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Handle(UpdateListingCommand command, CancellationToken cancellationToken = default)
    {
        // The domain Listing that is being updated
        var domainListing = command.DomainListing;
        if (domainListing == null)
        {
            throw new ListingNotFoundException(
                $"{nameof(Listing)} not found for CardExternalId {command.ExternalCardListing.CardExternalId.Value}");
        }

        // The up-to-date listing data from the external source
        var externalCardListing = command.ExternalCardListing;

        // Get new historical prices that are in the external source but not the domain Listing
        var newHistoricalPrices = externalCardListing.GetNewHistoricalPrices(domainListing);
        foreach (var newHistoricalPrice in newHistoricalPrices)
        {
            domainListing.LogHistoricalPrice(newHistoricalPrice.Date, buyPrice: newHistoricalPrice.BestBuyPrice,
                sellPrice: newHistoricalPrice.BestSellPrice);
        }

        // Update orders
        var newOrders = externalCardListing.GetNewOrders(domainListing);
        domainListing.UpdateOrders(newOrders.Select(_listingMapper.Map).ToList());

        // Update the domain Listing's current prices
        domainListing.UpdatePrices(newBuyPrice: externalCardListing.BestBuyPrice,
            newSellPrice: externalCardListing.BestSellPrice, command.PriceChangeThreshold);

        await _listingRepository.Update(domainListing, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        _domainEventDispatcher.Dispatch(domainListing.DomainEvents);
    }
}