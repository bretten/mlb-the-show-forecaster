using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Queries.GetPlayerByMlbId;

/// <summary>
/// Query that retrieves a <see cref="Player"/> by their MLB issued <see cref="MlbId"/>
/// </summary>
/// <param name="MlbId">The <see cref="Player"/>'s <see cref="MlbId"/></param>
internal readonly record struct GetPlayerByMlbIdQuery(MlbId MlbId) : IQuery<Player>;