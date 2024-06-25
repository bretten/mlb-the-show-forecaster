using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Repositories;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFrameworkCore;

/// <summary>
/// EntityFramework implementation of <see cref="IPlayerRepository"/>
/// </summary>
public sealed class EntityFrameworkCorePlayerRepository : IPlayerRepository
{
    /// <summary>
    /// The DB context for <see cref="Player"/>
    /// </summary>
    private readonly PlayersDbContext _dbContext;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbContext">The DB context for <see cref="Player"/></param>
    public EntityFrameworkCorePlayerRepository(PlayersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Adds a <see cref="Player"/> to the repository
    /// </summary>
    /// <param name="player">The <see cref="Player"/> to add</param>
    public async Task Add(Player player)
    {
        await _dbContext.Players.AddAsync(player);
    }

    /// <summary>
    /// Updates a <see cref="Player"/> in the repository
    /// </summary>
    /// <param name="player">The <see cref="Player"/> to update</param>
    public Task Update(Player player)
    {
        _dbContext.Players.Update(player);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets a <see cref="Player"/> by their <see cref="MlbId"/>
    /// </summary>
    /// <param name="mlbId">The player's <see cref="MlbId"/></param>
    /// <returns>The corresponding <see cref="Player"/>, null if no one is found</returns>
    public async Task<Player?> GetByMlbId(MlbId mlbId)
    {
        return await _dbContext.Players
            .AsNoTracking() // No tracking needed, will update with DbContext.Update(), which will start tracking
            .FirstOrDefaultAsync(x => x.MlbId == mlbId);
    }

    /// <summary>
    /// Retrieves all <see cref="Player"/>s by their name
    /// </summary>
    /// <param name="firstName">The <see cref="Player"/>'s first name</param>
    /// <param name="lastName">The <see cref="Player"/>'s last name</param>
    /// <returns>Any players with a matching name</returns>
    public async Task<IEnumerable<Player>> GetAllByName(PersonName firstName, PersonName lastName)
    {
        return await _dbContext.Players
            .AsNoTracking()
            .Where(x => x.FirstName == firstName && x.LastName == lastName)
            .ToListAsync();
    }
}