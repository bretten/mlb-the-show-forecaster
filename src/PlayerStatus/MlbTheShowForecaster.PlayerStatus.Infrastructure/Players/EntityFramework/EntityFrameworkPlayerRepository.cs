using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Repositories;
using Microsoft.EntityFrameworkCore;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Players.EntityFramework;

/// <summary>
/// EntityFramework implementation of <see cref="IPlayerRepository"/>
/// </summary>
public sealed class EntityFrameworkPlayerRepository : IPlayerRepository
{
    /// <summary>
    /// The DB context for <see cref="Player"/>
    /// </summary>
    private readonly PlayersDbContext _dbContext;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbContext">The DB context for <see cref="Player"/></param>
    public EntityFrameworkPlayerRepository(PlayersDbContext dbContext)
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
            //.AsNoTracking() // The entities will be updated, so leave AsNoTracking commented since is not a read-only scenario
            .FirstOrDefaultAsync(x => x.MlbId == mlbId);
    }
}