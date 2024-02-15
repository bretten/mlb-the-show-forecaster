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
        const int earnedRuns = 3;
        var stats = Faker.FakePlayerPitchingStats(inningsPitched: inningsPitched, earnedRuns: earnedRuns);

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
        var stats = Faker.FakePlayerPitchingStats(inningsPitched: inningsPitched, earnedRuns: earnedRuns);

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
        var stats = Faker.FakePlayerPitchingStats(hits: hits, battersFaced: battersFaced, baseOnBalls: baseOnBalls,
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
        var stats = Faker.FakePlayerPitchingStats(hits: hits, baseOnBalls: baseOnBalls, hitBatsmen: hitBatsmen,
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
        var stats = Faker.FakePlayerPitchingStats(hits: hits, doubles: doubles, triples: triples, homeRuns: homeRuns);

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
        var stats = Faker.FakePlayerPitchingStats(hits: hits, doubles: doubles, triples: triples, homeRuns: homeRuns,
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
        var stats = Faker.FakePlayerPitchingStats(hits: hits, doubles: doubles, triples: triples, homeRuns: homeRuns,
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
        var stats = Faker.FakePlayerPitchingStats(numberOfPitches: numberOfPitches, inningsPitched: inningsPitched);

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
        var stats = Faker.FakePlayerPitchingStats(strikes: strikes, numberOfPitches: numberOfPitches);

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
        var stats = Faker.FakePlayerPitchingStats(baseOnBalls: baseOnBalls, hits: hits, inningsPitched: inningsPitched);

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
        var stats = Faker.FakePlayerPitchingStats(strikeouts: strikeouts, baseOnBalls: baseOnBalls);

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
        var stats = Faker.FakePlayerPitchingStats(hits: hitsAllowed, inningsPitched: inningsPitched);

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
        var stats = Faker.FakePlayerPitchingStats(strikeouts: strikeouts, inningsPitched: inningsPitched);

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
        var stats = Faker.FakePlayerPitchingStats(baseOnBalls: baseOnBalls, inningsPitched: inningsPitched);

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
        var stats = Faker.FakePlayerPitchingStats(runs: runsAllowed, inningsPitched: inningsPitched);

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
        var stats = Faker.FakePlayerPitchingStats(homeRuns: homeRunsAllowed, inningsPitched: inningsPitched);

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
        var stats = Faker.FakePlayerPitchingStats(stolenBases: stolenBases, caughtStealing: caughtStealing);

        // Act
        var actual = stats.StolenBasePercentage;

        // Assert
        Assert.Equal(0.769m, actual.Value);
    }

    [Fact]
    public void Equals_SamePlayerSeasonDateGame_ReturnsTrue()
    {
        // Arrange
        var stats1 = Faker.FakePlayerPitchingStats(playerId: 1, 2024, new DateTime(2024, 4, 1), 10000);
        var stats2 = Faker.FakePlayerPitchingStats(playerId: 1, 2024, new DateTime(2024, 4, 1), 10000);

        // Act
        var actual = stats1.Equals(stats2);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Equals_DifferentPlayer_ReturnsTrue()
    {
        // Arrange
        var stats1 = Faker.FakePlayerPitchingStats(playerId: 1, 2024, new DateTime(2024, 4, 1), 10000);
        var stats2 = Faker.FakePlayerPitchingStats(playerId: 2, 2024, new DateTime(2024, 4, 1), 10000);

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
        const bool loss = false;
        const bool gameStarted = true;
        const bool gameFinished = false;
        const bool completeGame = true;
        const bool shutOut = true;
        const bool hold = false;
        const bool save = false;
        const bool blownSave = false;
        const bool saveOpportunity = false;
        const decimal inningsPitched = 8.1m;
        const int hits = 3;
        const int runs = 5;
        const int earnedRuns = 5;
        const int homeRuns = 2;
        const int numberOfPitches = 93;
        const int hitBatsmen = 3;
        const int baseOnBalls = 2;
        const int intentionalWalks = 0;
        const int strikeouts = 8;
        const int groundOuts = 6;
        const int airOuts = 4;
        const int doubles = 1;
        const int triples = 0;
        const int groundIntoDoublePlays = 0;
        const int wildPitches = 2;
        const int balks = 0;
        const int stolenBases = 1;
        const int caughtStealing = 0;
        const int pickOffs = 0;
        const int strikes = 63;
        const int battersFaced = 26;
        const int atBats = 21;
        const int inheritedRunners = 0;
        const int inheritedRunnersScored = 0;
        const int outs = 18;
        const int catchersInterferences = 0;
        const int sacrificeBunts = 0;
        const int sacrificeFlies = 0;

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
        Assert.Equal(1, actual.Wins.Value);
        Assert.Equal(0, actual.Losses.Value);
        Assert.Equal(1, actual.GamesStarted.Value);
        Assert.Equal(0, actual.GamesFinished.Value);
        Assert.Equal(1, actual.CompleteGames.Value);
        Assert.Equal(1, actual.Shutouts.Value);
        Assert.Equal(0, actual.Holds.Value);
        Assert.Equal(0, actual.Saves.Value);
        Assert.Equal(0, actual.BlownSaves.Value);
        Assert.Equal(0, actual.SaveOpportunities.Value);
        Assert.Equal(8.333m, actual.InningsPitched.Value);
        Assert.Equal(3, actual.Hits.Value);
        Assert.Equal(5, actual.Runs.Value);
        Assert.Equal(5, actual.EarnedRuns.Value);
        Assert.Equal(2, actual.HomeRuns.Value);
        Assert.Equal(93, actual.NumberOfPitches.Value);
        Assert.Equal(3, actual.HitBatsmen.Value);
        Assert.Equal(2, actual.BaseOnBalls.Value);
        Assert.Equal(0, actual.IntentionalWalks.Value);
        Assert.Equal(8, actual.Strikeouts.Value);
        Assert.Equal(6, actual.GroundOuts.Value);
        Assert.Equal(4, actual.AirOuts.Value);
        Assert.Equal(1, actual.Doubles.Value);
        Assert.Equal(0, actual.Triples.Value);
        Assert.Equal(0, actual.GroundIntoDoublePlays.Value);
        Assert.Equal(2, actual.WildPitches.Value);
        Assert.Equal(0, actual.Balks.Value);
        Assert.Equal(1, actual.StolenBases.Value);
        Assert.Equal(0, actual.CaughtStealing.Value);
        Assert.Equal(0, actual.PickOffs.Value);
        Assert.Equal(63, actual.Strikes.Value);
        Assert.Equal(26, actual.BattersFaced.Value);
        Assert.Equal(21, actual.AtBats.Value);
        Assert.Equal(0, actual.InheritedRunners.Value);
        Assert.Equal(0, actual.InheritedRunnersScored.Value);
        Assert.Equal(18, actual.Outs.Value);
        Assert.Equal(0, actual.CatchersInterferences.Value);
        Assert.Equal(0, actual.SacrificeBunts.Value);
        Assert.Equal(0, actual.SacrificeFlies.Value);
        Assert.True(actual.PitchingResult.Win);
        Assert.False(actual.PitchingResult.Loss);
        Assert.True(actual.PitchingResult.GameStarted);
        Assert.False(actual.PitchingResult.GameFinished);
        Assert.True(actual.PitchingResult.CompleteGame);
        Assert.True(actual.PitchingResult.Shutout);
        Assert.False(actual.PitchingResult.Hold);
        Assert.False(actual.PitchingResult.Save);
        Assert.False(actual.PitchingResult.BlownSave);
        Assert.False(actual.PitchingResult.SaveOpportunity);
    }
}