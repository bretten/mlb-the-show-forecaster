﻿using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Repositories;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreateListing;

/// <summary>
/// Handles a <see cref="CreateListingCommand"/>
///
/// <para>Creates a new <see cref="Listing"/> by adding it to the repository and wrapping the whole command as a
/// single unit of work</para>
/// </summary>
internal sealed class CreateListingCommandHandler : ICommandHandler<CreateListingCommand>
{
    /// <summary>
    /// The unit of work that encapsulates all actions for creating a <see cref="Listing"/>
    /// </summary>
    private readonly IUnitOfWork<IMarketplaceWork> _unitOfWork;

    /// <summary>
    /// Maps listing data from an external source to <see cref="Listing"/>
    /// </summary>
    private readonly IListingMapper _listingMapper;

    /// <summary>
    /// The <see cref="Listing"/> repository
    /// </summary>
    private readonly IListingRepository _listingRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="unitOfWork">The unit of work that encapsulates all actions for creating a <see cref="Listing"/></param>
    /// <param name="listingMapper">Maps listing data from an external source to <see cref="Listing"/></param>
    public CreateListingCommandHandler(IUnitOfWork<IMarketplaceWork> unitOfWork, IListingMapper listingMapper)
    {
        _unitOfWork = unitOfWork;
        _listingMapper = listingMapper;
        _listingRepository = unitOfWork.GetContributor<IListingRepository>();
    }

    /// <summary>
    /// Creates a <see cref="Listing"/> and adds it to the domain
    /// </summary>
    /// <param name="command">The command</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    public async Task Handle(CreateListingCommand command, CancellationToken cancellationToken = default)
    {
        var listing = _listingMapper.Map(command.ExternalCardListing);
        await _listingRepository.Add(listing, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}