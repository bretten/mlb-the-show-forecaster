using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Api.Responses;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Tests.Api.Responses;

public class PlayerSeasonPerformanceResponseTests
{
    [Fact]
    public void From_PlayerStatsBySeason_ReturnsResponse()
    {
        // Arrange
        var playerStatsBySeason =
            Faker.FakePlayerStatsBySeason(10, 2024, battingScore: 0.1m, pitchingScore: 0.2m, fieldingScore: 0.3m);

        // Act
        var actual = PlayerSeasonPerformanceResponse.From(playerStatsBySeason);

        // Assert
        Assert.Equal(10, actual.MlbId);
        Assert.Equal(2024, actual.Season);
        Assert.Equal(0.1m, actual.BattingScore);
        Assert.Equal(0.2m, actual.PitchingScore);
        Assert.Equal(0.3m, actual.FieldingScore);
    }
}