using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetPlayerCardByExternalId;

/// <summary>
/// Query that retrieves a <see cref="PlayerCard"/> by its <see cref="CardExternalId"/>
/// </summary>
/// <param name="CardExternalId">The external card ID</param>
internal readonly record struct GetPlayerCardByExternalIdQuery(
    CardExternalId CardExternalId
) : IQuery<PlayerCard>;