using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Participation;

/// <summary>
/// Represents an event where a player participates in a game
/// </summary>
/// <param name="PlayerMlbId">The MLB ID of the player</param>
/// <param name="Date">The date of the game</param>
public abstract record GameParticipationEvent(MlbId PlayerMlbId, DateOnly Date) : IDomainEvent;