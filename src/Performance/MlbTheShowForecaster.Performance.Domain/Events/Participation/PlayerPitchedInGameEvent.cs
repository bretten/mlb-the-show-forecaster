using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Participation;

/// <summary>
/// Published when a player pitches in a game
/// </summary>
/// <param name="PlayerMlbId">The MLB ID of the player</param>
/// <param name="Date">The date of the game</param>
public sealed record PlayerPitchedInGameEvent(MlbId PlayerMlbId, DateTime Date)
    : GameParticipationEvent(PlayerMlbId, Date);