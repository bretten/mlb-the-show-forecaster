using System.Text;
using com.brettnamba.MlbTheShowForecaster.Common.Application.FileSystems;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Reports;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.TestClasses;
using Microsoft.Extensions.Logging;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Services.Reports;

public class ForecastReportPublisherTests
{
    [Fact]
    public void Constructor_NoOutputPath_ThrowsException()
    {
        // Arrange
        const string outputPath = "";

        var action = () => new ForecastReportPublisher(Mock.Of<IForecastRepository>(),
            Mock.Of<IForecastReportGenerator>(), Mock.Of<IFileSystem>(), outputPath,
            Mock.Of<ILogger<ForecastReportPublisher>>());

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<ArgumentException>(actual);
    }

    [Fact]
    public async Task Publish_SeasonAndDate_PublishesReport()
    {
        // Arrange
        var year = SeasonYear.Create(2024);
        var date = new DateOnly(2024, 8, 26);
        const string outputPath = "path/to/file";

        var forecasts = new List<PlayerCardForecast>
        {
            Faker.FakePlayerCardForecast(),
            Faker.FakePlayerCardForecast()
        };

        var forecastReport = new ForecastReport(year, forecasts, "html");

        var stubForecastRepository = new Mock<IForecastRepository>();
        stubForecastRepository.Setup(x => x.GetImpactedForecasts(date))
            .ReturnsAsync(forecasts);

        var stubForecastReportGenerator = new Mock<IForecastReportGenerator>();
        stubForecastReportGenerator.Setup(x => x.Generate(year, forecasts, date))
            .ReturnsAsync(forecastReport);

        var mockFileSystem = new Mock<IFileSystem>();

        var publisher = new ForecastReportPublisher(stubForecastRepository.Object, stubForecastReportGenerator.Object,
            mockFileSystem.Object, outputPath, Mock.Of<ILogger<ForecastReportPublisher>>());

        // Act
        await publisher.Publish(year, date);

        // Assert
        mockFileSystem.Verify(x =>
            x.StoreFile(It.Is<MemoryStream>(y => Encoding.UTF8.GetString(y.ToArray()) == "html"), outputPath, true));
    }
}