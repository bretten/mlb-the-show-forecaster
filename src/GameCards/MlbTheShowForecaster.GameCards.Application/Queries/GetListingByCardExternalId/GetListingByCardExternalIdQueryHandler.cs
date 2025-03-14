﻿using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Repositories;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetListingByCardExternalId;

/// <summary>
/// Handles a <see cref="GetListingByCardExternalIdQuery"/>
///
/// <para>Gets a <see cref="Listing"/> by its <see cref="CardExternalId"/></para>
/// </summary>
internal sealed class GetListingByCardExternalIdQueryHandler : IQueryHandler<GetListingByCardExternalIdQuery, Listing?>
{
    /// <summary>
    /// The <see cref="Listing"/> repository
    /// </summary>
    private readonly IListingRepository _listingRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="unitOfWork">The unit of work that encapsulates all actions for getting a <see cref="Listing"/></param>
    public GetListingByCardExternalIdQueryHandler(IUnitOfWork<IMarketplaceWork> unitOfWork)
    {
        _listingRepository = unitOfWork.GetContributor<IListingRepository>();
    }

    /// <summary>
    /// Gets a <see cref="Listing"/> by the specified <see cref="CardExternalId"/>
    /// </summary>
    /// <param name="query">The query</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The <see cref="Listing"/> that matches the specified <see cref="CardExternalId"/></returns>
    public async Task<Listing?> Handle(GetListingByCardExternalIdQuery query, CancellationToken cancellationToken)
    {
        return await _listingRepository.GetByExternalId(query.Year, query.CardExternalId, query.IncludeRelated,
            cancellationToken);
    }
}