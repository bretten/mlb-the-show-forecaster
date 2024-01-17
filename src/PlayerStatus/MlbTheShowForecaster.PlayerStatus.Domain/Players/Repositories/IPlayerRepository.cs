using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Repositories;

/// <summary>
/// Defines a repository for <see cref="Player"/>
/// </summary>
public interface IPlayerRepository
{
    /// <summary>
    /// Adds a <see cref="Player"/>
    /// </summary>
    /// <param name="player">The <see cref="Player"/> to add</param>
    /// <returns>The completed task</returns>
    Task Add(Player player);

    /// <summary>
    /// Updates a <see cref="Player"/>
    /// </summary>
    /// <param name="player">The <see cref="Player"/> to update</param>
    /// <returns>The completed task</returns>
    Task Update(Player player);

    /// <summary>
    /// Retrieves a <see cref="Player"/> by their <see cref="MlbId"/>
    /// </summary>
    /// <param name="mlbId">The <see cref="MlbId"/> of the <see cref="Player"/></param>
    /// <returns>A <see cref="Player"/> corresponding to the <see cref="MlbId"/>, otherwise null</returns>
    Task<Player?> GetByMlbId(MlbId mlbId);
}