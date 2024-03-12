using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Fielding;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Fielding;

public class FieldingStatsTests
{
    [Fact]
    public void FieldingPercentage_AssistsPutoutsErrors_ReturnsCalculatedStat()
    {
        // Arrange
        const int assists = 276;
        const int putouts = 139;
        const int errors = 8;
        var stats = Faker.FakeFieldingStats(assists: assists, putouts: putouts, errors: errors);

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
        const int putouts = 139;
        const int errors = 8;
        var stats = Faker.FakeFieldingStats(assists: assists, putouts: putouts, errors: errors);

        // Act
        var actual = stats.TotalChances;

        // Assert
        Assert.Equal(423, actual.Value);
    }

    [Fact]
    public void RangeFactorPer9_AssistsPutoutsInnings_ReturnsCalculatedValue()
    {
        // Arrange
        const int assists = 276;
        const int putouts = 139;
        const decimal inningsPlayed = 951.2m;
        var stats = Faker.FakeFieldingStats(assists: assists, putouts: putouts, inningsPlayed: inningsPlayed);

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
        var stats = Faker.FakeFieldingStats(stolenBases: stolenBases, caughtStealing: caughtStealing);

        // Act
        var actual = stats.StolenBasePercentage;

        // Assert
        Assert.Equal(0.769m, actual.Value);
    }

    [Fact]
    public void Create_ValidValues_Created()
    {
        // Arrange
        var position = Position.FirstBase;
        const int gamesStarted = 50; // NOTE: Nonsensical stats
        const decimal inningsPlayed = 1.1m;
        const int assists = 1;
        const int putouts = 2;
        const int errors = 3;
        const int throwingErrors = 4;
        const int doublePlays = 5;
        const int triplePlays = 6;
        const int caughtStealing = 7;
        const int stolenBases = 8;
        const int passedBalls = 9;
        const int catcherInterferences = 10;
        const int wildPitches = 11;
        const int pickOffs = 12;

        // Act
        var actual = FieldingStats.Create(position: position,
            gamesStarted: gamesStarted,
            inningsPlayed: inningsPlayed,
            assists: assists,
            putouts: putouts,
            errors: errors,
            throwingErrors: throwingErrors,
            doublePlays: doublePlays,
            triplePlays: triplePlays,
            caughtStealing: caughtStealing,
            stolenBases: stolenBases,
            passedBalls: passedBalls,
            catcherInterferences: catcherInterferences,
            wildPitches: wildPitches,
            pickOffs: pickOffs
        );

        // Assert
        Assert.Equal(Position.FirstBase, actual.Position);
        Assert.Equal(50, actual.GamesStarted.Value);
        Assert.Equal(1.333m, actual.InningsPlayed.Value);
        Assert.Equal(1, actual.Assists.Value);
        Assert.Equal(2, actual.Putouts.Value);
        Assert.Equal(3, actual.Errors.Value);
        Assert.Equal(4, actual.ThrowingErrors.Value);
        Assert.Equal(5, actual.DoublePlays.Value);
        Assert.Equal(6, actual.TriplePlays.Value);
        Assert.Equal(7, actual.CaughtStealing.Value);
        Assert.Equal(8, actual.StolenBases.Value);
        Assert.Equal(9, actual.PassedBalls.Value);
        Assert.Equal(10, actual.CatcherInterferences.Value);
        Assert.Equal(11, actual.WildPitches.Value);
        Assert.Equal(12, actual.PickOffs.Value);
    }

    [Fact]
    public void Create_FieldingStatsCollection_ReturnsAggregatedStats()
    {
        // Arrange
        var stats1 = Faker.FakeFieldingStats(position: Position.RightField,
            gamesStarted: 1,
            inningsPlayed: 2,
            assists: 3,
            putouts: 4,
            errors: 5,
            throwingErrors: 6,
            doublePlays: 7,
            triplePlays: 8,
            caughtStealing: 9,
            stolenBases: 10,
            passedBalls: 11,
            catcherInterferences: 12,
            wildPitches: 13,
            pickOffs: 14
        );
        var stats2 = Faker.FakeFieldingStats(position: Position.RightField,
            gamesStarted: 1000,
            inningsPlayed: 2000,
            assists: 3000,
            putouts: 4000,
            errors: 5000,
            throwingErrors: 6000,
            doublePlays: 7000,
            triplePlays: 8000,
            caughtStealing: 9000,
            stolenBases: 10000,
            passedBalls: 11000,
            catcherInterferences: 12000,
            wildPitches: 13000,
            pickOffs: 14000
        );
        var statsCollection = new List<FieldingStats>() { stats1, stats2 };

        // Act
        var actual = FieldingStats.Create(statsCollection);

        // Assert
        Assert.Equal(Position.RightField, actual.Position);
        Assert.Equal(1001, actual.GamesStarted.Value);
        Assert.Equal(2002, actual.InningsPlayed.Value);
        Assert.Equal(3003, actual.Assists.Value);
        Assert.Equal(4004, actual.Putouts.Value);
        Assert.Equal(5005, actual.Errors.Value);
        Assert.Equal(6006, actual.ThrowingErrors.Value);
        Assert.Equal(7007, actual.DoublePlays.Value);
        Assert.Equal(8008, actual.TriplePlays.Value);
        Assert.Equal(9009, actual.CaughtStealing.Value);
        Assert.Equal(10010, actual.StolenBases.Value);
        Assert.Equal(11011, actual.PassedBalls.Value);
        Assert.Equal(12012, actual.CatcherInterferences.Value);
        Assert.Equal(13013, actual.WildPitches.Value);
        Assert.Equal(14014, actual.PickOffs.Value);
    }
}