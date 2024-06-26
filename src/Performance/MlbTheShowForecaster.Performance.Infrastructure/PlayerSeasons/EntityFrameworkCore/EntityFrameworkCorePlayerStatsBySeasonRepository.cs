using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;
using Microsoft.EntityFrameworkCore;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.PlayerSeasons.EntityFrameworkCore;

public sealed class EntityFrameworkCorePlayerStatsBySeasonRepository : IPlayerStatsBySeasonRepository
{
    /// <summary>
    /// The DB context for <see cref="PlayerStatsBySeason"/>
    /// </summary>
    private readonly PlayerSeasonsDbContext _dbContext;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbContext">The DB context for <see cref="PlayerStatsBySeason"/></param>
    public EntityFrameworkCorePlayerStatsBySeasonRepository(PlayerSeasonsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Adds a <see cref="PlayerStatsBySeason"/> to the repository
    /// </summary>
    /// <param name="stats">The <see cref="PlayerStatsBySeason"/> to add</param>
    public async Task Add(PlayerStatsBySeason stats)
    {
        await _dbContext.PlayerStatsBySeasons.AddAsync(stats);
    }

    /// <summary>
    /// Updates a <see cref="PlayerStatsBySeason"/> in the repository
    /// </summary>
    /// <param name="stats">The <see cref="PlayerStatsBySeason"/> to update</param>
    public Task Update(PlayerStatsBySeason stats)
    {
        _dbContext.PlayerStatsBySeasons.Update(stats);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Returns a <see cref="PlayerStatsBySeason"/> for the specified ID
    /// </summary>
    /// <param name="id">The ID of the <see cref="PlayerStatsBySeason"/></param>
    /// <returns><see cref="PlayerStatsBySeason"/> for the specified ID</returns>
    public async Task<PlayerStatsBySeason?> GetById(Guid id)
    {
        return await _dbContext.PlayerStatsBySeasonsWithGames()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Returns a <see cref="PlayerStatsBySeason"/> for the specified MLB ID
    /// </summary>
    /// <param name="mlbId">The MLB ID of the <see cref="PlayerStatsBySeason"/></param>
    /// <returns><see cref="PlayerStatsBySeason"/> for the specified MLB ID</returns>
    public async Task<PlayerStatsBySeason?> GetByMlbId(MlbId mlbId)
    {
        return await _dbContext.PlayerStatsBySeasonsWithGames()
            .FirstOrDefaultAsync(x => x.PlayerMlbId == mlbId);
    }

    /// <summary>
    /// Returns all <see cref="PlayerStatsBySeason"/> for the specified season
    /// </summary>
    /// <param name="seasonYear">The season</param>
    /// <returns>All <see cref="PlayerStatsBySeason"/> for the specified season</returns>
    public async Task<IEnumerable<PlayerStatsBySeason>> GetAll(SeasonYear seasonYear)
    {
        return await _dbContext.PlayerStatsBySeasonsWithGames()
            .Where(x => x.SeasonYear == seasonYear)
            .AsNoTracking() // Entities will not be updated so no tracking necessary
            .ToListAsync();
    }
}