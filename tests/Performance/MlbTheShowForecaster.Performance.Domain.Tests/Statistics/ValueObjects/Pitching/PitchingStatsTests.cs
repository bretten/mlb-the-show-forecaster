using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Pitching;

public class PitchingStatsTests
{
    [Fact]
    public void QualityStart_InningsPitchedEarnedRuns_ReturnsCalculatedStat()
    {
        // Arrange
        const decimal inningsPitched = 6;
        const int earnedRuns = 3;
        var stats = Faker.FakePitchingStats(inningsPitched: inningsPitched, earnedRuns: earnedRuns);

        // Act
        var actual = stats.QualityStart;

        // Assert
        Assert.True(actual.Value);
    }

    [Fact]
    public void EarnedRunAverage_EarnedRunsInningsPitched_ReturnsCalculatedStat()
    {
        // Arrange
        const int earnedRuns = 46;
        const decimal inningsPitched = 132m;
        var stats = Faker.FakePitchingStats(inningsPitched: inningsPitched, earnedRuns: earnedRuns);

        // Act
        var actual = stats.EarnedRunAverage;

        // Assert
        Assert.Equal(3.14m, actual.Value);
    }

    [Fact]
    public void OpponentsBattingAverage_HitsBattersWalksSacrificesInterferences_ReturnsCalculatedValue()
    {
        // Arrange
        const int hits = 85;
        const int battersFaced = 531;
        const int baseOnBalls = 55;
        const int hitBatsmen = 11;
        const int sacrificeHits = 0;
        const int sacrificeFlies = 1;
        const int catcherInterferences = 1;
        var stats = Faker.FakePitchingStats(hits: hits, battersFaced: battersFaced, baseOnBalls: baseOnBalls,
            hitBatsmen: hitBatsmen, sacrificeBunts: sacrificeHits, sacrificeFlies: sacrificeFlies,
            catcherInterferences: catcherInterferences);

        // Act
        var actual = stats.OpponentsBattingAverage;

        // Assert
        Assert.Equal(0.184m, actual.Value);
    }

    [Fact]
    public void OpponentsOnBasePercentage_HitsWalksHitByPitchesAtBatsSacFlies_ReturnsCalculatedValue()
    {
        // Arrange
        const int hits = 151;
        const int baseOnBalls = 91;
        const int hitBatsmen = 3;
        const int atBats = 497;
        const int sacrificeFlies = 3;
        var stats = Faker.FakePitchingStats(hits: hits, baseOnBalls: baseOnBalls, hitBatsmen: hitBatsmen,
            atBats: atBats, sacrificeFlies: sacrificeFlies);

        // Act
        var actual = stats.OpponentsOnBasePercentage;

        // Assert
        Assert.Equal(0.412m, actual.Value);
    }

    [Fact]
    public void TotalBases_HitsDoublesTriplesHomeRuns_ReturnsCalculatedValue()
    {
        // Arrange
        const int hits = 151;
        const int doubles = 26;
        const int triples = 8;
        const int homeRuns = 44;
        var stats = Faker.FakePitchingStats(hits: hits, doubles: doubles, triples: triples, homeRuns: homeRuns);

        // Act
        var actual = stats.TotalBases;

        // Assert
        Assert.Equal(325, actual.Value);
    }

    [Fact]
    public void Slugging_TotalBasesAtBats_ReturnsCalculatedValue()
    {
        // Arrange
        const int hits = 151;
        const int doubles = 26;
        const int triples = 8;
        const int homeRuns = 44;
        const int atBats = 497;
        var stats = Faker.FakePitchingStats(hits: hits, doubles: doubles, triples: triples, homeRuns: homeRuns,
            atBats: atBats);

        // Act
        var actual = stats.Slugging;

        // Assert
        Assert.Equal(0.654m, actual.Value);
    }

    [Fact]
    public void OpponentsOnBasePlusSlugging_OnBaseSlugging_ReturnsCalculatedValue()
    {
        // Arrange
        const int hits = 151;
        const int baseOnBalls = 91;
        const int hitBatsmen = 3;
        const int atBats = 497;
        const int sacrificeFlies = 3;
        const int doubles = 26;
        const int triples = 8;
        const int homeRuns = 44;
        var stats = Faker.FakePitchingStats(hits: hits, doubles: doubles, triples: triples, homeRuns: homeRuns,
            baseOnBalls: baseOnBalls, hitBatsmen: hitBatsmen, atBats: atBats, sacrificeFlies: sacrificeFlies);

        // Act
        var actual = stats.OpponentsOnBasePlusSlugging;

        // Assert
        Assert.Equal(1.066m, actual.Value);
    }

    [Fact]
    public void PitchesPerInning_PitchesInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const int numberOfPitches = 1993;
        const decimal inningsPitched = 117 + (decimal)2 / 3;
        var stats = Faker.FakePitchingStats(numberOfPitches: numberOfPitches, inningsPitched: inningsPitched);

        // Act
        var actual = stats.PitchesPerInning;

        // Assert
        Assert.Equal(16.938m, actual.Value);
    }

    [Fact]
    public void StrikePercentage_StrikesPitches_ReturnsValue()
    {
        // Arrange
        const int strikes = 1337;
        const int numberOfPitches = 2094;
        var stats = Faker.FakePitchingStats(strikes: strikes, numberOfPitches: numberOfPitches);

        // Act
        var actual = stats.StrikePercentage;

        // Assert
        Assert.Equal(0.638m, actual.Value);
    }

    [Fact]
    public void WalksPlusHitsPerInningPitched_WalksHitsInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const int baseOnBalls = 55;
        const int hits = 85;
        const decimal inningsPitched = 132;
        var stats = Faker.FakePitchingStats(baseOnBalls: baseOnBalls, hits: hits, inningsPitched: inningsPitched);

        // Act
        var actual = stats.WalksPlusHitsPerInningPitched;

        // Assert
        Assert.Equal(1.061m, actual.Value);
    }

    [Fact]
    public void StrikeoutToWalkRatio_StrikeoutsWalks_ReturnsCalculatedValue()
    {
        // Arrange
        const int strikeouts = 167;
        const int baseOnBalls = 55;
        var stats = Faker.FakePitchingStats(strikeouts: strikeouts, baseOnBalls: baseOnBalls);

        // Act
        var actual = stats.StrikeoutToWalkRatio;

        // Assert
        Assert.Equal(3.036m, actual.Value);
    }

    [Fact]
    public void HitsPer9_HitsInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const int hitsAllowed = 85;
        const decimal inningsPitched = 132;
        var stats = Faker.FakePitchingStats(hits: hitsAllowed, inningsPitched: inningsPitched);

        // Act
        var actual = stats.HitsPer9;

        // Assert
        Assert.Equal(5.795m, actual.Value);
    }

    [Fact]
    public void StrikeoutsPer9_StrikeoutsInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const int strikeouts = 167;
        const decimal inningsPitched = 132;
        var stats = Faker.FakePitchingStats(strikeouts: strikeouts, inningsPitched: inningsPitched);

        // Act
        var actual = stats.StrikeoutsPer9;

        // Assert
        Assert.Equal(11.386m, actual.Value);
    }

    [Fact]
    public void BaseOnBallsPer9_WalksInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const int baseOnBalls = 55;
        const decimal inningsPitched = 132;
        var stats = Faker.FakePitchingStats(baseOnBalls: baseOnBalls, inningsPitched: inningsPitched);

        // Act
        var actual = stats.BaseOnBallsPer9;

        // Assert
        Assert.Equal(3.750m, actual.Value);
    }

    [Fact]
    public void RunsScoredPer9_RunsInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const int runsAllowed = 50;
        const decimal inningsPitched = 132;
        var stats = Faker.FakePitchingStats(runs: runsAllowed, inningsPitched: inningsPitched);

        // Act
        var actual = stats.RunsScoredPer9;

        // Assert
        Assert.Equal(3.409m, actual.Value);
    }

    [Fact]
    public void HomeRunsPer9_HomeRunsInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const int homeRunsAllowed = 18;
        const decimal inningsPitched = 132;
        var stats = Faker.FakePitchingStats(homeRuns: homeRunsAllowed, inningsPitched: inningsPitched);

        // Act
        var actual = stats.HomeRunsPer9;

        // Assert
        Assert.Equal(1.227m, actual.Value);
    }

    [Fact]
    public void StolenBasePercentage_StolenBasesCaughtStealing_ReturnsCalculatedValue()
    {
        // Arrange
        const int stolenBases = 20;
        const int caughtStealing = 6;
        var stats = Faker.FakePitchingStats(stolenBases: stolenBases, caughtStealing: caughtStealing);

        // Act
        var actual = stats.StolenBasePercentage;

        // Assert
        Assert.Equal(0.769m, actual.Value);
    }

    [Fact]
    public void Create_ValidValues_Created()
    {
        // Arrange
        const int wins = 1;
        const int losses = 2;
        const int gamesStarted = 3;
        const int gamesFinished = 4;
        const int completeGames = 5;
        const int shutouts = 6;
        const int holds = 7;
        const int saves = 8;
        const int blownSaves = 9;
        const int saveOpportunities = 10;
        const decimal inningsPitched = 11.1m;
        const int hits = 12;
        const int doubles = 13;
        const int triples = 14;
        const int homeRuns = 15;
        const int runs = 16;
        const int earnedRuns = 17;
        const int strikeouts = 18;
        const int baseOnBalls = 19;
        const int intentionalWalks = 20;
        const int hitBatsmen = 21;
        const int outs = 22;
        const int groundOuts = 23;
        const int airOuts = 24;
        const int groundIntoDoublePlays = 25;
        const int numberOfPitches = 26;
        const int strikes = 27;
        const int wildPitches = 28;
        const int balks = 29;
        const int battersFaced = 30;
        const int atBats = 31;
        const int stolenBases = 32;
        const int caughtStealing = 33;
        const int pickOffs = 34;
        const int inheritedRunners = 35;
        const int inheritedRunnersScored = 36;
        const int catcherInterferences = 37;
        const int sacrificeBunts = 38;
        const int sacrificeFlies = 39;

        // Act
        var actual = PitchingStats.Create(wins: wins,
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
            catcherInterferences: catcherInterferences,
            sacrificeBunts: sacrificeBunts,
            sacrificeFlies: sacrificeFlies
        );

        // Assert
        Assert.Equal(1, actual.Wins.Value);
        Assert.Equal(2, actual.Losses.Value);
        Assert.Equal(3, actual.GamesStarted.Value);
        Assert.Equal(4, actual.GamesFinished.Value);
        Assert.Equal(5, actual.CompleteGames.Value);
        Assert.Equal(6, actual.Shutouts.Value);
        Assert.Equal(7, actual.Holds.Value);
        Assert.Equal(8, actual.Saves.Value);
        Assert.Equal(9, actual.BlownSaves.Value);
        Assert.Equal(10, actual.SaveOpportunities.Value);
        Assert.Equal(11.333m, actual.InningsPitched.Value);
        Assert.Equal(12, actual.Hits.Value);
        Assert.Equal(13, actual.Doubles.Value);
        Assert.Equal(14, actual.Triples.Value);
        Assert.Equal(15, actual.HomeRuns.Value);
        Assert.Equal(16, actual.Runs.Value);
        Assert.Equal(17, actual.EarnedRuns.Value);
        Assert.Equal(18, actual.Strikeouts.Value);
        Assert.Equal(19, actual.BaseOnBalls.Value);
        Assert.Equal(20, actual.IntentionalWalks.Value);
        Assert.Equal(21, actual.HitBatsmen.Value);
        Assert.Equal(22, actual.Outs.Value);
        Assert.Equal(23, actual.GroundOuts.Value);
        Assert.Equal(24, actual.AirOuts.Value);
        Assert.Equal(25, actual.GroundIntoDoublePlays.Value);
        Assert.Equal(26, actual.NumberOfPitches.Value);
        Assert.Equal(27, actual.Strikes.Value);
        Assert.Equal(28, actual.WildPitches.Value);
        Assert.Equal(29, actual.Balks.Value);
        Assert.Equal(30, actual.BattersFaced.Value);
        Assert.Equal(31, actual.AtBats.Value);
        Assert.Equal(32, actual.StolenBases.Value);
        Assert.Equal(33, actual.CaughtStealing.Value);
        Assert.Equal(34, actual.PickOffs.Value);
        Assert.Equal(35, actual.InheritedRunners.Value);
        Assert.Equal(36, actual.InheritedRunnersScored.Value);
        Assert.Equal(37, actual.CatcherInterferences.Value);
        Assert.Equal(38, actual.SacrificeBunts.Value);
        Assert.Equal(39, actual.SacrificeFlies.Value);
    }

    [Fact]
    public void Create_PitchingStatsCollection_ReturnsAggregatedStats()
    {
        // Arrange
        var stats1 = Faker.FakePitchingStats(wins: 1,
            losses: 2,
            gamesStarted: 3,
            gamesFinished: 4,
            completeGames: 5,
            shutouts: 6,
            holds: 7,
            saves: 8,
            blownSaves: 9,
            saveOpportunities: 10,
            inningsPitched: 11,
            hits: 12,
            doubles: 13,
            triples: 14,
            homeRuns: 15,
            runs: 16,
            earnedRuns: 17,
            strikeouts: 18,
            baseOnBalls: 19,
            intentionalWalks: 20,
            hitBatsmen: 21,
            outs: 22,
            groundOuts: 23,
            airOuts: 24,
            groundIntoDoublePlays: 25,
            numberOfPitches: 26,
            strikes: 27,
            wildPitches: 28,
            balks: 29,
            battersFaced: 30,
            atBats: 31,
            stolenBases: 32,
            caughtStealing: 33,
            pickOffs: 34,
            inheritedRunners: 35,
            inheritedRunnersScored: 36,
            catcherInterferences: 37,
            sacrificeBunts: 38,
            sacrificeFlies: 39
        );
        var stats2 = Faker.FakePitchingStats(wins: 1000,
            losses: 2000,
            gamesStarted: 3000,
            gamesFinished: 4000,
            completeGames: 5000,
            shutouts: 6000,
            holds: 7000,
            saves: 8000,
            blownSaves: 9000,
            saveOpportunities: 10000,
            inningsPitched: 11000,
            hits: 12000,
            doubles: 13000,
            triples: 14000,
            homeRuns: 15000,
            runs: 16000,
            earnedRuns: 17000,
            strikeouts: 18000,
            baseOnBalls: 19000,
            intentionalWalks: 20000,
            hitBatsmen: 21000,
            outs: 22000,
            groundOuts: 23000,
            airOuts: 24000,
            groundIntoDoublePlays: 25000,
            numberOfPitches: 26000,
            strikes: 27000,
            wildPitches: 28000,
            balks: 29000,
            battersFaced: 30000,
            atBats: 31000,
            stolenBases: 32000,
            caughtStealing: 33000,
            pickOffs: 34000,
            inheritedRunners: 35000,
            inheritedRunnersScored: 36000,
            catcherInterferences: 37000,
            sacrificeBunts: 38000,
            sacrificeFlies: 39000
        );
        var statsCollection = new List<PitchingStats>() { stats1, stats2 };

        // Act
        var actual = PitchingStats.Create(statsCollection);

        // Assert
        Assert.Equal(1001, actual.Wins.Value);
        Assert.Equal(2002, actual.Losses.Value);
        Assert.Equal(3003, actual.GamesStarted.Value);
        Assert.Equal(4004, actual.GamesFinished.Value);
        Assert.Equal(5005, actual.CompleteGames.Value);
        Assert.Equal(6006, actual.Shutouts.Value);
        Assert.Equal(7007, actual.Holds.Value);
        Assert.Equal(8008, actual.Saves.Value);
        Assert.Equal(9009, actual.BlownSaves.Value);
        Assert.Equal(10010, actual.SaveOpportunities.Value);
        Assert.Equal(11011, actual.InningsPitched.Value);
        Assert.Equal(12012, actual.Hits.Value);
        Assert.Equal(13013, actual.Doubles.Value);
        Assert.Equal(14014, actual.Triples.Value);
        Assert.Equal(15015, actual.HomeRuns.Value);
        Assert.Equal(16016, actual.Runs.Value);
        Assert.Equal(17017, actual.EarnedRuns.Value);
        Assert.Equal(18018, actual.Strikeouts.Value);
        Assert.Equal(19019, actual.BaseOnBalls.Value);
        Assert.Equal(20020, actual.IntentionalWalks.Value);
        Assert.Equal(21021, actual.HitBatsmen.Value);
        Assert.Equal(22022, actual.Outs.Value);
        Assert.Equal(23023, actual.GroundOuts.Value);
        Assert.Equal(24024, actual.AirOuts.Value);
        Assert.Equal(25025, actual.GroundIntoDoublePlays.Value);
        Assert.Equal(26026, actual.NumberOfPitches.Value);
        Assert.Equal(27027, actual.Strikes.Value);
        Assert.Equal(28028, actual.WildPitches.Value);
        Assert.Equal(29029, actual.Balks.Value);
        Assert.Equal(30030, actual.BattersFaced.Value);
        Assert.Equal(31031, actual.AtBats.Value);
        Assert.Equal(32032, actual.StolenBases.Value);
        Assert.Equal(33033, actual.CaughtStealing.Value);
        Assert.Equal(34034, actual.PickOffs.Value);
        Assert.Equal(35035, actual.InheritedRunners.Value);
        Assert.Equal(36036, actual.InheritedRunnersScored.Value);
        Assert.Equal(37037, actual.CatcherInterferences.Value);
        Assert.Equal(38038, actual.SacrificeBunts.Value);
        Assert.Equal(39039, actual.SacrificeFlies.Value);
    }
}