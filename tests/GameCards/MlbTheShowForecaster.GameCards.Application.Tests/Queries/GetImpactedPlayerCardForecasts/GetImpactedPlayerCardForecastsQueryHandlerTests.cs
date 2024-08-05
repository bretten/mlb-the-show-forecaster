using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetImpactedPlayerCardForecasts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Queries.GetImpactedPlayerCardForecasts;

public class GetImpactedPlayerCardForecastsQueryHandlerTests
{
    [Fact]
    public async Task Handle_QueryWithDate_ReturnsImpactedForecasts()
    {
        // Arrange
        var date = new DateOnly(2024, 8, 5);
        var forecast1 = Faker.FakePlayerCardForecast();
        var forecast2 = Faker.FakePlayerCardForecast();

        var stubForecastRepository = new Mock<IForecastRepository>();
        stubForecastRepository.Setup(x => x.GetImpactedForecasts(date))
            .ReturnsAsync(new List<PlayerCardForecast>() { forecast1, forecast2 });

        var cToken = CancellationToken.None;
        var query = new GetImpactedPlayerCardForecastsQuery(date);
        var handler = new GetImpactedPlayerCardForecastsQueryHandler(stubForecastRepository.Object);

        // Act
        var actual = await handler.Handle(query, cToken);

        // Assert
        stubForecastRepository.Verify(x => x.GetImpactedForecasts(date), Times.Once);
        Assert.Equal(new List<PlayerCardForecast>() { forecast1, forecast2 }, actual!.ToList());
    }
}