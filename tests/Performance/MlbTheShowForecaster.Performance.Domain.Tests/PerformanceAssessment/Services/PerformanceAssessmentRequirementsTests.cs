using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Shared;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.PerformanceAssessment.Services;

public class PerformanceAssessmentRequirementsTests
{
    [Fact]
    public void AreBattingAssessmentRequirementsMet_EqualToRequiredStats_ReturnsTrue()
    {
        // Arrange
        const int plateAppearances = 15;
        var requirements = new PerformanceAssessmentRequirements(statPercentChangeThreshold: 20,
            minimumPlateAppearances: NaturalNumber.Create(15),
            minimumInningsPitched: InningsCount.Create(6),
            minimumBattersFaced: NaturalNumber.Create(25),
            minimumTotalChances: NaturalNumber.Create(30)
        );

        // Act
        var actual = requirements.AreBattingAssessmentRequirementsMet(NaturalNumber.Create(plateAppearances));

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void AreBattingAssessmentRequirementsMet_LessThanRequiredStats_ReturnsFalse()
    {
        // Arrange
        const int plateAppearances = 14;
        var requirements = new PerformanceAssessmentRequirements(statPercentChangeThreshold: 20,
            minimumPlateAppearances: NaturalNumber.Create(15),
            minimumInningsPitched: InningsCount.Create(6),
            minimumBattersFaced: NaturalNumber.Create(25),
            minimumTotalChances: NaturalNumber.Create(30)
        );

        // Act
        var actual = requirements.AreBattingAssessmentRequirementsMet(NaturalNumber.Create(plateAppearances));

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void ArePitchingAssessmentRequirementsMet_EqualToRequiredStats_ReturnsTrue()
    {
        // Arrange
        const decimal inningsPitched = 6m;
        const int battersFaced = 25;
        var requirements = new PerformanceAssessmentRequirements(statPercentChangeThreshold: 20,
            minimumPlateAppearances: NaturalNumber.Create(15),
            minimumInningsPitched: InningsCount.Create(6),
            minimumBattersFaced: NaturalNumber.Create(25),
            minimumTotalChances: NaturalNumber.Create(30)
        );

        // Act
        var actual = requirements.ArePitchingAssessmentRequirementsMet(InningsCount.Create(inningsPitched),
            NaturalNumber.Create(battersFaced));

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void ArePitchingAssessmentRequirementsMet_LessInningsPitchedThanRequiredStats_ReturnsFalse()
    {
        // Arrange
        const decimal inningsPitched = 5m;
        const int battersFaced = 25;
        var requirements = new PerformanceAssessmentRequirements(statPercentChangeThreshold: 20,
            minimumPlateAppearances: NaturalNumber.Create(15),
            minimumInningsPitched: InningsCount.Create(6),
            minimumBattersFaced: NaturalNumber.Create(25),
            minimumTotalChances: NaturalNumber.Create(30)
        );

        // Act
        var actual = requirements.ArePitchingAssessmentRequirementsMet(InningsCount.Create(inningsPitched),
            NaturalNumber.Create(battersFaced));

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void ArePitchingAssessmentRequirementsMet_LessBattersFacedThanRequiredStats_ReturnsFalse()
    {
        // Arrange
        const decimal inningsPitched = 6m;
        const int battersFaced = 24;
        var requirements = new PerformanceAssessmentRequirements(statPercentChangeThreshold: 20,
            minimumPlateAppearances: NaturalNumber.Create(15),
            minimumInningsPitched: InningsCount.Create(6),
            minimumBattersFaced: NaturalNumber.Create(25),
            minimumTotalChances: NaturalNumber.Create(30)
        );

        // Act
        var actual = requirements.ArePitchingAssessmentRequirementsMet(InningsCount.Create(inningsPitched),
            NaturalNumber.Create(battersFaced));

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void AreFieldingAssessmentRequirementsMet_EqualToRequiredStats_ReturnsTrue()
    {
        // Arrange
        const int totalChances = 30;
        var requirements = new PerformanceAssessmentRequirements(statPercentChangeThreshold: 20,
            minimumPlateAppearances: NaturalNumber.Create(15),
            minimumInningsPitched: InningsCount.Create(6),
            minimumBattersFaced: NaturalNumber.Create(25),
            minimumTotalChances: NaturalNumber.Create(30)
        );

        // Act
        var actual = requirements.AreFieldingAssessmentRequirementsMet(NaturalNumber.Create(totalChances));

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void AreFieldingAssessmentRequirementsMet_LessThanRequiredStats_ReturnsFalse()
    {
        // Arrange
        const int totalChances = 29;
        var requirements = new PerformanceAssessmentRequirements(statPercentChangeThreshold: 20,
            minimumPlateAppearances: NaturalNumber.Create(15),
            minimumInningsPitched: InningsCount.Create(6),
            minimumBattersFaced: NaturalNumber.Create(25),
            minimumTotalChances: NaturalNumber.Create(30)
        );

        // Act
        var actual = requirements.AreFieldingAssessmentRequirementsMet(NaturalNumber.Create(totalChances));

        // Assert
        Assert.False(actual);
    }
}