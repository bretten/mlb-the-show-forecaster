using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

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

    /// <summary>
    /// Should map a collection of <see cref="PlayerGameBattingStats"/> to <see cref="PlayerBattingStatsByGame"/>
    /// </summary>
    /// <param name="stats">The collection of <see cref="PlayerGameBattingStats"/> to map</param>
    /// <returns>The mapped collection of <see cref="PlayerBattingStatsByGame"/></returns>
    IEnumerable<PlayerBattingStatsByGame> MapBattingGames(IEnumerable<PlayerGameBattingStats> stats);

    /// <summary>
    /// Should map a collection of <see cref="PlayerGamePitchingStats"/> to <see cref="PlayerPitchingStatsByGame"/>
    /// </summary>
    /// <param name="stats">The collection of <see cref="PlayerGamePitchingStats"/> to map</param>
    /// <returns>The mapped collection of <see cref="PlayerPitchingStatsByGame"/></returns>
    IEnumerable<PlayerPitchingStatsByGame> MapPitchingGames(IEnumerable<PlayerGamePitchingStats> stats);

    /// <summary>
    /// Should map a collection of <see cref="PlayerGameFieldingStats"/> to <see cref="PlayerFieldingStatsByGame"/>
    /// </summary>
    /// <param name="stats">The collection of <see cref="PlayerGameFieldingStats"/> to map</param>
    /// <returns>The mapped collection of <see cref="PlayerFieldingStatsByGame"/></returns>
    IEnumerable<PlayerFieldingStatsByGame> MapFieldingGames(IEnumerable<PlayerGameFieldingStats> stats);
}