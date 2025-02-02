using com.brettnamba.MlbTheShowForecaster.Common.Application.Pagination;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Api.Controllers;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Dtos.TestClasses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Faker = com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses.Faker;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.Tests.Api.Controllers;

public class TrendControllerTests
{
    [Fact]
    public async Task Index_YearAndPage_ReturnsResults()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var year = SeasonYear.Create(2024);
        const int page = 1;
        const int pageSize = 10;

        var trendReports = new List<TrendReport>()
        {
            Faker.FakeTrendReport(externalId: Faker.FakeGuid1),
            Faker.FakeTrendReport(externalId: Faker.FakeGuid2, mlbId: 100, overallRating: 99,
                metricsByDate: new List<TrendMetricsByDate>()
                {
                    Faker.FakeTrendMetricsByDate(new DateOnly(2024, 10, 8)),
                    Faker.FakeTrendMetricsByDate(new DateOnly(2024, 10, 9)),
                    Faker.FakeTrendMetricsByDate(new DateOnly(2024, 10, 10)),
                    Faker.FakeTrendMetricsByDate(new DateOnly(2024, 10, 11)),
                }, impacts: new List<TrendImpact>()
                {
                    Faker.FakeTrendImpact(new DateOnly(2024, 10, 8), new DateOnly(2024, 10, 9)),
                    Faker.FakeTrendImpact(new DateOnly(2024, 10, 9), new DateOnly(2024, 10, 10)),
                    Faker.FakeTrendImpact(new DateOnly(2024, 10, 10), new DateOnly(2024, 10, 11)),
                }),
        };
        var paginationResult = Faker.FakeTrendReportPaginationResult(page, pageSize, trendReports, 2);

        var stubTrendReporter = new Mock<ITrendReporter>();
        stubTrendReporter.Setup(x => x.GetTrendReports(year, page, pageSize, null, null, null, cToken))
            .ReturnsAsync(paginationResult);

        var controller = new TrendController(stubTrendReporter.Object);

        // Act
        var actual = await controller.Index(year.Value, page, pageSize, cancellationToken: cToken);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<JsonResult>(actual);
        var responseObject = (PaginationResult<TrendReport>)(actual as JsonResult)!.Value!;
        var items = responseObject.Items.ToList();
        Assert.Equal(2, items.Count);
        Asserter.Equal(trendReports[0], items[0]);
        Asserter.Equal(trendReports[1], items[1]);
    }
}