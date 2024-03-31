using System.Collections.Immutable;
using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetAllPlayerCards;

/// <summary>
/// Handles a <see cref="GetAllPlayerCardsQuery"/>
///
/// <para>Gets all <see cref="PlayerCard"/>s for the specified year</para>
/// </summary>
internal sealed class GetAllPlayerCardsQueryHandler : IQueryHandler<GetAllPlayerCardsQuery, IReadOnlyList<PlayerCard>>
{
    /// <summary>
    /// The <see cref="PlayerCard"/> repository
    /// </summary>
    private readonly IPlayerCardRepository _playerCardRepository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="playerCardRepository">The <see cref="PlayerCard"/> repository</param>
    public GetAllPlayerCardsQueryHandler(IPlayerCardRepository playerCardRepository)
    {
        _playerCardRepository = playerCardRepository;
    }

    /// <summary>
    /// Gets all <see cref="PlayerCard"/> for the specified year
    /// </summary>
    /// <param name="query">The query</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The <see cref="PlayerCard"/>s for the specified year</returns>
    public async Task<IReadOnlyList<PlayerCard>?> Handle(GetAllPlayerCardsQuery query,
        CancellationToken cancellationToken)
    {
        return (await _playerCardRepository.GetAll(query.Year)).ToImmutableList();
    }
}