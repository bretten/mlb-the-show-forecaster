using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.TestClasses;

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
        const int catchersInterferences = 1;
        var stats = Faker.FakePitchingStats(hits: hits, battersFaced: battersFaced, baseOnBalls: baseOnBalls,
            hitBatsmen: hitBatsmen, sacrificeBunts: sacrificeHits, sacrificeFlies: sacrificeFlies,
            catchersInterferences: catchersInterferences);

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
        const int catchersInterferences = 37;
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
            catchersInterferences: catchersInterferences,
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
        Assert.Equal(37, actual.CatchersInterferences.Value);
        Assert.Equal(38, actual.SacrificeBunts.Value);
        Assert.Equal(39, actual.SacrificeFlies.Value);
    }
}