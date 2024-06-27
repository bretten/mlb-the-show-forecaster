using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;

/// <summary>
/// Defines a repository for <see cref="PlayerStatsBySeason"/>
/// </summary>
public interface IPlayerStatsBySeasonRepository
{
    /// <summary>
    /// Adds a <see cref="PlayerStatsBySeason"/>
    /// </summary>
    /// <param name="stats">The <see cref="PlayerStatsBySeason"/> to add</param>
    /// <returns>The completed task</returns>
    Task Add(PlayerStatsBySeason stats);

    /// <summary>
    /// Updates a <see cref="PlayerStatsBySeason"/>
    /// </summary>
    /// <param name="stats">The <see cref="PlayerStatsBySeason"/> to update</param>
    /// <returns>The completed task</returns>
    Task Update(PlayerStatsBySeason stats);

    /// <summary>
    /// Returns a <see cref="PlayerStatsBySeason"/> for the specified ID
    /// </summary>
    /// <param name="id">The ID of the <see cref="PlayerStatsBySeason"/></param>
    /// <returns><see cref="PlayerStatsBySeason"/> for the specified ID</returns>
    Task<PlayerStatsBySeason?> GetById(Guid id);

    /// <summary>
    /// Returns a <see cref="PlayerStatsBySeason"/> for the specified season and MLB ID
    /// </summary>
    /// <param name="seasonYear">The season</param>
    /// <param name="mlbId">The MLB ID of the <see cref="PlayerStatsBySeason"/></param>
    /// <returns><see cref="PlayerStatsBySeason"/> for the specified season and MLB ID</returns>
    Task<PlayerStatsBySeason?> GetBy(SeasonYear seasonYear, MlbId mlbId);

    /// <summary>
    /// Returns all <see cref="PlayerStatsBySeason"/> for the specified season
    /// </summary>
    /// <param name="seasonYear">The season</param>
    /// <returns>All <see cref="PlayerStatsBySeason"/> for the specified season</returns>
    Task<IEnumerable<PlayerStatsBySeason>> GetAll(SeasonYear seasonYear);
}