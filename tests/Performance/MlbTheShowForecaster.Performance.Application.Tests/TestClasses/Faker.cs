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
        DateOnly? gameDate = null, int gameId = 10000, int teamId = 100, int plateAppearances = 1, int atBats = 2,
        int runs = 3, int hits = 4, int doubles = 5, int triples = 6, int homeRuns = 7, int runsBattedIn = 8,
        int baseOnBalls = 9, int intentionalWalks = 10, int strikeouts = 11, int stolenBases = 12,
        int caughtStealing = 13, int hitByPitch = 14, int sacrificeBunts = 15, int sacrificeFlies = 16,
        int numberOfPitchesSeen = 17, int leftOnBase = 18, int groundOuts = 19, int groundIntoDoublePlays = 20,
        int groundIntoTriplePlays = 21, int airOuts = 22, int catcherInterferences = 23, int scalar = 1)
    {
        return PlayerBattingStatsByGame.Create(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            gameDate ?? new DateOnly(2024, 4, 1), MlbId.Create(gameId), MlbId.Create(teamId),
            plateAppearances: plateAppearances * scalar,
            atBats: atBats * scalar,
            runs: runs * scalar,
            hits: hits * scalar,
            doubles: doubles * scalar,
            triples: triples * scalar,
            homeRuns: homeRuns * scalar,
            runsBattedIn: runsBattedIn * scalar,
            baseOnBalls: baseOnBalls * scalar,
            intentionalWalks: intentionalWalks * scalar,
            strikeouts: strikeouts * scalar,
            stolenBases: stolenBases * scalar,
            caughtStealing: caughtStealing * scalar,
            hitByPitch: hitByPitch * scalar,
            sacrificeBunts: sacrificeBunts * scalar,
            sacrificeFlies: sacrificeFlies * scalar,
            numberOfPitchesSeen: numberOfPitchesSeen * scalar,
            leftOnBase: leftOnBase * scalar,
            groundOuts: groundOuts * scalar,
            groundIntoDoublePlays: groundIntoDoublePlays * scalar,
            groundIntoTriplePlays: groundIntoTriplePlays * scalar,
            airOuts: airOuts * scalar,
            catcherInterferences: catcherInterferences * scalar
        );
    }

    public static PlayerPitchingStatsByGame FakePlayerPitchingStats(int playerMlbId = 1, ushort seasonYear = 2024,
        DateOnly? gameDate = null, int gameId = 10000, int teamId = 100, bool win = false, bool loss = false,
        bool gameStarted = false, bool gameFinished = false, bool completeGame = false, bool shutout = false,
        bool hold = false, bool save = false, bool blownSave = false, bool saveOpportunity = false,
        decimal inningsPitched = 1, int hits = 2, int doubles = 3, int triples = 4, int homeRuns = 5, int runs = 6,
        int earnedRuns = 7, int strikeouts = 8, int baseOnBalls = 9, int intentionalWalks = 10, int hitBatsmen = 11,
        int outs = 12, int groundOuts = 13, int airOuts = 14, int groundIntoDoublePlays = 15, int numberOfPitches = 16,
        int strikes = 17, int wildPitches = 18, int balks = 19, int battersFaced = 20, int atBats = 21,
        int stolenBases = 22, int caughtStealing = 23, int pickoffs = 24, int inheritedRunners = 25,
        int inheritedRunnersScored = 26, int catcherInterferences = 27, int sacrificeBunts = 28,
        int sacrificeFlies = 29, int scalar = 1)
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
            inningsPitched: inningsPitched * scalar,
            hits: hits * scalar,
            doubles: doubles * scalar,
            triples: triples * scalar,
            homeRuns: homeRuns * scalar,
            runs: runs * scalar,
            earnedRuns: earnedRuns * scalar,
            strikeouts: strikeouts * scalar,
            baseOnBalls: baseOnBalls * scalar,
            intentionalWalks: intentionalWalks * scalar,
            hitBatsmen: hitBatsmen * scalar,
            outs: outs * scalar,
            groundOuts: groundOuts * scalar,
            airOuts: airOuts * scalar,
            groundIntoDoublePlays: groundIntoDoublePlays * scalar,
            numberOfPitches: numberOfPitches * scalar,
            strikes: strikes * scalar,
            wildPitches: wildPitches * scalar,
            balks: balks * scalar,
            battersFaced: battersFaced * scalar,
            atBats: atBats * scalar,
            stolenBases: stolenBases * scalar,
            caughtStealing: caughtStealing * scalar,
            pickoffs: pickoffs * scalar,
            inheritedRunners: inheritedRunners * scalar,
            inheritedRunnersScored: inheritedRunnersScored * scalar,
            catcherInterferences: catcherInterferences * scalar,
            sacrificeBunts: sacrificeBunts * scalar,
            sacrificeFlies: sacrificeFlies * scalar
        );
    }

    public static PlayerFieldingStatsByGame FakePlayerFieldingStats(int playerMlbId = 1, ushort seasonYear = 2024,
        DateOnly? gameDate = null, int gameId = 10000, int teamId = 100, Position position = Position.Catcher,
        bool gameStarted = false, decimal inningsPlayed = 1, int assists = 2, int putouts = 3, int errors = 4,
        int throwingErrors = 5, int doublePlays = 6, int triplePlays = 7, int caughtStealing = 8, int stolenBases = 9,
        int passedBalls = 10, int catcherInterferences = 11, int wildPitches = 12, int pickoffs = 13, int scalar = 1)
    {
        return PlayerFieldingStatsByGame.Create(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            gameDate ?? new DateOnly(2024, 4, 1), MlbId.Create(gameId), MlbId.Create(teamId),
            position: position,
            gameStarted: gameStarted,
            inningsPlayed: inningsPlayed * scalar,
            assists: assists * scalar,
            putouts: putouts * scalar,
            errors: errors * scalar,
            throwingErrors: throwingErrors * scalar,
            doublePlays: doublePlays * scalar,
            triplePlays: triplePlays * scalar,
            caughtStealing: caughtStealing * scalar,
            stolenBases: stolenBases * scalar,
            passedBalls: passedBalls * scalar,
            catcherInterferences: catcherInterferences * scalar,
            wildPitches: wildPitches * scalar,
            pickoffs: pickoffs * scalar
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