using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Repositories;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Queries.GetPlayerByMlbId;

/// <summary>
/// Handles a <see cref="GetPlayerByMlbIdQuery"/>
///
/// <para>Returns a <see cref="Player"/> based on their <see cref="MlbId"/></para>
/// </summary>
public sealed class GetPlayerByMlbIdQueryHandler : IQueryHandler<GetPlayerByMlbIdQuery, Player>
{
    /// <summary>
    /// The <see cref="Player"/> repository
    /// </summary>
    private readonly IPlayerRepository _playerRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerRepository">The <see cref="Player"/> repository</param>
    public GetPlayerByMlbIdQueryHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    /// <summary>
    /// Handles the <see cref="GetPlayerByMlbIdQuery"/>
    /// </summary>
    /// <param name="query">The query</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The <see cref="Player"/> if one is found</returns>
    public async Task<Player?> Handle(GetPlayerByMlbIdQuery query, CancellationToken cancellationToken)
    {
        return await _playerRepository.GetByMlbId(query.MlbId);
    }
}