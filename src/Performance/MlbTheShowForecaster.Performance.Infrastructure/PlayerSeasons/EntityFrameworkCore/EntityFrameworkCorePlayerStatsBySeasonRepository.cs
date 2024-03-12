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
    /// Returns all <see cref="PlayerStatsBySeason"/> for the specified season
    /// </summary>
    /// <param name="seasonYear">The season</param>
    /// <returns>All <see cref="PlayerStatsBySeason"/> for the specified season</returns>
    public async Task<IEnumerable<PlayerStatsBySeason>> GetAll(SeasonYear seasonYear)
    {
        return await _dbContext.PlayerStatsBySeasonsWithGames()
            .Where(x => x.SeasonYear == seasonYear)
            //.AsNoTracking() // The entities will be updated, so leave AsNoTracking commented since is not a read-only scenario
            .ToListAsync();
    }
}