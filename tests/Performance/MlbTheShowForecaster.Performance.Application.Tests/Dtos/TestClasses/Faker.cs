using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Dtos.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static PlayerSeason FakePlayerSeason(int playerMlbId = 1, ushort seasonYear = 2024,
        List<PlayerGameBattingStats>? playerGameBattingStats = null,
        List<PlayerGamePitchingStats>? playerGamePitchingStats = null,
        List<PlayerGameFieldingStats>? playerGameFieldingStats = null)
    {
        return new PlayerSeason(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            playerGameBattingStats ?? new List<PlayerGameBattingStats>(),
            playerGamePitchingStats ?? new List<PlayerGamePitchingStats>(),
            playerGameFieldingStats ?? new List<PlayerGameFieldingStats>()
        );
    }

    public static PlayerGameBattingStats FakePlayerGameBattingStats(int playerMlbId = 1, ushort seasonYear = 2024,
        DateOnly? gameDate = null, int gameMlbId = 10000, int teamMlbId = 100, int plateAppearances = 0, int atBats = 0,
        int runs = 0, int hits = 0, int doubles = 0, int triples = 0, int homeRuns = 0, int runsBattedIn = 0,
        int baseOnBalls = 0, int intentionalWalks = 0, int strikeouts = 0, int stolenBases = 0, int caughtStealing = 0,
        int hitByPitch = 0, int sacrificeBunts = 0, int sacrificeFlies = 0, int numberOfPitchesSeen = 0,
        int leftOnBase = 0, int groundOuts = 0, int groundIntoDoublePlays = 0, int groundIntoTriplePlays = 0,
        int airOuts = 0, int catcherInterferences = 0)
    {
        return new PlayerGameBattingStats(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            gameDate ?? new DateOnly(2024, 4, 1), MlbId.Create(gameMlbId), MlbId.Create(teamMlbId),
            PlateAppearances: NaturalNumber.Create(plateAppearances),
            AtBats: NaturalNumber.Create(atBats),
            Runs: NaturalNumber.Create(runs),
            Hits: NaturalNumber.Create(hits),
            Doubles: NaturalNumber.Create(doubles),
            Triples: NaturalNumber.Create(triples),
            HomeRuns: NaturalNumber.Create(homeRuns),
            RunsBattedIn: NaturalNumber.Create(runsBattedIn),
            BaseOnBalls: NaturalNumber.Create(baseOnBalls),
            IntentionalWalks: NaturalNumber.Create(intentionalWalks),
            Strikeouts: NaturalNumber.Create(strikeouts),
            StolenBases: NaturalNumber.Create(stolenBases),
            CaughtStealing: NaturalNumber.Create(caughtStealing),
            HitByPitch: NaturalNumber.Create(hitByPitch),
            SacrificeBunts: NaturalNumber.Create(sacrificeBunts),
            SacrificeFlies: NaturalNumber.Create(sacrificeFlies),
            NumberOfPitchesSeen: NaturalNumber.Create(numberOfPitchesSeen),
            LeftOnBase: NaturalNumber.Create(leftOnBase),
            GroundOuts: NaturalNumber.Create(groundOuts),
            GroundIntoDoublePlays: NaturalNumber.Create(groundIntoDoublePlays),
            GroundIntoTriplePlays: NaturalNumber.Create(groundIntoTriplePlays),
            AirOuts: NaturalNumber.Create(airOuts),
            CatcherInterferences: NaturalNumber.Create(catcherInterferences)
        );
    }

    public static PlayerGameBattingStats FakePlayerGameBattingStats(int scalar, int playerMlbId = 1,
        ushort seasonYear = 2024, DateOnly? gameDate = null, int gameMlbId = 10000, int teamMlbId = 100)
    {
        return FakePlayerGameBattingStats(playerMlbId, seasonYear, gameDate ?? new DateOnly(2024, 4, 1), gameMlbId,
            teamMlbId,
            plateAppearances: scalar * TestStats.Batting.PlateAppearances,
            atBats: scalar * TestStats.Batting.AtBats,
            runs: scalar * TestStats.Batting.Runs,
            hits: scalar * TestStats.Batting.Hits,
            doubles: scalar * TestStats.Batting.Doubles,
            triples: scalar * TestStats.Batting.Triples,
            homeRuns: scalar * TestStats.Batting.HomeRuns,
            runsBattedIn: scalar * TestStats.Batting.RunsBattedIn,
            baseOnBalls: scalar * TestStats.Batting.BaseOnBalls,
            intentionalWalks: scalar * TestStats.Batting.IntentionalWalks,
            strikeouts: scalar * TestStats.Batting.Strikeouts,
            stolenBases: scalar * TestStats.Batting.StolenBases,
            caughtStealing: scalar * TestStats.Batting.CaughtStealing,
            hitByPitch: scalar * TestStats.Batting.HitByPitch,
            sacrificeBunts: scalar * TestStats.Batting.SacrificeBunts,
            sacrificeFlies: scalar * TestStats.Batting.SacrificeFlies,
            numberOfPitchesSeen: scalar * TestStats.Batting.NumberOfPitchesSeen,
            leftOnBase: scalar * TestStats.Batting.LeftOnBase,
            groundOuts: scalar * TestStats.Batting.GroundOuts,
            groundIntoDoublePlays: scalar * TestStats.Batting.GroundIntoDoublePlays,
            groundIntoTriplePlays: scalar * TestStats.Batting.GroundIntoTriplePlays,
            airOuts: scalar * TestStats.Batting.AirOuts,
            catcherInterferences: scalar * TestStats.Batting.CatcherInterferences
        );
    }

    public static PlayerGamePitchingStats FakePlayerGamePitchingStats(int playerMlbId = 1, ushort seasonYear = 2024,
        DateOnly? gameDate = null, int gameMlbId = 10000, int teamMlbId = 100, bool win = false, bool loss = false,
        bool gameStarted = false, bool gameFinished = false, bool completeGame = false, bool shutout = false,
        bool hold = false, bool save = false, bool blownSave = false, bool saveOpportunity = false,
        decimal inningsPitched = 0, int hits = 0, int doubles = 0, int triples = 0, int homeRuns = 0, int runs = 0,
        int earnedRuns = 0, int strikeouts = 0, int baseOnBalls = 0, int intentionalWalks = 0, int hitBatsmen = 0,
        int outs = 0, int groundOuts = 0, int airOuts = 0, int groundIntoDoublePlays = 0, int numberOfPitches = 0,
        int strikes = 0, int wildPitches = 0, int balks = 0, int battersFaced = 0, int atBats = 0, int stolenBases = 0,
        int caughtStealing = 0, int pickoffs = 0, int inheritedRunners = 0, int inheritedRunnersScored = 0,
        int catcherInterferences = 0, int sacrificeBunts = 0, int sacrificeFlies = 0)
    {
        return new PlayerGamePitchingStats(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            gameDate ?? new DateOnly(2024, 4, 1), MlbId.Create(gameMlbId), MlbId.Create(teamMlbId),
            Win: win,
            Loss: loss,
            GameStarted: gameStarted,
            GameFinished: gameFinished,
            CompleteGame: completeGame,
            Shutout: shutout,
            Hold: hold,
            Save: save,
            BlownSave: blownSave,
            SaveOpportunity: saveOpportunity,
            InningsPitched: InningsCount.Create(inningsPitched),
            Hits: NaturalNumber.Create(hits),
            Doubles: NaturalNumber.Create(doubles),
            Triples: NaturalNumber.Create(triples),
            HomeRuns: NaturalNumber.Create(homeRuns),
            Runs: NaturalNumber.Create(runs),
            EarnedRuns: NaturalNumber.Create(earnedRuns),
            Strikeouts: NaturalNumber.Create(strikeouts),
            BaseOnBalls: NaturalNumber.Create(baseOnBalls),
            IntentionalWalks: NaturalNumber.Create(intentionalWalks),
            HitBatsmen: NaturalNumber.Create(hitBatsmen),
            Outs: NaturalNumber.Create(outs),
            GroundOuts: NaturalNumber.Create(groundOuts),
            AirOuts: NaturalNumber.Create(airOuts),
            GroundIntoDoublePlays: NaturalNumber.Create(groundIntoDoublePlays),
            NumberOfPitches: NaturalNumber.Create(numberOfPitches),
            Strikes: NaturalNumber.Create(strikes),
            WildPitches: NaturalNumber.Create(wildPitches),
            Balks: NaturalNumber.Create(balks),
            BattersFaced: NaturalNumber.Create(battersFaced),
            AtBats: NaturalNumber.Create(atBats),
            StolenBases: NaturalNumber.Create(stolenBases),
            CaughtStealing: NaturalNumber.Create(caughtStealing),
            Pickoffs: NaturalNumber.Create(pickoffs),
            InheritedRunners: NaturalNumber.Create(inheritedRunners),
            InheritedRunnersScored: NaturalNumber.Create(inheritedRunnersScored),
            CatcherInterferences: NaturalNumber.Create(catcherInterferences),
            SacrificeBunts: NaturalNumber.Create(sacrificeBunts),
            SacrificeFlies: NaturalNumber.Create(sacrificeFlies)
        );
    }

    public static PlayerGamePitchingStats FakePlayerGamePitchingStats(int scalar, int playerMlbId = 1,
        ushort seasonYear = 2024, DateOnly? gameDate = null, int gameMlbId = 10000, int teamMlbId = 100,
        bool win = false, bool loss = false, bool gameStarted = false, bool gameFinished = false,
        bool completeGame = false, bool shutout = false, bool hold = false, bool save = false, bool blownSave = false,
        bool saveOpportunity = false)
    {
        return FakePlayerGamePitchingStats(playerMlbId, seasonYear, gameDate ?? new DateOnly(2024, 4, 1), gameMlbId,
            teamMlbId,
            win: win,
            loss: loss,
            gameStarted: gameStarted,
            gameFinished: gameFinished,
            completeGame: completeGame,
            shutout: shutout,
            hold: hold,
            save: save,
            blownSave: blownSave,
            saveOpportunity: saveOpportunity,
            inningsPitched: TestStats.Pitching.InningsPitched * scalar,
            hits: TestStats.Pitching.Hits * scalar,
            doubles: TestStats.Pitching.Doubles * scalar,
            triples: TestStats.Pitching.Triples * scalar,
            homeRuns: TestStats.Pitching.HomeRuns * scalar,
            runs: TestStats.Pitching.Runs * scalar,
            earnedRuns: TestStats.Pitching.EarnedRuns * scalar,
            strikeouts: TestStats.Pitching.Strikeouts * scalar,
            baseOnBalls: TestStats.Pitching.BaseOnBalls * scalar,
            intentionalWalks: TestStats.Pitching.IntentionalWalks * scalar,
            hitBatsmen: TestStats.Pitching.HitBatsmen * scalar,
            outs: TestStats.Pitching.Outs * scalar,
            groundOuts: TestStats.Pitching.GroundOuts * scalar,
            airOuts: TestStats.Pitching.AirOuts * scalar,
            groundIntoDoublePlays: TestStats.Pitching.GroundIntoDoublePlays * scalar,
            numberOfPitches: TestStats.Pitching.NumberOfPitches * scalar,
            strikes: TestStats.Pitching.Strikes * scalar,
            wildPitches: TestStats.Pitching.WildPitches * scalar,
            balks: TestStats.Pitching.Balks * scalar,
            battersFaced: TestStats.Pitching.BattersFaced * scalar,
            atBats: TestStats.Pitching.AtBats * scalar,
            stolenBases: TestStats.Pitching.StolenBases * scalar,
            caughtStealing: TestStats.Pitching.CaughtStealing * scalar,
            pickoffs: TestStats.Pitching.Pickoffs * scalar,
            inheritedRunners: TestStats.Pitching.InheritedRunners * scalar,
            inheritedRunnersScored: TestStats.Pitching.InheritedRunnersScored * scalar,
            catcherInterferences: TestStats.Pitching.CatcherInterferences * scalar,
            sacrificeBunts: TestStats.Pitching.SacrificeBunts * scalar,
            sacrificeFlies: TestStats.Pitching.SacrificeFlies * scalar
        );
    }

    public static PlayerGameFieldingStats FakePlayerGameFieldingStats(int playerMlbId = 1, ushort seasonYear = 2024,
        DateOnly? gameDate = null, int gameMlbId = 10000, int teamMlbId = 100, Position position = Position.Catcher,
        bool gameStarted = false, decimal inningsPlayed = 0, int assists = 0, int putouts = 0, int errors = 0,
        int throwingErrors = 0, int doublePlays = 0, int triplePlays = 0, int caughtStealing = 0, int stolenBases = 0,
        int passedBalls = 0, int catcherInterferences = 0, int wildPitches = 0, int pickoffs = 0)
    {
        return new PlayerGameFieldingStats(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            gameDate ?? new DateOnly(2024, 4, 1), MlbId.Create(gameMlbId), MlbId.Create(teamMlbId),
            Position: position,
            GameStarted: gameStarted,
            InningsPlayed: InningsCount.Create(inningsPlayed),
            Assists: NaturalNumber.Create(assists),
            Putouts: NaturalNumber.Create(putouts),
            Errors: NaturalNumber.Create(errors),
            ThrowingErrors: NaturalNumber.Create(throwingErrors),
            DoublePlays: NaturalNumber.Create(doublePlays),
            TriplePlays: NaturalNumber.Create(triplePlays),
            CaughtStealing: NaturalNumber.Create(caughtStealing),
            StolenBases: NaturalNumber.Create(stolenBases),
            PassedBalls: NaturalNumber.Create(passedBalls),
            CatcherInterferences: NaturalNumber.Create(catcherInterferences),
            WildPitches: NaturalNumber.Create(wildPitches),
            Pickoffs: NaturalNumber.Create(pickoffs)
        );
    }

    public static PlayerGameFieldingStats FakePlayerGameFieldingStats(int scalar, int playerMlbId = 1,
        ushort seasonYear = 2024, DateOnly? gameDate = null, int gameMlbId = 10000, int teamMlbId = 100,
        Position position = Position.Catcher, bool gameStarted = false)
    {
        return FakePlayerGameFieldingStats(playerMlbId, seasonYear, gameDate ?? new DateOnly(2024, 4, 1), gameMlbId,
            teamMlbId,
            position: position,
            gameStarted: gameStarted,
            inningsPlayed: TestStats.Fielding.InningsPlayed * scalar,
            assists: TestStats.Fielding.Assists * scalar,
            putouts: TestStats.Fielding.Putouts * scalar,
            errors: TestStats.Fielding.Errors * scalar,
            throwingErrors: TestStats.Fielding.ThrowingErrors * scalar,
            doublePlays: TestStats.Fielding.DoublePlays * scalar,
            triplePlays: TestStats.Fielding.TriplePlays * scalar,
            caughtStealing: TestStats.Fielding.CaughtStealing * scalar,
            stolenBases: TestStats.Fielding.StolenBases * scalar,
            passedBalls: TestStats.Fielding.PassedBalls * scalar,
            catcherInterferences: TestStats.Fielding.CatcherInterferences * scalar,
            wildPitches: TestStats.Fielding.WildPitches * scalar,
            pickoffs: TestStats.Fielding.Pickoffs * scalar
        );
    }

    public static PlayerSeasonPerformanceMetrics FakePlayerSeasonPerformanceMetrics(ushort seasonYear = 2024,
        int mlbId = 100, List<PerformanceMetricsByDate>? metricsByDate = null)
    {
        return new PlayerSeasonPerformanceMetrics(
            Year: SeasonYear.Create(seasonYear),
            MlbId: MlbId.Create(mlbId),
            MetricsByDate: metricsByDate ?? new List<PerformanceMetricsByDate>());
    }

    public static PerformanceMetricsByDate FakePerformanceMetricByDate(DateOnly? date = null,
        decimal battingScore = 0.1m, bool significantBattingParticipation = false, decimal pitchingScore = 0.2m,
        bool significantPitchingParticipation = false, decimal fieldingScore = 0.3m,
        bool significantFieldingParticipation = false, decimal battingAverage = 1.1m, decimal onBasePercentage = 1.2m,
        decimal slugging = 1.3m, decimal earnedRunAverage = 1.4m, decimal opponentsBattingAverage = 1.5m,
        decimal strikeoutsPer9 = 1.6m, decimal baseOnBallsPer9 = 1.7m, decimal homeRunsPer9 = 1.8m,
        decimal fieldingPercentage = 1.9m)
    {
        return new PerformanceMetricsByDate(Date: date ?? new DateOnly(2024, 10, 2),
            BattingScore: battingScore,
            SignificantBattingParticipation: significantBattingParticipation,
            PitchingScore: pitchingScore,
            SignificantPitchingParticipation: significantPitchingParticipation,
            FieldingScore: fieldingScore,
            SignificantFieldingParticipation: significantFieldingParticipation,
            BattingAverage: battingAverage,
            OnBasePercentage: onBasePercentage,
            Slugging: slugging,
            EarnedRunAverage: earnedRunAverage,
            OpponentsBattingAverage: opponentsBattingAverage,
            StrikeoutsPer9: strikeoutsPer9,
            BaseOnBallsPer9: baseOnBallsPer9,
            HomeRunsPer9: homeRunsPer9,
            FieldingPercentage: fieldingPercentage
        );
    }
}