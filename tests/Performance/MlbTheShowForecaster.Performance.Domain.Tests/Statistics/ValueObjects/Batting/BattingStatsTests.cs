using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Batting;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.TestClasses;

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
            hits: hits, sacrificeFlies: sacrificeFlies, doubles: doubles, triples: triples, homeRuns: homeRuns);

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

    [Fact]
    public void Create_BattingStatsCollection_ReturnsAggregatedStats()
    {
        // Arrange
        var stats1 = Faker.FakeBattingStats(
            plateAppearances: 1, // Every number is different to ensure no crossed variable assignment
            atBats: 2,
            runs: 3,
            hits: 4,
            doubles: 5,
            triples: 6,
            homeRuns: 7,
            runsBattedIn: 8,
            baseOnBalls: 9,
            intentionalWalks: 10,
            strikeouts: 11,
            stolenBases: 12,
            caughtStealing: 13,
            hitByPitch: 14,
            sacrificeBunts: 15,
            sacrificeFlies: 16,
            numberOfPitchesSeen: 17,
            leftOnBase: 18,
            groundOuts: 19,
            groundIntoDoublePlays: 20,
            groundIntoTriplePlays: 21,
            airOuts: 22,
            catchersInterference: 23
        );
        var stats2 = Faker.FakeBattingStats(
            plateAppearances: 1000, // Values are the previous one multiplied by 1000 to make expected values easy to calculate
            atBats: 2000,
            runs: 3000,
            hits: 4000,
            doubles: 5000,
            triples: 6000,
            homeRuns: 7000,
            runsBattedIn: 8000,
            baseOnBalls: 9000,
            intentionalWalks: 10000,
            strikeouts: 11000,
            stolenBases: 12000,
            caughtStealing: 13000,
            hitByPitch: 14000,
            sacrificeBunts: 15000,
            sacrificeFlies: 16000,
            numberOfPitchesSeen: 17000,
            leftOnBase: 18000,
            groundOuts: 19000,
            groundIntoDoublePlays: 20000,
            groundIntoTriplePlays: 21000,
            airOuts: 22000,
            catchersInterference: 23000
        );
        var statsCollection = new List<BattingStats>() { stats1, stats2 };

        // Act
        var actual = BattingStats.Create(statsCollection);

        // Assert
        Assert.Equal(1001, actual.PlateAppearances.Value);
        Assert.Equal(2002, actual.AtBats.Value);
        Assert.Equal(3003, actual.Runs.Value);
        Assert.Equal(4004, actual.Hits.Value);
        Assert.Equal(5005, actual.Doubles.Value);
        Assert.Equal(6006, actual.Triples.Value);
        Assert.Equal(7007, actual.HomeRuns.Value);
        Assert.Equal(8008, actual.RunsBattedIn.Value);
        Assert.Equal(9009, actual.BaseOnBalls.Value);
        Assert.Equal(10010, actual.IntentionalWalks.Value);
        Assert.Equal(11011, actual.Strikeouts.Value);
        Assert.Equal(12012, actual.StolenBases.Value);
        Assert.Equal(13013, actual.CaughtStealing.Value);
        Assert.Equal(14014, actual.HitByPitch.Value);
        Assert.Equal(15015, actual.SacrificeBunts.Value);
        Assert.Equal(16016, actual.SacrificeFlies.Value);
        Assert.Equal(17017, actual.NumberOfPitchesSeen.Value);
        Assert.Equal(18018, actual.LeftOnBase.Value);
        Assert.Equal(19019, actual.GroundOuts.Value);
        Assert.Equal(20020, actual.GroundIntoDoublePlays.Value);
        Assert.Equal(21021, actual.GroundIntoTriplePlays.Value);
        Assert.Equal(22022, actual.AirOuts.Value);
        Assert.Equal(23023, actual.CatchersInterference.Value);
    }
}