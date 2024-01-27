using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.Entities;

public class PlayerBattingStatsByGameTests
{
    [Fact]
    public void Create_ValidValues_Created()
    {
        // Arrange
        var mlbId = MlbId.Create(1);
        var seasonYear = SeasonYear.Create(2024);
        var gameDate = new DateTime(2024, 4, 1);
        var gameId = MlbId.Create(10000);
        var teamId = MlbId.Create(100);
        uint plateAppearances = 1; // Note: Numbers are nonsensical
        uint atBats = 2;
        uint runs = 3;
        uint hits = 4;
        uint doubles = 5;
        uint triples = 6;
        uint homeRuns = 7;
        uint rbi = 8;
        uint baseOnBalls = 9;
        uint intentionalWalks = 10;
        uint strikeOuts = 11;
        uint stolenBases = 12;
        uint caughtStealing = 13;
        uint hitByPitch = 14;
        uint sacrificeBunts = 15;
        uint sacrificeFlies = 16;
        uint numberOfPitchesSeen = 17;
        uint leftOnBase = 18;
        uint groundOuts = 19;
        uint groundIntoDoublePlays = 20;
        uint groundIntoTriplePlays = 21;
        uint airOuts = 22;
        uint catchersInterference = 23;

        // Act
        var actual = PlayerBattingStatsByGame.Create(mlbId, seasonYear, gameDate, gameId, teamId, plateAppearances,
            atBats, runs, hits, doubles, triples, homeRuns, rbi, baseOnBalls, intentionalWalks, strikeOuts,
            stolenBases, caughtStealing, hitByPitch, sacrificeBunts, sacrificeFlies, numberOfPitchesSeen, leftOnBase,
            groundOuts, groundIntoDoublePlays, groundIntoTriplePlays, airOuts, catchersInterference);

        // Assert
        Assert.Equal(mlbId, actual.MlbId);
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

    [Fact]
    public void BattingAverage_RequiredStats_ReturnsCalculatedStat()
    {
        // Arrange
        uint hits = 3;
        uint atBats = 4;
        var stats = Faker.FakeBattingStats(hits: hits, atBats: atBats);

        // Act
        var actual = stats.BattingAverage;

        // Assert
        Assert.Equal("0.750", actual.AsRounded().ToString("N3"));
    }

    [Fact]
    public void OnBasePercentage_RequiredStats_ReturnsCalculatedStat()
    {
        // Arrange
        uint hits = 5;
        uint baseOnBalls = 2;
        uint hitByPitch = 1;
        uint atBats = 20;
        uint sacFlies = 6;
        var stats = Faker.FakeBattingStats(hits: hits, baseOnBalls: baseOnBalls, hitByPitch: hitByPitch,
            atBats: atBats, sacrificeFlies: sacFlies);

        // Act
        var actual = stats.OnBasePercentage;

        // Assert
        Assert.Equal("0.276", actual.AsRounded().ToString("N3"));
    }

    [Fact]
    public void TotalBases_RequiredStats_ReturnsCalculatedStat()
    {
        // Arrange
        uint hits = 12;
        uint doubles = 6;
        uint triples = 1;
        uint homeRuns = 4;
        var stats = Faker.FakeBattingStats(hits: hits, doubles: doubles, triples: triples, homeRuns: homeRuns);

        // Act
        var actual = stats.TotalBases;

        // Assert
        Assert.Equal((uint)32, actual.Value);
    }

    [Fact]
    public void Slugging_RequiredStats_ReturnsCalculatedStat()
    {
        // Arrange
        uint hits = 12;
        uint doubles = 6;
        uint triples = 1;
        uint homeRuns = 4;
        uint atBats = 60;
        var stats = Faker.FakeBattingStats(atBats: atBats, hits: hits, doubles: doubles, triples: triples,
            homeRuns: homeRuns);

        // Act
        var actual = stats.Slugging;

        // Assert
        Assert.Equal("0.533", actual.AsRounded().ToString("N3"));
    }

    [Fact]
    public void OnBasePlusSlugging_RequiredStats_ReturnsCalculatedStat()
    {
        // Arrange
        uint hits = 12;
        uint baseOnBalls = 2;
        uint hitByPitch = 1;
        uint atBats = 60;
        uint sacFlies = 6;

        uint doubles = 6;
        uint triples = 1;
        uint homeRuns = 4;
        var stats = Faker.FakeBattingStats(atBats: atBats, baseOnBalls: baseOnBalls, hitByPitch: hitByPitch, hits: hits,
            sacrificeFlies: sacFlies, doubles: doubles, triples: triples, homeRuns: homeRuns);

        // Act
        var actual = stats.OnBasePlusSlugging;

        // Assert
        Assert.Equal("0.751", actual.AsRounded().ToString("N3"));
    }

    [Fact]
    public void StolenBasePercentage_RequiredStats_ReturnsCalculatedStat()
    {
        // Arrange
        uint stolenBases = 5;
        uint caughtStealing = 6;
        var stats = Faker.FakeBattingStats(stolenBases: stolenBases, caughtStealing: caughtStealing);

        // Act
        var actual = stats.StolenBasePercentage;

        // Assert
        Assert.Equal("0.455", actual.AsRounded().ToString("N3"));
    }

    [Fact]
    public void BattingAverageOnBallsInPlay_RequiredStats_ReturnsCalculatedStat()
    {
        // Arrange
        uint hits = 8;
        uint homeRuns = 2;
        uint atBats = 30;
        uint strikeOuts = 17;
        uint sacFlies = 5;
        var stats = Faker.FakeBattingStats(hits: hits, homeRuns: homeRuns, atBats: atBats, strikeOuts: strikeOuts,
            sacrificeFlies: sacFlies);

        // Act
        var actual = stats.BattingAverageOnBallsInPlay;

        // Assert
        Assert.Equal("0.375", actual.AsRounded().ToString("N3"));
    }
}