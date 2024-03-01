using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Queries.GetAllPlayerStatsBySeason;

/// <summary>
/// Query that retrieves all <see cref="PlayerStatsBySeason"/> for a specified season
/// </summary>
/// <param name="SeasonYear">The season to get <see cref="PlayerStatsBySeason"/> for</param>
internal readonly record struct GetAllPlayerStatsBySeasonQuery(SeasonYear SeasonYear)
    : IQuery<IEnumerable<PlayerStatsBySeason>>;