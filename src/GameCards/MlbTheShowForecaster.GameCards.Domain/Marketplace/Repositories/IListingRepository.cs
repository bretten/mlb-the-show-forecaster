using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;

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
    /// <returns>The completed task</returns>
    Task Add(Listing listing);

    /// <summary>
    /// Should update a <see cref="Listing"/>
    /// </summary>
    /// <param name="listing">The <see cref="Listing"/> to update</param>
    /// <returns>The completed task</returns>
    Task Update(Listing listing);

    /// <summary>
    /// Should return a <see cref="Listing"/> for the specified <see cref="CardExternalId"/>
    /// </summary>
    /// <param name="externalId">The <see cref="CardExternalId"/> of the <see cref="Listing"/></param>
    /// <returns>The corresponding <see cref="Listing"/></returns>
    Task<Listing?> GetByExternalId(CardExternalId externalId);
}