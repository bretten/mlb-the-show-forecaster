﻿using System.Collections.Immutable;
using System.ComponentModel;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Stats;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Infrastructure.Dtos.Mapping;

/// <summary>
/// Maps MLB API data to application level DTOs
/// </summary>
public sealed class MlbApiPlayerStatsMapper : IMlbApiPlayerStatsMapper
{
    /// <summary>
    /// Maps a player's MLB API season stats data to <see cref="PlayerSeason"/>
    /// </summary>
    /// <param name="dto">A player's MLB API season stats data</param>
    /// <returns>The application level <see cref="PlayerSeason"/></returns>
    public PlayerSeason Map(PlayerSeasonStatsByGameDto dto)
    {
        var playerMlbId = MlbId.Create(dto.Id);
        var seasonYear = SeasonYear.Create((ushort)dto.SeasonYear);
        return new PlayerSeason(playerMlbId, seasonYear, MapBatting(dto), MapPitching(dto), MapFielding(dto));
    }

    /// <summary>
    /// Maps MLB API batting stats by games to the corresponding application level DTOs
    /// </summary>
    /// <param name="dto">A player's MLB API season stats data</param>
    /// <returns>Batting stats by games</returns>
    private IReadOnlyList<PlayerGameBattingStats> MapBatting(PlayerSeasonStatsByGameDto dto)
    {
        return dto.GetHittingStats()
            .Where(x => x.Stat.PlateAppearances > 0)
            .Select(x => new PlayerGameBattingStats(
                PlayerMlbId: MlbId.Create(dto.Id),
                SeasonYear: SeasonYear.Create((ushort)dto.SeasonYear),
                GameDate: x.Date,
                GameMlbId: MlbId.Create(x.Game.GamePk),
                TeamMlbId: MlbId.Create(x.Team.Id),
                PlateAppearances: NaturalNumber.Create(x.Stat.PlateAppearances),
                AtBats: NaturalNumber.Create(x.Stat.AtBats),
                Runs: NaturalNumber.Create(x.Stat.Runs),
                Hits: NaturalNumber.Create(x.Stat.Hits),
                Doubles: NaturalNumber.Create(x.Stat.Doubles),
                Triples: NaturalNumber.Create(x.Stat.Triples),
                HomeRuns: NaturalNumber.Create(x.Stat.HomeRuns),
                RunsBattedIn: NaturalNumber.Create(x.Stat.Rbi),
                BaseOnBalls: NaturalNumber.Create(x.Stat.BaseOnBalls),
                IntentionalWalks: NaturalNumber.Create(x.Stat.IntentionalWalks),
                Strikeouts: NaturalNumber.Create(x.Stat.StrikeOuts),
                StolenBases: NaturalNumber.Create(x.Stat.StolenBases),
                CaughtStealing: NaturalNumber.Create(x.Stat.CaughtStealing),
                HitByPitch: NaturalNumber.Create(x.Stat.HitByPitch),
                SacrificeBunts: NaturalNumber.Create(x.Stat.SacBunts),
                SacrificeFlies: NaturalNumber.Create(x.Stat.SacFlies),
                NumberOfPitchesSeen: NaturalNumber.Create(x.Stat.NumberOfPitches),
                LeftOnBase: NaturalNumber.Create(x.Stat.LeftOnBase),
                GroundOuts: NaturalNumber.Create(x.Stat.GroundOuts),
                GroundIntoDoublePlays: NaturalNumber.Create(x.Stat.GroundIntoDoublePlay),
                GroundIntoTriplePlays: NaturalNumber.Create(x.Stat.GroundIntoTriplePlay),
                AirOuts: NaturalNumber.Create(x.Stat.AirOuts),
                CatcherInterferences: NaturalNumber.Create(x.Stat.CatcherInterferences)
            )).ToImmutableList();
    }

    /// <summary>
    /// Maps MLB API pitching stats by games to the corresponding application level DTOs
    /// </summary>
    /// <param name="dto">A player's MLB API season stats data</param>
    /// <returns>Pitching stats by games</returns>
    private IReadOnlyList<PlayerGamePitchingStats> MapPitching(PlayerSeasonStatsByGameDto dto)
    {
        return dto.GetPitchingStats().Select(x => new PlayerGamePitchingStats(
            PlayerMlbId: MlbId.Create(dto.Id),
            SeasonYear: SeasonYear.Create((ushort)dto.SeasonYear),
            GameDate: x.Date,
            GameMlbId: MlbId.Create(x.Game.GamePk),
            TeamMlbId: MlbId.Create(x.Team.Id),
            Win: x.Stat.Wins > 1,
            Loss: x.Stat.Losses > 1,
            GameStarted: x.Stat.GamesStarted > 1,
            GameFinished: x.Stat.GamesFinished > 1,
            CompleteGame: x.Stat.CompleteGames > 1,
            Shutout: x.Stat.Shutouts > 1,
            Hold: x.Stat.Holds > 1,
            Save: x.Stat.Saves > 1,
            BlownSave: x.Stat.BlownSaves > 1,
            SaveOpportunity: x.Stat.SaveOpportunities > 1,
            InningsPitched: InningsCount.Create(x.Stat.InningsPitched),
            Hits: NaturalNumber.Create(x.Stat.Hits),
            Doubles: NaturalNumber.Create(x.Stat.Doubles),
            Triples: NaturalNumber.Create(x.Stat.Triples),
            HomeRuns: NaturalNumber.Create(x.Stat.HomeRuns),
            Runs: NaturalNumber.Create(x.Stat.Runs),
            EarnedRuns: NaturalNumber.Create(x.Stat.EarnedRuns),
            Strikeouts: NaturalNumber.Create(x.Stat.StrikeOuts),
            BaseOnBalls: NaturalNumber.Create(x.Stat.BaseOnBalls),
            IntentionalWalks: NaturalNumber.Create(x.Stat.IntentionalWalks),
            HitBatsmen: NaturalNumber.Create(x.Stat.HitBatsmen),
            Outs: NaturalNumber.Create(x.Stat.Outs),
            GroundOuts: NaturalNumber.Create(x.Stat.GroundOuts),
            AirOuts: NaturalNumber.Create(x.Stat.AirOuts),
            GroundIntoDoublePlays: NaturalNumber.Create(x.Stat.GroundIntoDoublePlay),
            NumberOfPitches: NaturalNumber.Create(x.Stat.NumberOfPitches),
            Strikes: NaturalNumber.Create(x.Stat.Strikes),
            WildPitches: NaturalNumber.Create(x.Stat.WildPitches),
            Balks: NaturalNumber.Create(x.Stat.Balks),
            BattersFaced: NaturalNumber.Create(x.Stat.BattersFaced),
            AtBats: NaturalNumber.Create(x.Stat.AtBats),
            StolenBases: NaturalNumber.Create(x.Stat.StolenBases),
            CaughtStealing: NaturalNumber.Create(x.Stat.CaughtStealing),
            Pickoffs: NaturalNumber.Create(x.Stat.Pickoffs),
            InheritedRunners: NaturalNumber.Create(x.Stat.InheritedRunners),
            InheritedRunnersScored: NaturalNumber.Create(x.Stat.InheritedRunnersScored),
            CatcherInterferences: NaturalNumber.Create(x.Stat.CatcherInterferences),
            SacrificeBunts: NaturalNumber.Create(x.Stat.SacBunts),
            SacrificeFlies: NaturalNumber.Create(x.Stat.SacFlies)
        )).ToImmutableList();
    }

    /// <summary>
    /// Maps MLB API fielding stats by games to the corresponding application level DTOs
    /// </summary>
    /// <param name="dto">A player's MLB API season stats data</param>
    /// <returns>Fielding stats by games</returns>
    private IReadOnlyList<PlayerGameFieldingStats> MapFielding(PlayerSeasonStatsByGameDto dto)
    {
        return dto.GetFieldingStats().Select(x => new PlayerGameFieldingStats(
            PlayerMlbId: MlbId.Create(dto.Id),
            SeasonYear: SeasonYear.Create((ushort)dto.SeasonYear),
            GameDate: x.Date,
            GameMlbId: MlbId.Create(x.Game.GamePk),
            TeamMlbId: MlbId.Create(x.Team.Id),
            Position: string.IsNullOrEmpty(x.Stat.Position.Abbreviation)
                ? Position.None
                : (Position)TypeDescriptor.GetConverter(typeof(Position)).ConvertFrom(x.Stat.Position.Abbreviation)!,
            GameStarted: x.Stat.GamesStarted > 1,
            InningsPlayed: InningsCount.Create(x.Stat.Innings),
            Assists: NaturalNumber.Create(x.Stat.Assists),
            Putouts: NaturalNumber.Create(x.Stat.Putouts),
            Errors: NaturalNumber.Create(x.Stat.Errors),
            ThrowingErrors: NaturalNumber.Create(x.Stat.ThrowingErrors),
            DoublePlays: NaturalNumber.Create(x.Stat.DoublePlays),
            TriplePlays: NaturalNumber.Create(x.Stat.TriplePlays),
            CaughtStealing: NaturalNumber.Create(x.Stat.CaughtStealing),
            StolenBases: NaturalNumber.Create(x.Stat.StolenBases),
            PassedBalls: NaturalNumber.Create(x.Stat.PassedBall),
            CatcherInterferences: NaturalNumber.Create(x.Stat.CatcherInterferences),
            WildPitches: NaturalNumber.Create(x.Stat.WildPitches),
            Pickoffs: NaturalNumber.Create(x.Stat.Pickoffs)
        )).ToImmutableList();
    }
}