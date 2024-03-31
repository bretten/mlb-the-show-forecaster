using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetListingByCardExternalId;

/// <summary>
/// Query that retrieves a <see cref="Listing"/> by its <see cref="CardExternalId"/>
/// </summary>
/// <param name="CardExternalId">The <see cref="CardExternalId"/> of the <see cref="Listing"/></param>
internal readonly record struct GetListingByCardExternalIdQuery(
    CardExternalId CardExternalId
) : IQuery<Listing?>;