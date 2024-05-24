using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Cards.EntityFrameworkCore;

public sealed class EntityFrameworkCorePlayerCardRepository : IPlayerCardRepository
{
    /// <summary>
    /// The DB context for <see cref="PlayerCard"/>
    /// </summary>
    private readonly CardsDbContext _dbContext;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbContext">The DB context for <see cref="PlayerCard"/></param>
    public EntityFrameworkCorePlayerCardRepository(CardsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Adds a <see cref="PlayerCard"/> to the repository
    /// </summary>
    /// <param name="card">The <see cref="PlayerCard"/> to add</param>
    /// <returns>The completed task</returns>
    public async Task Add(PlayerCard card)
    {
        await _dbContext.AddAsync(card);
    }

    /// <summary>
    /// Updates a <see cref="PlayerCard"/> in the repository
    /// </summary>
    /// <param name="card">The <see cref="PlayerCard"/> to update</param>
    /// <returns>The completed task</returns>
    public Task Update(PlayerCard card)
    {
        _dbContext.Update(card);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets all <see cref="PlayerCard"/>s for a specified season
    /// </summary>
    /// <param name="season">The season to retrieve <see cref="PlayerCard"/>s for</param>
    /// <returns>The collection of <see cref="PlayerCard"/>s for the season</returns>
    public async Task<IEnumerable<PlayerCard>> GetAll(SeasonYear season)
    {
        return await _dbContext.PlayerCardsWithHistoricalRatings()
            .Where(x => x.Year == season)
            .AsNoTracking() // The entities will not be changed
            .ToListAsync();
    }

    /// <summary>
    /// Returns a <see cref="PlayerCard"/> for the specified <see cref="CardExternalId"/>
    /// </summary>
    /// <param name="externalId">The <see cref="CardExternalId"/> of the <see cref="PlayerCard"/></param>
    /// <returns>The corresponding <see cref="PlayerCard"/></returns>
    public async Task<PlayerCard?> GetByExternalId(CardExternalId externalId)
    {
        return await _dbContext.PlayerCardsWithHistoricalRatings()
            .FirstOrDefaultAsync(x => x.ExternalId == externalId);
    }

    /// <summary>
    /// Checks if a <see cref="PlayerCard"/> exists
    /// </summary>
    /// <param name="externalId">The <see cref="CardExternalId"/> of the <see cref="PlayerCard"/></param>
    /// <returns>True if the <see cref="PlayerCard"/> exists, otherwise false</returns>
    public async Task<bool> Exists(CardExternalId externalId)
    {
        return await _dbContext.PlayerCards
            .AsNoTracking()
            .AnyAsync(x => x.ExternalId == externalId);
    }
}