using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.TestClasses;

/// <summary>
/// Creates fakes
/// </summary>
public static class Faker
{
    public static PlayerBattingStatsByGame FakeBattingStats(int mlbId = 1, ushort seasonYear = 2024,
        DateTime? gameDate = null, int gameId = 10000, int teamId = 100, uint plateAppearances = 0,
        uint atBats = 0, uint runs = 0, uint hits = 0, uint doubles = 0, uint triples = 0, uint homeRuns = 0,
        uint runsBattedIn = 0, uint baseOnBalls = 0, uint intentionalWalks = 0, uint strikeOuts = 0,
        uint stolenBases = 0, uint caughtStealing = 0, uint hitByPitch = 0, uint sacrificeBunts = 0,
        uint sacrificeFlies = 0, uint numberOfPitchesSeen = 0, uint leftOnBase = 0, uint groundOuts = 0,
        uint groundIntoDoublePlays = 0, uint groundIntoTriplePlays = 0, uint airOuts = 0,
        uint catchersInterference = 0)
    {
        return PlayerBattingStatsByGame.Create(MlbId.Create(mlbId), SeasonYear.Create(seasonYear),
            gameDate ?? new DateTime(2024, 4, 1), MlbId.Create(gameId), MlbId.Create(teamId),
            plateAppearances, atBats, runs, hits, doubles, triples, homeRuns, runsBattedIn, baseOnBalls,
            intentionalWalks, strikeOuts, stolenBases, caughtStealing, hitByPitch, sacrificeBunts, sacrificeFlies,
            numberOfPitchesSeen, leftOnBase, groundOuts, groundIntoDoublePlays, groundIntoTriplePlays, airOuts,
            catchersInterference);
    }
}