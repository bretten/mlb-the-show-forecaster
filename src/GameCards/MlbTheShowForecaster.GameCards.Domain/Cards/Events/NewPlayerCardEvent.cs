using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;

/// <summary>
/// Raised when a new <see cref="PlayerCard"/> is created
/// </summary>
/// <param name="PlayerCard">The new <see cref="PlayerCard"/></param>
public sealed record NewPlayerCardEvent(PlayerCard PlayerCard) : IDomainEvent;