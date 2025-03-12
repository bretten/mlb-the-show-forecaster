using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetPlayerCardByExternalId;

/// <summary>
/// Query that retrieves a <see cref="PlayerCard"/> by its <see cref="CardExternalId"/>
/// </summary>
/// <param name="Season">The season of the <see cref="PlayerCard"/></param>
/// <param name="CardExternalId">The external card ID</param>
internal readonly record struct GetPlayerCardByExternalIdQuery(
    SeasonYear Season,
    CardExternalId CardExternalId
) : IQuery<PlayerCard?>;