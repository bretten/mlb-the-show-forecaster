using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Batting;

public class BattingStatsTests
{
    [Fact]
    public void BattingAverage_RequiredStats_ReturnsCalculatedStat()
    {
        // Arrange
        const int hits = 151;
        const int atBats = 497;
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
        const int hits = 151;
        const int baseOnBalls = 91;
        const int hitByPitches = 3;
        const int atBats = 497;
        const int sacrificeFlies = 3;
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
        const int hits = 151;
        const int doubles = 26;
        const int triples = 8;
        const int homeRuns = 44;
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
        const int atBats = 497;
        const int hits = 151;
        const int doubles = 26;
        const int triples = 8;
        const int homeRuns = 44;
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
        const int hits = 151;
        const int baseOnBalls = 91;
        const int hitByPitches = 3;
        const int atBats = 497;
        const int sacrificeFlies = 3;

        const int doubles = 26;
        const int triples = 8;
        const int homeRuns = 44;
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
        const int stolenBases = 20;
        const int caughtStealing = 6;
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
        const int hits = 151;
        const int homeRuns = 44;
        const int atBats = 497;
        const int strikeouts = 143;
        const int sacrificeFlies = 3;
        var stats = Faker.FakeBattingStats(hits: hits, homeRuns: homeRuns, atBats: atBats, strikeouts: strikeouts,
            sacrificeFlies: sacrificeFlies);

        // Act
        var actual = stats.BattingAverageOnBallsInPlay;

        // Assert
        Assert.Equal(0.342m, actual.Value);
    }

    [Fact]
    public void Create_ValidValues_Created()
    {
        // Arrange
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
        var actual = BattingStats.Create(plateAppearances: plateAppearances,
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
            catchersInterference: catchersInterference
        );

        // Assert
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
        Assert.Equal(strikeouts, actual.Strikeouts.Value);
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