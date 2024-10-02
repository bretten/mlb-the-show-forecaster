using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static PlayerBattingStatsByGame FakePlayerBattingStats(int playerMlbId = 1, ushort seasonYear = 2024,
        DateOnly? gameDate = null, int gameId = 10000, int teamId = 100, int plateAppearances = 0, int atBats = 0,
        int runs = 0, int hits = 0, int doubles = 0, int triples = 0, int homeRuns = 0, int runsBattedIn = 0,
        int baseOnBalls = 0, int intentionalWalks = 0, int strikeouts = 0, int stolenBases = 0, int caughtStealing = 0,
        int hitByPitch = 0, int sacrificeBunts = 0, int sacrificeFlies = 0, int numberOfPitchesSeen = 0,
        int leftOnBase = 0, int groundOuts = 0, int groundIntoDoublePlays = 0, int groundIntoTriplePlays = 0,
        int airOuts = 0, int catcherInterferences = 0)
    {
        return PlayerBattingStatsByGame.Create(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            gameDate ?? new DateOnly(2024, 4, 1), MlbId.Create(gameId), MlbId.Create(teamId),
            plateAppearances: plateAppearances,
            atBats: atBats,
            runs: runs,
            hits: hits,
            doubles: doubles,
            triples: triples,
            homeRuns: homeRuns,
            runsBattedIn: runsBattedIn,
            baseOnBalls: baseOnBalls,
            intentionalWalks: intentionalWalks,
            strikeouts: strikeouts,
            stolenBases: stolenBases,
            caughtStealing: caughtStealing,
            hitByPitch: hitByPitch,
            sacrificeBunts: sacrificeBunts,
            sacrificeFlies: sacrificeFlies,
            numberOfPitchesSeen: numberOfPitchesSeen,
            leftOnBase: leftOnBase,
            groundOuts: groundOuts,
            groundIntoDoublePlays: groundIntoDoublePlays,
            groundIntoTriplePlays: groundIntoTriplePlays,
            airOuts: airOuts,
            catcherInterferences: catcherInterferences
        );
    }

    public static PlayerBattingStatsByGame FakePlayerBattingStats(int scalar, int playerMlbId = 1,
        ushort seasonYear = 2024, DateOnly? gameDate = null, int gameId = 10000, int teamId = 100)
    {
        return FakePlayerBattingStats(playerMlbId, seasonYear, gameDate ?? new DateOnly(2024, 4, 1), gameId, teamId,
            plateAppearances: TestStats.Batting.PlateAppearances * scalar,
            atBats: TestStats.Batting.AtBats * scalar,
            runs: TestStats.Batting.Runs * scalar,
            hits: TestStats.Batting.Hits * scalar,
            doubles: TestStats.Batting.Doubles * scalar,
            triples: TestStats.Batting.Triples * scalar,
            homeRuns: TestStats.Batting.HomeRuns * scalar,
            runsBattedIn: TestStats.Batting.RunsBattedIn * scalar,
            baseOnBalls: TestStats.Batting.BaseOnBalls * scalar,
            intentionalWalks: TestStats.Batting.IntentionalWalks * scalar,
            strikeouts: TestStats.Batting.Strikeouts * scalar,
            stolenBases: TestStats.Batting.StolenBases * scalar,
            caughtStealing: TestStats.Batting.CaughtStealing * scalar,
            hitByPitch: TestStats.Batting.HitByPitch * scalar,
            sacrificeBunts: TestStats.Batting.SacrificeBunts * scalar,
            sacrificeFlies: TestStats.Batting.SacrificeFlies * scalar,
            numberOfPitchesSeen: TestStats.Batting.NumberOfPitchesSeen * scalar,
            leftOnBase: TestStats.Batting.LeftOnBase * scalar,
            groundOuts: TestStats.Batting.GroundOuts * scalar,
            groundIntoDoublePlays: TestStats.Batting.GroundIntoDoublePlays * scalar,
            groundIntoTriplePlays: TestStats.Batting.GroundIntoTriplePlays * scalar,
            airOuts: TestStats.Batting.AirOuts * scalar,
            catcherInterferences: TestStats.Batting.CatcherInterferences * scalar
        );
    }

    public static PlayerPitchingStatsByGame FakePlayerPitchingStats(int playerMlbId = 1, ushort seasonYear = 2024,
        DateOnly? gameDate = null, int gameId = 10000, int teamId = 100, bool win = false, bool loss = false,
        bool gameStarted = false, bool gameFinished = false, bool completeGame = false, bool shutout = false,
        bool hold = false, bool save = false, bool blownSave = false, bool saveOpportunity = false,
        decimal inningsPitched = 0, int hits = 0, int doubles = 0, int triples = 0, int homeRuns = 0, int runs = 0,
        int earnedRuns = 0, int strikeouts = 0, int baseOnBalls = 0, int intentionalWalks = 0, int hitBatsmen = 0,
        int outs = 0, int groundOuts = 0, int airOuts = 0, int groundIntoDoublePlays = 0, int numberOfPitches = 0,
        int strikes = 0, int wildPitches = 0, int balks = 0, int battersFaced = 0, int atBats = 0, int stolenBases = 0,
        int caughtStealing = 0, int pickoffs = 0, int inheritedRunners = 0, int inheritedRunnersScored = 0,
        int catcherInterferences = 0, int sacrificeBunts = 0, int sacrificeFlies = 0)
    {
        return PlayerPitchingStatsByGame.Create(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            gameDate ?? new DateOnly(2024, 4, 1), MlbId.Create(gameId), MlbId.Create(teamId),
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
            inningsPitched: inningsPitched,
            hits: hits,
            doubles: doubles,
            triples: triples,
            homeRuns: homeRuns,
            runs: runs,
            earnedRuns: earnedRuns,
            strikeouts: strikeouts,
            baseOnBalls: baseOnBalls,
            intentionalWalks: intentionalWalks,
            hitBatsmen: hitBatsmen,
            outs: outs,
            groundOuts: groundOuts,
            airOuts: airOuts,
            groundIntoDoublePlays: groundIntoDoublePlays,
            numberOfPitches: numberOfPitches,
            strikes: strikes,
            wildPitches: wildPitches,
            balks: balks,
            battersFaced: battersFaced,
            atBats: atBats,
            stolenBases: stolenBases,
            caughtStealing: caughtStealing,
            pickoffs: pickoffs,
            inheritedRunners: inheritedRunners,
            inheritedRunnersScored: inheritedRunnersScored,
            catcherInterferences: catcherInterferences,
            sacrificeBunts: sacrificeBunts,
            sacrificeFlies: sacrificeFlies
        );
    }

    public static PlayerPitchingStatsByGame FakePlayerPitchingStats(int scalar, int playerMlbId = 1,
        ushort seasonYear = 2024, DateOnly? gameDate = null, int gameId = 10000, int teamId = 100, bool win = false,
        bool loss = false, bool gameStarted = false, bool gameFinished = false, bool completeGame = false,
        bool shutout = false, bool hold = false, bool save = false, bool blownSave = false,
        bool saveOpportunity = false)
    {
        return FakePlayerPitchingStats(playerMlbId, seasonYear, gameDate ?? new DateOnly(2024, 4, 1), gameId, teamId,
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

    public static PlayerFieldingStatsByGame FakePlayerFieldingStats(int playerMlbId = 1, ushort seasonYear = 2024,
        DateOnly? gameDate = null, int gameId = 10000, int teamId = 100, Position position = Position.Catcher,
        bool gameStarted = false, decimal inningsPlayed = 0, int assists = 0, int putouts = 0, int errors = 0,
        int throwingErrors = 0, int doublePlays = 0, int triplePlays = 0, int caughtStealing = 0, int stolenBases = 0,
        int passedBalls = 0, int catcherInterferences = 0, int wildPitches = 0, int pickoffs = 0)
    {
        return PlayerFieldingStatsByGame.Create(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            gameDate ?? new DateOnly(2024, 4, 1), MlbId.Create(gameId), MlbId.Create(teamId),
            position: position,
            gameStarted: gameStarted,
            inningsPlayed: inningsPlayed,
            assists: assists,
            putouts: putouts,
            errors: errors,
            throwingErrors: throwingErrors,
            doublePlays: doublePlays,
            triplePlays: triplePlays,
            caughtStealing: caughtStealing,
            stolenBases: stolenBases,
            passedBalls: passedBalls,
            catcherInterferences: catcherInterferences,
            wildPitches: wildPitches,
            pickoffs: pickoffs
        );
    }

    public static PlayerFieldingStatsByGame FakePlayerFieldingStats(int scalar, int playerMlbId = 1,
        ushort seasonYear = 2024, DateOnly? gameDate = null, int gameId = 10000, int teamId = 100,
        Position position = Position.Catcher, bool gameStarted = false)
    {
        return FakePlayerFieldingStats(playerMlbId, seasonYear, gameDate ?? new DateOnly(2024, 4, 1), gameId, teamId,
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

    public static PlayerStatsBySeason FakePlayerStatsBySeason(int playerMlbId = 1, ushort seasonYear = 2024,
        decimal battingScore = 0.5m, decimal pitchingScore = 0.5m, decimal fieldingScore = 0.5m,
        List<PlayerBattingStatsByGame>? battingStatsByGames = null,
        List<PlayerPitchingStatsByGame>? pitchingStatsByGames = null,
        List<PlayerFieldingStatsByGame>? fieldingStatsByGames = null)
    {
        return PlayerStatsBySeason.Create(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            battingScore: FakePerformanceScore(battingScore),
            pitchingScore: FakePerformanceScore(pitchingScore),
            fieldingScore: FakePerformanceScore(fieldingScore),
            battingStatsByGames ?? new List<PlayerBattingStatsByGame>(),
            pitchingStatsByGames ?? new List<PlayerPitchingStatsByGame>(),
            fieldingStatsByGames ?? new List<PlayerFieldingStatsByGame>()
        );
    }

    public static PerformanceScore FakePerformanceScore(decimal score = 0.5m)
    {
        return PerformanceScore.Create(score);
    }
}