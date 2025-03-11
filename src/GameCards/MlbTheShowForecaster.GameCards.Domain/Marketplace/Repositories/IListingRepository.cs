using System.Collections.ObjectModel;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Repositories;

/// <summary>
/// Defines a repository for <see cref="Listing"/>
/// </summary>
public interface IListingRepository
{
    /// <summary>
    /// Should add a <see cref="Listing"/>
    /// </summary>
    /// <param name="listing">The <see cref="Listing"/> to add</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    Task Add(Listing listing, CancellationToken cancellationToken);

    /// <summary>
    /// Should update a <see cref="Listing"/>
    /// </summary>
    /// <param name="listing">The <see cref="Listing"/> to update</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    Task Update(Listing listing, CancellationToken cancellationToken);

    /// <summary>
    /// Should return a <see cref="Listing"/> for the specified <see cref="CardExternalId"/>
    /// </summary>
    /// <param name="externalId">The <see cref="CardExternalId"/> of the <see cref="Listing"/></param>
    /// <param name="includeRelated">True to include associated prices and orders, otherwise false</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The corresponding <see cref="Listing"/></returns>
    Task<Listing?> GetByExternalId(CardExternalId externalId, bool includeRelated, CancellationToken cancellationToken);

    /// <summary>
    /// Should add the specified <see cref="ListingHistoricalPrice"/>s
    /// </summary>
    /// <param name="listings">The <see cref="Listing"/>s that the <see cref="ListingHistoricalPrice"/>s belong to</param>
    /// <param name="prices">The prices to add</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    Task Add(Dictionary<CardExternalId, Listing> listings,
        Dictionary<CardExternalId, ReadOnlyCollection<ListingHistoricalPrice>> prices,
        CancellationToken cancellationToken);

    /// <summary>
    /// Should add the specified <see cref="ListingOrder"/>s
    /// </summary>
    /// /// <param name="listings">The <see cref="Listing"/>s that the <see cref="ListingOrder"/>s belong to</param>
    /// <param name="orders">The orders to add</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The completed task</returns>
    Task Add(Dictionary<CardExternalId, Listing> listings,
        Dictionary<CardExternalId, ReadOnlyCollection<ListingOrder>> orders, CancellationToken cancellationToken);
}