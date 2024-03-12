using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static PlayerBattingStatsByGame FakePlayerBattingStats(int playerMlbId = 1, ushort seasonYear = 2024,
        DateTime? gameDate = null, int gameId = 10000, int teamId = 100, int plateAppearances = 0, int atBats = 0,
        int runs = 0, int hits = 0, int doubles = 0, int triples = 0, int homeRuns = 0, int runsBattedIn = 0,
        int baseOnBalls = 0, int intentionalWalks = 0, int strikeouts = 0, int stolenBases = 0, int caughtStealing = 0,
        int hitByPitch = 0, int sacrificeBunts = 0, int sacrificeFlies = 0, int numberOfPitchesSeen = 0,
        int leftOnBase = 0, int groundOuts = 0, int groundIntoDoublePlays = 0, int groundIntoTriplePlays = 0,
        int airOuts = 0, int catcherInterferences = 0)
    {
        return PlayerBattingStatsByGame.Create(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            gameDate ?? new DateTime(2024, 4, 1), MlbId.Create(gameId), MlbId.Create(teamId),
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

    public static PlayerPitchingStatsByGame FakePlayerPitchingStats(int playerMlbId = 1, ushort seasonYear = 2024,
        DateTime? gameDate = null, int gameId = 10000, int teamId = 100, bool win = false, bool loss = false,
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
            gameDate ?? new DateTime(2024, 4, 1), MlbId.Create(gameId), MlbId.Create(teamId),
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

    public static PlayerFieldingStatsByGame FakePlayerFieldingStats(int playerMlbId = 1, ushort seasonYear = 2024,
        DateTime? gameDate = null, int gameId = 10000, int teamId = 100, Position position = Position.Catcher,
        bool gameStarted = false, decimal inningsPlayed = 0, int assists = 0, int putouts = 0, int errors = 0,
        int throwingErrors = 0, int doublePlays = 0, int triplePlays = 0, int caughtStealing = 0, int stolenBases = 0,
        int passedBalls = 0, int catcherInterferences = 0, int wildPitches = 0, int pickoffs = 0)
    {
        return PlayerFieldingStatsByGame.Create(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            gameDate ?? new DateTime(2024, 4, 1), MlbId.Create(gameId), MlbId.Create(teamId),
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

    public static PlayerStatsBySeason FakePlayerSeasonStats(int playerMlbId = 1, ushort seasonYear = 2024,
        List<PlayerBattingStatsByGame>? battingStatsByGames = null,
        List<PlayerPitchingStatsByGame>? pitchingStatsByGames = null,
        List<PlayerFieldingStatsByGame>? fieldingStatsByGames = null)
    {
        return PlayerStatsBySeason.Create(MlbId.Create(playerMlbId), SeasonYear.Create(seasonYear),
            battingStatsByGames ?? new List<PlayerBattingStatsByGame>(),
            pitchingStatsByGames ?? new List<PlayerPitchingStatsByGame>(),
            fieldingStatsByGames ?? new List<PlayerFieldingStatsByGame>()
        );
    }
}