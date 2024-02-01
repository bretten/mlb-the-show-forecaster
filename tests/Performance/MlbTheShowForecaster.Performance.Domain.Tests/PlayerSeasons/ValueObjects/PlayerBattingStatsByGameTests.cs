﻿using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

public class PlayerBattingStatsByGameTests
{
    [Fact]
    public void BattingAverage_RequiredStats_ReturnsCalculatedStat()
    {
        // Arrange
        const uint hits = 151;
        const uint atBats = 497;
        var stats = Faker.FakeBattingStats(hits: hits, atBats: atBats);

        // Act
        var actual = stats.BattingAverage;

        // Assert
        Assert.Equal(0.304m, actual.Value);
    }

    [Fact]
    public void OnBasePercentage_RequiredStats_ReturnsCalculatedStat()
    {
        // Arrange
        const uint hits = 151;
        const uint baseOnBalls = 91;
        const uint hitByPitches = 3;
        const uint atBats = 497;
        const uint sacrificeFlies = 3;
        var stats = Faker.FakeBattingStats(hits: hits, baseOnBalls: baseOnBalls, hitByPitch: hitByPitches,
            atBats: atBats, sacrificeFlies: sacrificeFlies);

        // Act
        var actual = stats.OnBasePercentage;

        // Assert
        Assert.Equal(0.412m, actual.Value);
    }

    [Fact]
    public void TotalBases_RequiredStats_ReturnsCalculatedStat()
    {
        // Arrange
        const uint hits = 151;
        const uint doubles = 26;
        const uint triples = 8;
        const uint homeRuns = 44;
        var stats = Faker.FakeBattingStats(hits: hits, doubles: doubles, triples: triples, homeRuns: homeRuns);

        // Act
        var actual = stats.TotalBases;

        // Assert
        Assert.Equal(325, actual.Value);
    }

    [Fact]
    public void Slugging_RequiredStats_ReturnsCalculatedStat()
    {
        // Arrange
        const uint atBats = 497;
        const uint hits = 151;
        const uint doubles = 26;
        const uint triples = 8;
        const uint homeRuns = 44;
        var stats = Faker.FakeBattingStats(atBats: atBats, hits: hits, doubles: doubles, triples: triples,
            homeRuns: homeRuns);

        // Act
        var actual = stats.Slugging;

        // Assert
        Assert.Equal(0.654m, actual.Value);
    }

    [Fact]
    public void OnBasePlusSlugging_RequiredStats_ReturnsCalculatedStat()
    {
        // Arrange
        const uint hits = 151;
        const uint baseOnBalls = 91;
        const uint hitByPitches = 3;
        const uint atBats = 497;
        const uint sacrificeFlies = 3;

        const uint doubles = 26;
        const uint triples = 8;
        const uint homeRuns = 44;
        var stats = Faker.FakeBattingStats(atBats: atBats, baseOnBalls: baseOnBalls, hitByPitch: hitByPitches,
            hits: hits,
            sacrificeFlies: sacrificeFlies, doubles: doubles, triples: triples, homeRuns: homeRuns);

        // Act
        var actual = stats.OnBasePlusSlugging;

        // Assert
        Assert.Equal(1.066m, actual.Value);
    }

    [Fact]
    public void StolenBasePercentage_RequiredStats_ReturnsCalculatedStat()
    {
        // Arrange
        const uint stolenBases = 20;
        const uint caughtStealing = 6;
        var stats = Faker.FakeBattingStats(stolenBases: stolenBases, caughtStealing: caughtStealing);

        // Act
        var actual = stats.StolenBasePercentage;

        // Assert
        Assert.Equal(0.769m, actual.Value);
    }

    [Fact]
    public void BattingAverageOnBallsInPlay_RequiredStats_ReturnsCalculatedStat()
    {
        // Arrange
        const uint hits = 151;
        const uint homeRuns = 44;
        const uint atBats = 497;
        const uint strikeouts = 143;
        const uint sacrificeFlies = 3;
        var stats = Faker.FakeBattingStats(hits: hits, homeRuns: homeRuns, atBats: atBats, strikeouts: strikeouts,
            sacrificeFlies: sacrificeFlies);

        // Act
        var actual = stats.BattingAverageOnBallsInPlay;

        // Assert
        Assert.Equal(0.342m, actual.Value);
    }

    [Fact]
    public void Equals_SamePlayerSeasonDateGame_ReturnsTrue()
    {
        // Arrange
        var stats1 = Faker.FakeBattingStats(playerId: 1, 2024, new DateTime(2024, 4, 1), 10000);
        var stats2 = Faker.FakeBattingStats(playerId: 1, 2024, new DateTime(2024, 4, 1), 10000);

        // Act
        var actual = stats1.Equals(stats2);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Equals_DifferentPlayer_ReturnsTrue()
    {
        // Arrange
        var stats1 = Faker.FakeBattingStats(playerId: 1, 2024, new DateTime(2024, 4, 1), 10000);
        var stats2 = Faker.FakeBattingStats(playerId: 2, 2024, new DateTime(2024, 4, 1), 10000);

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
        const uint plateAppearances = 1; // Note: Numbers are nonsensical
        const uint atBats = 2;
        const uint runs = 3;
        const uint hits = 4;
        const uint doubles = 5;
        const uint triples = 6;
        const uint homeRuns = 7;
        const uint rbi = 8;
        const uint baseOnBalls = 9;
        const uint intentionalWalks = 10;
        const uint strikeouts = 11;
        const uint stolenBases = 12;
        const uint caughtStealing = 13;
        const uint hitByPitch = 14;
        const uint sacrificeBunts = 15;
        const uint sacrificeFlies = 16;
        const uint numberOfPitchesSeen = 17;
        const uint leftOnBase = 18;
        const uint groundOuts = 19;
        const uint groundIntoDoublePlays = 20;
        const uint groundIntoTriplePlays = 21;
        const uint airOuts = 22;
        const uint catchersInterference = 23;

        // Act
        var actual = PlayerBattingStatsByGame.Create(mlbId, seasonYear, gameDate, gameId, teamId, plateAppearances,
            atBats, runs, hits, doubles, triples, homeRuns, rbi, baseOnBalls, intentionalWalks, strikeouts,
            stolenBases, caughtStealing, hitByPitch, sacrificeBunts, sacrificeFlies, numberOfPitchesSeen, leftOnBase,
            groundOuts, groundIntoDoublePlays, groundIntoTriplePlays, airOuts, catchersInterference);

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