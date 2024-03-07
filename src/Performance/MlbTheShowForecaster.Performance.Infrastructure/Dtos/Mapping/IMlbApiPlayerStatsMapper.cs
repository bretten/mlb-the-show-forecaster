using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Dtos.Mapping;

/// <summary>
/// Should map MLB API data to application level DTOs
/// </summary>
public interface IMlbApiPlayerStatsMapper
{
    /// <summary>
    /// Should map a player's MLB API season stats data to <see cref="PlayerSeason"/>
    /// </summary>
    /// <param name="dto">A player's MLB API season stats data</param>
    /// <returns>The application level <see cref="PlayerSeason"/></returns>
    PlayerSeason Map(PlayerSeasonStatsByGameDto dto);
}