using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services.MinMaxNormalization.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PerformanceAssessment.Services.
    MinMaxNormalization;

public class MinMaxNormalizationPerformanceAssessorTests
{
    [Theory]
    [InlineData(80, 60, 70, 0.5455)]
    [InlineData(40, 90, 60, 0.6465)]
    [InlineData(70, 50, 80, 0.4444)]
    public void AssessBatting_BattingStats_CreatesScore(int hits, int doubles, int strikeouts, double expectedScore)
    {
        // Arrange
        var criteria = new List<MinMaxBattingStatCriteria>()
        {
            new("Hits", 0.2m, false, min: 1, max: 100),
            new("Doubles", 0.5m, false, min: 1, max: 100),
            new("Strikeouts", 0.3m, true, min: 1, max: 100) // Lower is better
        };

        var stats = Faker.FakeBattingStats(hits: hits, doubles: doubles, strikeouts: strikeouts);

        var options = new MinMaxNormalizationCriteria(criteria, BasicPitchingCriteria, BasicFieldingCriteria);
        var assessor = new MinMaxNormalizationPerformanceAssessor(options);

        // Act
        var actual = assessor.AssessBatting(stats);

        // Assert
        Assert.Equal((decimal)expectedScore, actual.Score.Value);
    }

    [Theory]
    [InlineData(80, 60, 70, 0.5455)]
    [InlineData(40, 90, 60, 0.6465)]
    [InlineData(70, 50, 80, 0.4444)]
    public void AssessPitching_PitchingStats_CreatesScore(int strikeouts, int wins, int losses, double expectedScore)
    {
        // Arrange
        var criteria = new List<MinMaxPitchingStatCriteria>()
        {
            new("Strikeouts", 0.2m, false, min: 1, max: 100),
            new("Wins", 0.5m, false, min: 1, max: 100),
            new("Losses", 0.3m, true, min: 1, max: 100) // Lower is better
        };

        var stats = Faker.FakePitchingStats(strikeouts: strikeouts, wins: wins, losses: losses);

        var options = new MinMaxNormalizationCriteria(BasicBattingCriteria, criteria, BasicFieldingCriteria);
        var assessor = new MinMaxNormalizationPerformanceAssessor(options);

        // Act
        var actual = assessor.AssessPitching(stats);

        // Assert
        Assert.Equal((decimal)expectedScore, actual.Score.Value);
    }

    [Theory]
    [InlineData(80, 60, 70, 0.5455)]
    [InlineData(40, 90, 60, 0.6465)]
    [InlineData(70, 50, 80, 0.4444)]
    public void AssessFielding_FieldingStats_CreatesScore(int assists, int putouts, int errors, double expectedScore)
    {
        // Arrange
        var criteria = new List<MinMaxFieldingStatCriteria>()
        {
            new("Assists", 0.2m, false, min: 1, max: 100),
            new("Putouts", 0.5m, false, min: 1, max: 100),
            new("Errors", 0.3m, true, min: 1, max: 100) // Lower is better
        };

        var stats = Faker.FakeFieldingStats(assists: assists, putouts: putouts, errors: errors);

        var options = new MinMaxNormalizationCriteria(BasicBattingCriteria, BasicPitchingCriteria, criteria);
        var assessor = new MinMaxNormalizationPerformanceAssessor(options);

        // Act
        var actual = assessor.AssessFielding(stats);

        // Assert
        Assert.Equal((decimal)expectedScore, actual.Score.Value);
    }

    [Fact]
    public void CalculateScore_InvalidStatType_ThrowsException()
    {
        // Arrange
        var criteria = new List<MinMaxFieldingStatCriteria>()
        {
            new("FieldingPercentage", 0.2m, false, min: 1, max: 100),
            new("InningsPlayed", 0.5m, false, min: 1, max: 100),
            new("Position", 0.3m, false, min: 1, max: 100) // Unexpected stat type
        };

        var stats = Faker.FakeFieldingStats(assists: 1, putouts: 1, errors: 1);

        var options = new MinMaxNormalizationCriteria(BasicBattingCriteria, BasicPitchingCriteria, criteria);
        var assessor = new MinMaxNormalizationPerformanceAssessor(options);

        var action = () => assessor.AssessFielding(stats);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<UnexpectedMinMaxStatTypeException>(actual);
    }

    private static readonly IReadOnlyList<MinMaxBattingStatCriteria> BasicBattingCriteria =
        new List<MinMaxBattingStatCriteria>()
        {
            new("Hits", 1m, false, min: 1, max: 100),
        };

    private static readonly IReadOnlyList<MinMaxPitchingStatCriteria> BasicPitchingCriteria =
        new List<MinMaxPitchingStatCriteria>()
        {
            new("EarnedRunAverage", 1m, false, min: 1, max: 100),
        };

    private static readonly IReadOnlyList<MinMaxFieldingStatCriteria> BasicFieldingCriteria =
        new List<MinMaxFieldingStatCriteria>()
        {
            new("FieldingPercentage", 1m, false, min: 1, max: 100),
        };
}