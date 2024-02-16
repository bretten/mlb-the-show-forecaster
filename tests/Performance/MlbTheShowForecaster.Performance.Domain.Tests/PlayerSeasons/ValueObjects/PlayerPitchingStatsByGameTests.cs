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