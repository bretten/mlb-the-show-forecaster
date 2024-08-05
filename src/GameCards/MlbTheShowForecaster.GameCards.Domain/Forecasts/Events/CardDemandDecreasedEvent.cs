using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Events;

/// <summary>
/// Represents a <see cref="Card"/>'s market demand decreasing
/// </summary>
/// <param name="SeasonYear">The year of MLB The Show</param>
/// <param name="CardExternalId">The card ID from MLB The Show</param>
public sealed record CardDemandDecreasedEvent(SeasonYear SeasonYear, CardExternalId CardExternalId) : IDomainEvent;