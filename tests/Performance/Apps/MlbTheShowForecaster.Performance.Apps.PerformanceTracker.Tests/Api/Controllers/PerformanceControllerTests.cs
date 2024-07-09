using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Api.Controllers;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Api.Responses;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PerformanceAssessment.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Tests.Api.Controllers;

public class PerformanceControllerTests
{
    [Fact]
    public async Task FindPlayerSeasonPerformance_UnknownPlayer_Returns404()
    {
        // Arrange
        var season = SeasonYear.Create(2024);
        var mlbId = MlbId.Create(1);
        var start = new DateOnly(2024, 7, 1);
        var end = new DateOnly(2024, 7, 8);

        var stubPlayerStatsRepo = new Mock<IPlayerStatsBySeasonRepository>();
        stubPlayerStatsRepo.Setup(x => x.GetBy(season, mlbId))
            .ReturnsAsync((PlayerStatsBySeason?)null);

        var controller = new PerformanceController(stubPlayerStatsRepo.Object, Mock.Of<IPerformanceAssessor>(),
            Mock.Of<IParticipationAssessor>());

        // Act
        var actual = await controller.FindPlayerSeasonPerformance(season.Value, mlbId.Value, start, end);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<NotFoundResult>(actual);
    }

    [Fact]
    public async Task FindPlayerSeasonPerformance_KnownPlayer_ReturnsPlayerSeasonStats()
    {
        // Arrange
        var season = SeasonYear.Create(2024);
        var mlbId = MlbId.Create(1);
        var playerStatsBySeason = Faker.FakePlayerStatsBySeason(seasonYear: season.Value, playerMlbId: mlbId.Value);

        var start = new DateOnly(2024, 7, 1);
        var end = new DateOnly(2024, 7, 8);

        var battingStats = playerStatsBySeason.BattingStatsFor(start, end);
        var pitchingStats = playerStatsBySeason.PitchingStatsFor(start, end);
        var fieldingStats = playerStatsBySeason.FieldingStatsFor(start, end);

        var stubPlayerStatsRepo = new Mock<IPlayerStatsBySeasonRepository>();
        stubPlayerStatsRepo.Setup(x => x.GetBy(season, mlbId))
            .ReturnsAsync(playerStatsBySeason);

        var stubPerformanceAssessor = new Mock<IPerformanceAssessor>();
        stubPerformanceAssessor.Setup(x => x.AssessBatting(battingStats))
            .Returns(Faker.FakePerformanceScore(0.1m));
        stubPerformanceAssessor.Setup(x => x.AssessPitching(pitchingStats))
            .Returns(Faker.FakePerformanceScore(0.2m));
        stubPerformanceAssessor.Setup(x => x.AssessFielding(fieldingStats))
            .Returns(Faker.FakePerformanceScore(0.3m));

        var stubParticipationAssessor = new Mock<IParticipationAssessor>();
        stubParticipationAssessor.Setup(x => x.AssessBatting(start, end, battingStats))
            .Returns(true);
        stubParticipationAssessor.Setup(x => x.AssessPitching(start, end, pitchingStats))
            .Returns(false);
        stubParticipationAssessor.Setup(x => x.AssessFielding(start, end, fieldingStats))
            .Returns(true);

        var controller = new PerformanceController(stubPlayerStatsRepo.Object, stubPerformanceAssessor.Object,
            stubParticipationAssessor.Object);

        // Act
        var actual = await controller.FindPlayerSeasonPerformance(season.Value, mlbId.Value, start, end);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<JsonResult>(actual);
        var responseObject = (PlayerSeasonPerformanceResponse)(actual as JsonResult)!.Value!;
        Assert.Equal(2024, responseObject.Season);
        Assert.Equal(1, responseObject.MlbId);
        Assert.Equal(0.1m, responseObject.BattingScore);
        Assert.True(responseObject.HadSignificantBattingParticipation);
        Assert.Equal(0.2m, responseObject.PitchingScore);
        Assert.False(responseObject.HadSignificantPitchingParticipation);
        Assert.Equal(0.3m, responseObject.FieldingScore);
        Assert.True(responseObject.HadSignificantFieldingParticipation);
    }
}