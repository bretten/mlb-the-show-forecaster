using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

public class PlayerPitchingStatsByGameTests
{
    [Fact]
    public void Equals_SamePlayerSeasonDateGame_ReturnsTrue()
    {
        // Arrange
        var stats1 = Faker.FakePlayerPitchingStats(playerMlbId: 1, 2024, new DateTime(2024, 4, 1), 10000);
        var stats2 = Faker.FakePlayerPitchingStats(playerMlbId: 1, 2024, new DateTime(2024, 4, 1), 10000);

        // Act
        var actual = stats1.Equals(stats2);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Equals_DifferentPlayer_ReturnsFalse()
    {
        // Arrange
        var stats1 = Faker.FakePlayerPitchingStats(playerMlbId: 1, 2024, new DateTime(2024, 4, 1), 10000);
        var stats2 = Faker.FakePlayerPitchingStats(playerMlbId: 2, 2024, new DateTime(2024, 4, 1), 10000);

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
            sacrificeFlies: sacrificeFlies
        );

        // Assert
        Assert.Equal(1, actual.PlayerMlbId.Value);
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