using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetAllPlayerCards;

/// <summary>
/// Query that retrieves all <see cref="PlayerCard"/>s for a specific year
/// </summary>
/// <param name="Year">The year to retrieve <see cref="PlayerCard"/>s for</param>
internal readonly record struct GetAllPlayerCardsQuery(SeasonYear Year) : IQuery<IReadOnlyList<PlayerCard>>;