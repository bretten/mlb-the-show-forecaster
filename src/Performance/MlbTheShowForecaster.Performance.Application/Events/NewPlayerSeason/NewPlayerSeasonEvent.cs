using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Events.NewPlayerSeason;

/// <summary>
/// A new season for a player has been added
/// </summary>
/// <param name="PlayerMlbId">The MLB ID of the player</param>
public sealed record NewPlayerSeasonEvent(MlbId PlayerMlbId) : IDomainEvent;