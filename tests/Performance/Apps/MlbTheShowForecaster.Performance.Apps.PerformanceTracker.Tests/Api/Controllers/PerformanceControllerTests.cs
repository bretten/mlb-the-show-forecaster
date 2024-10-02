using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Api.Controllers;
using com.brettnamba.MlbTheShowForecaster.Performance.Apps.PerformanceTracker.Api.Responses;
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

        var controller = new PerformanceController(stubPlayerStatsRepo.Object, Mock.Of<IPlayerSeasonMapper>());

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
        var performanceMetrics = Faker.FakePlayerSeasonPerformanceMetrics(seasonYear: season.Value, mlbId: mlbId.Value,
            new List<PerformanceMetricsByDate>()
            {
                Faker.FakePerformanceMetricByDate(date: new DateOnly(2024, 10, 1),
                    battingScore: 0.1m, significantBattingParticipation: false,
                    pitchingScore: 0.2m, significantPitchingParticipation: false,
                    fieldingScore: 0.3m, significantFieldingParticipation: false,
                    battingAverage: 1.1m, onBasePercentage: 1.2m, slugging: 1.3m, earnedRunAverage: 1.4m,
                    opponentsBattingAverage: 1.5m, strikeoutsPer9: 1.6m, baseOnBallsPer9: 1.7m, homeRunsPer9: 1.8m,
                    fieldingPercentage: 1.9m),
                Faker.FakePerformanceMetricByDate(date: new DateOnly(2024, 10, 2),
                    battingScore: 0.4m, significantBattingParticipation: true,
                    pitchingScore: 0.5m, significantPitchingParticipation: true,
                    fieldingScore: 0.6m, significantFieldingParticipation: true,
                    battingAverage: 2.1m, onBasePercentage: 2.2m, slugging: 2.3m, earnedRunAverage: 2.4m,
                    opponentsBattingAverage: 2.5m, strikeoutsPer9: 2.6m, baseOnBallsPer9: 2.7m, homeRunsPer9: 2.8m,
                    fieldingPercentage: 2.9m)
            });

        var start = new DateOnly(2024, 10, 1);
        var end = new DateOnly(2024, 10, 2);

        var stubPlayerStatsRepo = new Mock<IPlayerStatsBySeasonRepository>();
        stubPlayerStatsRepo.Setup(x => x.GetBy(season, mlbId))
            .ReturnsAsync(playerStatsBySeason);

        var stubPlayerSeasonMapper = new Mock<IPlayerSeasonMapper>();
        stubPlayerSeasonMapper.Setup(x => x.MapToPlayerSeasonPerformanceMetrics(playerStatsBySeason, start, end))
            .Returns(performanceMetrics);

        var controller = new PerformanceController(stubPlayerStatsRepo.Object, stubPlayerSeasonMapper.Object);

        // Act
        var actual = await controller.FindPlayerSeasonPerformance(season.Value, mlbId.Value, start, end);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<JsonResult>(actual);
        var responseObject = (PlayerSeasonPerformanceResponse)(actual as JsonResult)!.Value!;
        Assert.Equal(2024, responseObject.Season);
        Assert.Equal(1, responseObject.MlbId);
        Assert.Equal(2, responseObject.MetricsByDate.Count);

        Assert.Equal(new DateOnly(2024, 10, 1), responseObject.MetricsByDate[0].Date);
        Assert.Equal(0.1m, responseObject.MetricsByDate[0].BattingScore);
        Assert.False(responseObject.MetricsByDate[0].SignificantBattingParticipation);
        Assert.Equal(0.2m, responseObject.MetricsByDate[0].PitchingScore);
        Assert.False(responseObject.MetricsByDate[0].SignificantPitchingParticipation);
        Assert.Equal(0.3m, responseObject.MetricsByDate[0].FieldingScore);
        Assert.False(responseObject.MetricsByDate[0].SignificantFieldingParticipation);
        Assert.Equal(1.1m, responseObject.MetricsByDate[0].BattingAverage);
        Assert.Equal(1.2m, responseObject.MetricsByDate[0].OnBasePercentage);
        Assert.Equal(1.3m, responseObject.MetricsByDate[0].Slugging);
        Assert.Equal(1.4m, responseObject.MetricsByDate[0].EarnedRunAverage);
        Assert.Equal(1.5m, responseObject.MetricsByDate[0].OpponentsBattingAverage);
        Assert.Equal(1.6m, responseObject.MetricsByDate[0].StrikeoutsPer9);
        Assert.Equal(1.7m, responseObject.MetricsByDate[0].BaseOnBallsPer9);
        Assert.Equal(1.8m, responseObject.MetricsByDate[0].HomeRunsPer9);
        Assert.Equal(1.9m, responseObject.MetricsByDate[0].FieldingPercentage);

        Assert.Equal(new DateOnly(2024, 10, 2), responseObject.MetricsByDate[1].Date);
        Assert.Equal(0.4m, responseObject.MetricsByDate[1].BattingScore);
        Assert.True(responseObject.MetricsByDate[1].SignificantBattingParticipation);
        Assert.Equal(0.5m, responseObject.MetricsByDate[1].PitchingScore);
        Assert.True(responseObject.MetricsByDate[1].SignificantPitchingParticipation);
        Assert.Equal(0.6m, responseObject.MetricsByDate[1].FieldingScore);
        Assert.True(responseObject.MetricsByDate[1].SignificantFieldingParticipation);
        Assert.Equal(2.1m, responseObject.MetricsByDate[1].BattingAverage);
        Assert.Equal(2.2m, responseObject.MetricsByDate[1].OnBasePercentage);
        Assert.Equal(2.3m, responseObject.MetricsByDate[1].Slugging);
        Assert.Equal(2.4m, responseObject.MetricsByDate[1].EarnedRunAverage);
        Assert.Equal(2.5m, responseObject.MetricsByDate[1].OpponentsBattingAverage);
        Assert.Equal(2.6m, responseObject.MetricsByDate[1].StrikeoutsPer9);
        Assert.Equal(2.7m, responseObject.MetricsByDate[1].BaseOnBallsPer9);
        Assert.Equal(2.8m, responseObject.MetricsByDate[1].HomeRunsPer9);
        Assert.Equal(2.9m, responseObject.MetricsByDate[1].FieldingPercentage);
    }
}