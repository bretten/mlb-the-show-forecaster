using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Api.Controllers;
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

        var stubPlayerStatsRepo = new Mock<IPlayerStatsBySeasonRepository>();
        stubPlayerStatsRepo.Setup(x => x.GetBy(season, mlbId))
            .ReturnsAsync((PlayerStatsBySeason?)null);

        var controller = new PerformanceController(stubPlayerStatsRepo.Object);

        // Act
        var actual = await controller.FindPlayerSeasonPerformance(season.Value, mlbId.Value);

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

        var stubPlayerStatsRepo = new Mock<IPlayerStatsBySeasonRepository>();
        stubPlayerStatsRepo.Setup(x => x.GetBy(season, mlbId))
            .ReturnsAsync(playerStatsBySeason);

        var controller = new PerformanceController(stubPlayerStatsRepo.Object);

        // Act
        var actual = await controller.FindPlayerSeasonPerformance(season.Value, mlbId.Value);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<JsonResult>(actual);
    }
}