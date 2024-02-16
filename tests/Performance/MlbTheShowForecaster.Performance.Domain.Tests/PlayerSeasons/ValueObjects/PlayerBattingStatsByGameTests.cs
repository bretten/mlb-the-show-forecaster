using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

public class PlayerBattingStatsByGameTests
{
    [Fact]
    public void Equals_SamePlayerSeasonDateGame_ReturnsTrue()
    {
        // Arrange
        var stats1 = Faker.FakePlayerBattingStats(playerId: 1, 2024, new DateTime(2024, 4, 1), 10000);
        var stats2 = Faker.FakePlayerBattingStats(playerId: 1, 2024, new DateTime(2024, 4, 1), 10000);

        // Act
        var actual = stats1.Equals(stats2);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Equals_DifferentPlayer_ReturnsTrue()
    {
        // Arrange
        var stats1 = Faker.FakePlayerBattingStats(playerId: 1, 2024, new DateTime(2024, 4, 1), 10000);
        var stats2 = Faker.FakePlayerBattingStats(playerId: 2, 2024, new DateTime(2024, 4, 1), 10000);

        // Act
        var actual = stats1.Equals(stats2);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Create_ValidValues_Created()
    {
        // Arrange
        var mlbId = MlbId.Create(1);
        var seasonYear = SeasonYear.Create(2024);
        var gameDate = new DateTime(2024, 4, 1);
        var gameId = MlbId.Create(10000);
        var teamId = MlbId.Create(100);
        const int plateAppearances = 1; // Note: Numbers are nonsensical
        const int atBats = 2;
        const int runs = 3;
        const int hits = 4;
        const int doubles = 5;
        const int triples = 6;
        const int homeRuns = 7;
        const int rbi = 8;
        const int baseOnBalls = 9;
        const int intentionalWalks = 10;
        const int strikeouts = 11;
        const int stolenBases = 12;
        const int caughtStealing = 13;
        const int hitByPitch = 14;
        const int sacrificeBunts = 15;
        const int sacrificeFlies = 16;
        const int numberOfPitchesSeen = 17;
        const int leftOnBase = 18;
        const int groundOuts = 19;
        const int groundIntoDoublePlays = 20;
        const int groundIntoTriplePlays = 21;
        const int airOuts = 22;
        const int catchersInterference = 23;

        // Act
        var actual = PlayerBattingStatsByGame.Create(mlbId, seasonYear, gameDate, gameId, teamId,
            plateAppearances: plateAppearances,
            atBats: atBats,
            runs: runs,
            hits: hits,
            doubles: doubles,
            triples: triples,
            homeRuns: homeRuns,
            runsBattedIn: rbi,
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
            catchersInterference: catchersInterference);

        // Assert
        Assert.Equal(mlbId, actual.PlayerId);
        Assert.Equal(seasonYear, actual.SeasonYear);
        Assert.Equal(gameDate, actual.GameDate);
        Assert.Equal(gameId, actual.GameId);
        Assert.Equal(teamId, actual.TeamId);
        Assert.Equal(plateAppearances, actual.PlateAppearances.Value);
        Assert.Equal(atBats, actual.AtBats.Value);
        Assert.Equal(runs, actual.Runs.Value);
        Assert.Equal(hits, actual.Hits.Value);
        Assert.Equal(doubles, actual.Doubles.Value);
        Assert.Equal(triples, actual.Triples.Value);
        Assert.Equal(homeRuns, actual.HomeRuns.Value);
        Assert.Equal(rbi, actual.RunsBattedIn.Value);
        Assert.Equal(baseOnBalls, actual.BaseOnBalls.Value);
        Assert.Equal(intentionalWalks, actual.IntentionalWalks.Value);
        Assert.Equal(stolenBases, actual.StolenBases.Value);
        Assert.Equal(caughtStealing, actual.CaughtStealing.Value);
        Assert.Equal(hitByPitch, actual.HitByPitch.Value);
        Assert.Equal(sacrificeBunts, actual.SacrificeBunts.Value);
        Assert.Equal(sacrificeFlies, actual.SacrificeFlies.Value);
        Assert.Equal(numberOfPitchesSeen, actual.NumberOfPitchesSeen.Value);
        Assert.Equal(leftOnBase, actual.LeftOnBase.Value);
        Assert.Equal(groundOuts, actual.GroundOuts.Value);
        Assert.Equal(groundIntoDoublePlays, actual.GroundIntoDoublePlays.Value);
        Assert.Equal(groundIntoTriplePlays, actual.GroundIntoTriplePlays.Value);
        Assert.Equal(airOuts, actual.AirOuts.Value);
        Assert.Equal(catchersInterference, actual.CatchersInterference.Value);
    }
}