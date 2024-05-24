using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;

/// <summary>
/// Defines a repository for <see cref="PlayerCard"/>
/// </summary>
public interface IPlayerCardRepository
{
    /// <summary>
    /// Should add a <see cref="PlayerCard"/>
    /// </summary>
    /// <param name="card">The <see cref="PlayerCard"/> to add</param>
    /// <returns>The completed task</returns>
    Task Add(PlayerCard card);

    /// <summary>
    /// Should update a <see cref="PlayerCard"/>
    /// </summary>
    /// <param name="card">The <see cref="PlayerCard"/> to update</param>
    /// <returns>The completed task</returns>
    Task Update(PlayerCard card);

    /// <summary>
    /// Should return all <see cref="PlayerCard"/>s for a specific season
    /// </summary>
    /// <param name="season">The season to retrieve <see cref="PlayerCard"/>s for</param>
    /// <returns>The collection of <see cref="PlayerCard"/>s for the season</returns>
    Task<IEnumerable<PlayerCard>> GetAll(SeasonYear season);

    /// <summary>
    /// Should return a <see cref="PlayerCard"/> for the specified <see cref="CardExternalId"/>
    /// </summary>
    /// <param name="externalId">The <see cref="CardExternalId"/> of the <see cref="PlayerCard"/></param>
    /// <returns>The corresponding <see cref="PlayerCard"/></returns>
    Task<PlayerCard?> GetByExternalId(CardExternalId externalId);

    /// <summary>
    /// Should determine if a <see cref="PlayerCard"/> exists
    /// </summary>
    /// <param name="externalId">The <see cref="CardExternalId"/> of the <see cref="PlayerCard"/></param>
    /// <returns>True if the <see cref="PlayerCard"/> exists, otherwise false</returns>
    Task<bool> Exists(CardExternalId externalId);
}