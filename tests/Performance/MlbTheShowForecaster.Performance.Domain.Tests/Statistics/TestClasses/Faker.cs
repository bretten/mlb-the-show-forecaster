using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static BattingStats FakeBattingStats(int plateAppearances = 0, int atBats = 0, int runs = 0, int hits = 0,
        int doubles = 0, int triples = 0, int homeRuns = 0, int runsBattedIn = 0, int baseOnBalls = 0,
        int intentionalWalks = 0, int strikeouts = 0, int stolenBases = 0, int caughtStealing = 0, int hitByPitch = 0,
        int sacrificeBunts = 0, int sacrificeFlies = 0, int numberOfPitchesSeen = 0, int leftOnBase = 0,
        int groundOuts = 0, int groundIntoDoublePlays = 0, int groundIntoTriplePlays = 0, int airOuts = 0,
        int catchersInterference = 0)
    {
        return BattingStats.Create(plateAppearances: plateAppearances,
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
            catchersInterference: catchersInterference
        );
    }

    public static PitchingStats FakePitchingStats(int wins = 0, int losses = 0, int gamesStarted = 0,
        int gamesFinished = 0, int completeGames = 0, int shutouts = 0, int holds = 0, int saves = 0,
        int blownSaves = 0, int saveOpportunities = 0, decimal inningsPitched = 0, int hits = 0, int doubles = 0,
        int triples = 0, int homeRuns = 0, int runs = 0, int earnedRuns = 0, int strikeouts = 0, int baseOnBalls = 0,
        int intentionalWalks = 0, int hitBatsmen = 0, int outs = 0, int groundOuts = 0, int airOuts = 0,
        int groundIntoDoublePlays = 0, int numberOfPitches = 0, int strikes = 0, int wildPitches = 0, int balks = 0,
        int battersFaced = 0, int atBats = 0, int stolenBases = 0, int caughtStealing = 0, int pickOffs = 0,
        int inheritedRunners = 0, int inheritedRunnersScored = 0, int catchersInterferences = 0, int sacrificeBunts = 0,
        int sacrificeFlies = 0)
    {
        return PitchingStats.Create(wins: wins,
            losses: losses,
            gamesStarted: gamesStarted,
            gamesFinished: gamesFinished,
            completeGames: completeGames,
            shutouts: shutouts,
            holds: holds,
            saves: saves,
            blownSaves: blownSaves,
            saveOpportunities: saveOpportunities,
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
            pickOffs: pickOffs,
            inheritedRunners: inheritedRunners,
            inheritedRunnersScored: inheritedRunnersScored,
            catchersInterferences: catchersInterferences,
            sacrificeBunts: sacrificeBunts,
            sacrificeFlies: sacrificeFlies
        );
    }

    public static FieldingStats FakeFieldingStats(Position position = Position.Catcher, int gamesStarted = 0,
        decimal inningsPlayed = 0, int assists = 0, int putOuts = 0, int errors = 0, int throwingErrors = 0,
        int doublePlays = 0, int triplePlays = 0, int caughtStealing = 0, int stolenBases = 0, int passedBalls = 0,
        int catchersInterference = 0, int wildPitches = 0, int pickOffs = 0)
    {
        return FieldingStats.Create(position: position,
            gamesStarted: gamesStarted,
            inningsPlayed: inningsPlayed,
            assists: assists,
            putOuts: putOuts,
            errors: errors,
            throwingErrors: throwingErrors,
            doublePlays: doublePlays,
            triplePlays: triplePlays,
            caughtStealing: caughtStealing,
            stolenBases: stolenBases,
            passedBalls: passedBalls,
            catchersInterference: catchersInterference,
            wildPitches: wildPitches,
            pickOffs: pickOffs
        );
    }
}