using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PerformanceAssessment.Services;

public class MinimumRequirementsParticipationAssessorTests
{
    [Fact]
    public void AssessBatting_RequirementsMet_ReturnsTrue()
    {
        // Arrange
        var start = new DateOnly(2024, 7, 1);
        var end = new DateOnly(2024, 7, 14);
        var stats = Faker.FakeBattingStats(plateAppearances: 26);

        var service = new MinimumRequirementsParticipationAssessor();

        // Act
        var actual = service.AssessBatting(start, end, stats);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void AssessBatting_RequirementsFailed_ReturnsFalse()
    {
        // Arrange
        var start = new DateOnly(2024, 7, 1);
        var end = new DateOnly(2024, 7, 14);
        var stats = Faker.FakeBattingStats(plateAppearances: 25);

        var service = new MinimumRequirementsParticipationAssessor();

        // Act
        var actual = service.AssessBatting(start, end, stats);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void AssessPitching_RequirementsMet_ReturnsTrue()
    {
        // Arrange
        var start = new DateOnly(2024, 7, 1);
        var end = new DateOnly(2024, 7, 14);
        var stats = Faker.FakePitchingStats(battersFaced: 15);

        var service = new MinimumRequirementsParticipationAssessor();

        // Act
        var actual = service.AssessPitching(start, end, stats);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void AssessPitching_RequirementsFailed_ReturnsFalse()
    {
        // Arrange
        var start = new DateOnly(2024, 7, 1);
        var end = new DateOnly(2024, 7, 14);
        var stats = Faker.FakePitchingStats(battersFaced: 14);

        var service = new MinimumRequirementsParticipationAssessor();

        // Act
        var actual = service.AssessPitching(start, end, stats);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void AssessFielding_RequirementsMet_ReturnsTrue()
    {
        // Arrange
        var start = new DateOnly(2024, 7, 1);
        var end = new DateOnly(2024, 7, 14);
        var stats = Faker.FakeFieldingStats(putouts: 6);

        var service = new MinimumRequirementsParticipationAssessor();

        // Act
        var actual = service.AssessFielding(start, end, stats);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void AssessFielding_RequirementsFailed_ReturnsFalse()
    {
        // Arrange
        var start = new DateOnly(2024, 7, 1);
        var end = new DateOnly(2024, 7, 14);
        var stats = Faker.FakeFieldingStats(putouts: 5);

        var service = new MinimumRequirementsParticipationAssessor();

        // Act
        var actual = service.AssessFielding(start, end, stats);

        // Assert
        Assert.False(actual);
    }
}