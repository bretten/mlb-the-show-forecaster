﻿using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetPlayerCardByExternalId;

/// <summary>
/// Handles a <see cref="GetPlayerCardByExternalIdQuery"/>
///
/// <para>Returns a <see cref="PlayerCard"/> that corresponds to the specified <see cref="CardExternalId"/></para>
/// </summary>
internal sealed class GetPlayerCardByExternalIdQueryHandler : IQueryHandler<GetPlayerCardByExternalIdQuery, PlayerCard?>
{
    /// <summary>
    /// The <see cref="PlayerCard"/> repository
    /// </summary>
    private readonly IPlayerCardRepository _repository;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="repository">The <see cref="PlayerCard"/> repository</param>
    public GetPlayerCardByExternalIdQueryHandler(IPlayerCardRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Returns a <see cref="PlayerCard"/> by its <see cref="CardExternalId"/>
    /// </summary>
    /// <param name="query">The query</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete</param>
    /// <returns>The <see cref="PlayerCard"/> corresponding to the <see cref="CardExternalId"/> in the query, or null if no match is found</returns>
    public async Task<PlayerCard?> Handle(GetPlayerCardByExternalIdQuery query, CancellationToken cancellationToken)
    {
        return await _repository.GetByExternalId(query.CardExternalId);
    }
}