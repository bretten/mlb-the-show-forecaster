using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static PlayerBattingStatsByGame FakeBattingStats(int playerId = 1, ushort seasonYear = 2024,
        DateTime? gameDate = null, int gameId = 10000, int teamId = 100, uint plateAppearances = 0,
        uint atBats = 0, uint runs = 0, uint hits = 0, uint doubles = 0, uint triples = 0, uint homeRuns = 0,
        uint runsBattedIn = 0, uint baseOnBalls = 0, uint intentionalWalks = 0, uint strikeouts = 0,
        uint stolenBases = 0, uint caughtStealing = 0, uint hitByPitch = 0, uint sacrificeBunts = 0,
        uint sacrificeFlies = 0, uint numberOfPitchesSeen = 0, uint leftOnBase = 0, uint groundOuts = 0,
        uint groundIntoDoublePlays = 0, uint groundIntoTriplePlays = 0, uint airOuts = 0,
        uint catchersInterference = 0)
    {
        return PlayerBattingStatsByGame.Create(MlbId.Create(playerId), SeasonYear.Create(seasonYear),
            gameDate ?? new DateTime(2024, 4, 1), MlbId.Create(gameId), MlbId.Create(teamId),
            plateAppearances, atBats, runs, hits, doubles, triples, homeRuns, runsBattedIn, baseOnBalls,
            intentionalWalks, strikeouts, stolenBases, caughtStealing, hitByPitch, sacrificeBunts, sacrificeFlies,
            numberOfPitchesSeen, leftOnBase, groundOuts, groundIntoDoublePlays, groundIntoTriplePlays, airOuts,
            catchersInterference);
    }

    public static PlayerPitchingStatsByGame FakePitchingStats(int playerId = 1, ushort seasonYear = 2024,
        DateTime? gameDate = null, int gameId = 10000, int teamId = 100, bool win = false, bool loss = false,
        bool gameStarted = false, bool gameFinished = false, bool completeGame = false, bool shutout = false,
        bool hold = false, bool save = false, bool blownSave = false, bool saveOpportunity = false,
        decimal inningsPitched = 0, uint hits = 0, uint doubles = 0, uint triples = 0, uint homeRuns = 0, uint runs = 0,
        uint earnedRuns = 0, uint strikeouts = 0, uint baseOnBalls = 0, uint intentionalWalks = 0, uint hitBatsmen = 0,
        uint outs = 0, uint groundOuts = 0, uint airOuts = 0, uint groundIntoDoublePlays = 0, uint numberOfPitches = 0,
        uint strikes = 0, uint wildPitches = 0, uint balks = 0, uint battersFaced = 0, uint atBats = 0,
        uint stolenBases = 0, uint caughtStealing = 0, uint pickOffs = 0, uint inheritedRunners = 0,
        uint inheritedRunnersScored = 0, uint catchersInterferences = 0, uint sacrificeBunts = 0,
        uint sacrificeFlies = 0)
    {
        return PlayerPitchingStatsByGame.Create(MlbId.Create(playerId), SeasonYear.Create(seasonYear),
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
            pickOffs: pickOffs,
            inheritedRunners: inheritedRunners,
            inheritedRunnersScored: inheritedRunnersScored,
            catchersInterferences: catchersInterferences,
            sacrificeBunts: sacrificeBunts,
            sacrificeFlies: sacrificeFlies
        );
    }
}