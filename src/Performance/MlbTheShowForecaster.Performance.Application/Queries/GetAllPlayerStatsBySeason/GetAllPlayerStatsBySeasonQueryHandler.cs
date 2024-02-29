using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Queries.GetAllPlayerStatsBySeason;

/// <summary>
/// Handles a <see cref="GetAllPlayerStatsBySeasonQuery"/>
///
/// <para>Returns all <see cref="PlayerStatsBySeason"/> for a specified season</para>
/// </summary>
internal sealed class
    GetAllPlayerStatsBySeasonQueryHandler : IQueryHandler<GetAllPlayerStatsBySeasonQuery,
    IEnumerable<PlayerStatsBySeason>>
{
    /// <summary>
    /// The <see cref="PlayerStatsBySeason"/> repository
    /// </summary>
    private readonly IPlayerStatsBySeasonRepository _playerStatsBySeasonRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerStatsBySeasonRepository">The <see cref="PlayerStatsBySeason"/> repository</param>
    public GetAllPlayerStatsBySeasonQueryHandler(IPlayerStatsBySeasonRepository playerStatsBySeasonRepository)
    {
        _playerStatsBySeasonRepository = playerStatsBySeasonRepository;
    }

    /// <summary>
    /// Handles a <see cref="GetAllPlayerStatsBySeasonQuery"/>
    /// </summary>
    /// <param name="query">The query</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>A collection of <see cref="PlayerStatsBySeason"/> for the specified season</returns>
    public async Task<IEnumerable<PlayerStatsBySeason>?> Handle(GetAllPlayerStatsBySeasonQuery query,
        CancellationToken cancellationToken)
    {
        return await _playerStatsBySeasonRepository.GetAll(query.SeasonYear);
    }
}