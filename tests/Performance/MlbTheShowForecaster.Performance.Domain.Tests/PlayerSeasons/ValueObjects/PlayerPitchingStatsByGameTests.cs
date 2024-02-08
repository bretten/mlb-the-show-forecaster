using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

public class PlayerPitchingStatsByGameTests
{
    [Fact]
    public void QualityStart_InningsPitchedEarnedRuns_ReturnsCalculatedStat()
    {
        // Arrange
        const decimal inningsPitched = 6;
        const uint earnedRuns = 3;
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
        const uint earnedRuns = 46U;
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
        const uint hits = 85;
        const uint battersFaced = 531;
        const uint baseOnBalls = 55;
        const uint hitBatsmen = 11;
        const uint sacrificeHits = 0;
        const uint sacrificeFlies = 1;
        const uint catchersInterferences = 1;
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
        const uint hits = 151;
        const uint baseOnBalls = 91;
        const uint hitBatsmen = 3;
        const uint atBats = 497;
        const uint sacrificeFlies = 3;
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
        const uint hits = 151;
        const uint doubles = 26;
        const uint triples = 8;
        const uint homeRuns = 44;
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
        const uint hits = 151;
        const uint doubles = 26;
        const uint triples = 8;
        const uint homeRuns = 44;
        const uint atBats = 497;
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
        const uint hits = 151;
        const uint baseOnBalls = 91;
        const uint hitBatsmen = 3;
        const uint atBats = 497;
        const uint sacrificeFlies = 3;
        const uint doubles = 26;
        const uint triples = 8;
        const uint homeRuns = 44;
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
        const uint numberOfPitches = 1993;
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
        const uint strikes = 1337;
        const uint numberOfPitches = 2094;
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
        const uint baseOnBalls = 55;
        const uint hits = 85;
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
        const uint strikeouts = 167;
        const uint baseOnBalls = 55;
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
        const uint hitsAllowed = 85;
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
        const uint strikeouts = 167;
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
        const uint baseOnBalls = 55;
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
        const uint runsAllowed = 50;
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
        const uint homeRunsAllowed = 18;
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
        const uint stolenBases = 20;
        const uint caughtStealing = 6;
        var stats = Faker.FakePitchingStats(stolenBases: stolenBases, caughtStealing: caughtStealing);

        // Act
        var actual = stats.StolenBasePercentage;

        // Assert
        Assert.Equal(0.769m, actual.Value);
    }

    [Fact]
    public void Equals_SamePlayerSeasonDateGame_ReturnsTrue()
    {
        // Arrange
        var stats1 = Faker.FakePitchingStats(playerId: 1, 2024, new DateTime(2024, 4, 1), 10000);
        var stats2 = Faker.FakePitchingStats(playerId: 1, 2024, new DateTime(2024, 4, 1), 10000);

        // Act
        var actual = stats1.Equals(stats2);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Equals_DifferentPlayer_ReturnsTrue()
    {
        // Arrange
        var stats1 = Faker.FakePitchingStats(playerId: 1, 2024, new DateTime(2024, 4, 1), 10000);
        var stats2 = Faker.FakePitchingStats(playerId: 2, 2024, new DateTime(2024, 4, 1), 10000);

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
        const bool win = true; // NOTE: Nonsensical stats
        const bool loss = true;
        const bool gameStarted = true;
        const bool gameFinished = true;
        const bool completeGame = true;
        const bool shutOut = true;
        const bool hold = true;
        const bool save = true;
        const bool blownSave = true;
        const bool saveOpportunity = true;
        const decimal inningsPitched = 8.1m;
        const uint hits = 3;
        const uint runs = 5;
        const uint earnedRuns = 5;
        const uint homeRuns = 2;
        const uint numberOfPitches = 93;
        const uint hitBatsmen = 3;
        const uint baseOnBalls = 2;
        const uint intentionalWalks = 0;
        const uint strikeouts = 8;
        const uint groundOuts = 6;
        const uint airOuts = 4;
        const uint doubles = 1;
        const uint triples = 0;
        const uint groundIntoDoublePlays = 0;
        const uint wildPitches = 2;
        const uint balks = 0;
        const uint stolenBases = 1;
        const uint caughtStealing = 0;
        const uint pickOffs = 0;
        const uint strikes = 63;
        const uint battersFaced = 26;
        const uint atBats = 21;
        const uint inheritedRunners = 0;
        const uint inheritedRunnersScored = 0;
        const uint outs = 18;
        const uint catchersInterferences = 0;
        const uint sacrificeBunts = 0;
        const uint sacrificeFlies = 0;

        // Act
        var actual = PlayerPitchingStatsByGame.Create(mlbId, seasonYear, gameDate, gameId, teamId,
            win: win,
            loss: loss,
            gameStarted: gameStarted,
            gameFinished: gameFinished,
            completeGame: completeGame,
            shutout: shutOut,
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
            sacrificeFlies: sacrificeFlies);

        // Assert
        Assert.Equal(1, actual.PlayerId.Value);
        Assert.Equal(2024, actual.SeasonYear.Value);
        Assert.Equal(new DateTime(2024, 4, 1), actual.GameDate);
        Assert.Equal(10000, actual.GameId.Value);
        Assert.Equal(100, actual.TeamId.Value);
        Assert.True(actual.Win);
        Assert.True(actual.Loss);
        Assert.True(actual.GameStarted);
        Assert.True(actual.GameFinished);
        Assert.True(actual.CompleteGame);
        Assert.True(actual.Shutout);
        Assert.True(actual.Hold);
        Assert.True(actual.Save);
        Assert.True(actual.BlownSave);
        Assert.True(actual.SaveOpportunity);
        Assert.Equal(8.333m, actual.InningsPitched.Value);
        Assert.Equal(3U, actual.Hits.Value);
        Assert.Equal(5U, actual.Runs.Value);
        Assert.Equal(5U, actual.EarnedRuns.Value);
        Assert.Equal(2U, actual.HomeRuns.Value);
        Assert.Equal(93U, actual.NumberOfPitches.Value);
        Assert.Equal(3U, actual.HitBatsmen.Value);
        Assert.Equal(2U, actual.BaseOnBalls.Value);
        Assert.Equal(0U, actual.IntentionalWalks.Value);
        Assert.Equal(8U, actual.Strikeouts.Value);
        Assert.Equal(6U, actual.GroundOuts.Value);
        Assert.Equal(4U, actual.AirOuts.Value);
        Assert.Equal(1U, actual.Doubles.Value);
        Assert.Equal(0U, actual.Triples.Value);
        Assert.Equal(0U, actual.GroundIntoDoublePlays.Value);
        Assert.Equal(2U, actual.WildPitches.Value);
        Assert.Equal(0U, actual.Balks.Value);
        Assert.Equal(1U, actual.StolenBases.Value);
        Assert.Equal(0U, actual.CaughtStealing.Value);
        Assert.Equal(0U, actual.PickOffs.Value);
        Assert.Equal(63U, actual.Strikes.Value);
        Assert.Equal(26U, actual.BattersFaced.Value);
        Assert.Equal(21U, actual.AtBats.Value);
        Assert.Equal(0U, actual.InheritedRunners.Value);
        Assert.Equal(0U, actual.InheritedRunnersScored.Value);
        Assert.Equal(18U, actual.Outs.Value);
        Assert.Equal(0U, actual.CatchersInterferences.Value);
        Assert.Equal(0U, actual.SacrificeBunts.Value);
        Assert.Equal(0U, actual.SacrificeFlies.Value);
    }
}