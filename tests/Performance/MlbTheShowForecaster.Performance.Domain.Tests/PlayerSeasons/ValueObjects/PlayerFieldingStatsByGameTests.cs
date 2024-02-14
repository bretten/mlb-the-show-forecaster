using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PlayerSeasons.ValueObjects;

public class PlayerFieldingStatsByGameTests
{
    [Fact]
    public void FieldingPercentage_AssistsPutoutsErrors_ReturnsCalculatedStat()
    {
        // Arrange
        const int assists = 276;
        const int putOuts = 139;
        const int errors = 8;
        var stats = Faker.FakePlayerFieldingStats(assists: assists, putOuts: putOuts, errors: errors);

        // Act
        var actual = stats.FieldingPercentage;

        // Assert
        Assert.Equal(0.981m, actual.Value);
    }

    [Fact]
    public void TotalChances_AssistsPutoutsErrors_ReturnsCalculatedStat()
    {
        // Arrange
        const int assists = 276;
        const int putOuts = 139;
        const int errors = 8;
        var stats = Faker.FakePlayerFieldingStats(assists: assists, putOuts: putOuts, errors: errors);

        // Act
        var actual = stats.TotalChances;

        // Assert
        Assert.Equal(423, actual.Value);
    }

    [Fact]
    public void RangeFactorPer9_AssistsPutOutsInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const int assists = 276;
        const int putOuts = 139;
        const decimal inningsPlayed = 951.2m;
        var stats = Faker.FakePlayerFieldingStats(assists: assists, putOuts: putOuts, inningsPlayed: inningsPlayed);

        // Act
        var actual = stats.RangeFactorPer9;

        // Assert
        Assert.Equal(3.925m, actual.Value);
    }

    [Fact]
    public void StolenBasePercentage_StolenBasesCaughtStealing_ReturnsCalculatedValue()
    {
        // Arrange
        const int stolenBases = 20;
        const int caughtStealing = 6;
        var stats = Faker.FakePlayerFieldingStats(stolenBases: stolenBases, caughtStealing: caughtStealing);

        // Act
        var actual = stats.StolenBasePercentage;

        // Assert
        Assert.Equal(0.769m, actual.Value);
    }

    [Fact]
    public void Equals_SamePlayerSeasonDateGame_ReturnsTrue()
    {
        // Arrange
        var stats1 = Faker.FakePlayerFieldingStats(playerId: 1, 2024, new DateTime(2024, 4, 1), 10000);
        var stats2 = Faker.FakePlayerFieldingStats(playerId: 1, 2024, new DateTime(2024, 4, 1), 10000);

        // Act
        var actual = stats1.Equals(stats2);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Equals_DifferentPlayer_ReturnsTrue()
    {
        // Arrange
        var stats1 = Faker.FakePlayerFieldingStats(playerId: 1, 2024, new DateTime(2024, 4, 1), 10000);
        var stats2 = Faker.FakePlayerFieldingStats(playerId: 2, 2024, new DateTime(2024, 4, 1), 10000);

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
        var position = Position.FirstBase;
        const bool gameStarted = true; // NOTE: Nonsensical stats
        const decimal inningsPlayed = 1.1m;
        const int assists = 1;
        const int putOuts = 2;
        const int errors = 3;
        const int throwingErrors = 4;
        const int doublePlays = 5;
        const int triplePlays = 6;
        const int caughtStealing = 7;
        const int stolenBases = 8;
        const int passedBalls = 9;
        const int catchersInterference = 10;
        const int wildPitches = 11;
        const int pickOffs = 12;

        // Act
        var actual = PlayerFieldingStatsByGame.Create(mlbId, seasonYear, gameDate, gameId, teamId,
            position: position,
            gameStarted: gameStarted,
            inningsPlayed: inningsPlayed,
            assists: assists,
            putOuts: putOuts,
            errors: errors,
            throwingErrors: throwingErrors,
            doublePlays: doublePlays,
            triplePlays: triplePlays,
            caughtStealing: caughtStealing,
            stolenBases: stolenBases,
            passedBalls: passedBalls,
            catchersInterference: catchersInterference,
            wildPitches: wildPitches,
            pickOffs: pickOffs);

        // Assert
        Assert.Equal(1, actual.PlayerId.Value);
        Assert.Equal(2024, actual.SeasonYear.Value);
        Assert.Equal(new DateTime(2024, 4, 1), actual.GameDate);
        Assert.Equal(10000, actual.GameId.Value);
        Assert.Equal(100, actual.TeamId.Value);
        Assert.Equal(Position.FirstBase, actual.Position);
        Assert.Equal(1, actual.GamesStarted.Value);
        Assert.Equal(1.333m, actual.InningsPlayed.Value);
        Assert.Equal(1, actual.Assists.Value);
        Assert.Equal(2, actual.PutOuts.Value);
        Assert.Equal(3, actual.Errors.Value);
        Assert.Equal(4, actual.ThrowingErrors.Value);
        Assert.Equal(5, actual.DoublePlays.Value);
        Assert.Equal(6, actual.TriplePlays.Value);
        Assert.Equal(7, actual.CaughtStealing.Value);
        Assert.Equal(8, actual.StolenBases.Value);
        Assert.Equal(9, actual.PassedBalls.Value);
        Assert.Equal(10, actual.CatchersInterference.Value);
        Assert.Equal(11, actual.WildPitches.Value);
        Assert.Equal(12, actual.PickOffs.Value);
    }
}