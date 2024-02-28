using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;

/// <summary>
/// Defines a mapper for converting <see cref="PlayerSeason"/> to other objects
/// </summary>
public interface IPlayerSeasonMapper
{
    /// <summary>
    /// Should map <see cref="PlayerSeason"/> to <see cref="PlayerStatsBySeason"/>
    /// </summary>
    /// <param name="playerSeason">The <see cref="PlayerSeason"/> to map</param>
    /// <returns>The mapped <see cref="PlayerStatsBySeason"/></returns>
    PlayerStatsBySeason Map(PlayerSeason playerSeason);
}