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
    /// Returns all <see cref="PlayerStatsBySeason"/> for the specified season
    /// </summary>
    /// <param name="seasonYear">The season</param>
    /// <returns>All <see cref="PlayerStatsBySeason"/> for the specified season</returns>
    Task<IEnumerable<PlayerStatsBySeason>> GetAll(SeasonYear seasonYear);
}