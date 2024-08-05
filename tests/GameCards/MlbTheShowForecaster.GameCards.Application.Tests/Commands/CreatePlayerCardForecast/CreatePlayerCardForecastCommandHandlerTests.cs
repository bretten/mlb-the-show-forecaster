using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCardForecast;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Repositories;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Commands.CreatePlayerCardForecast;

public class CreatePlayerCardForecastCommandHandlerTests
{
    [Fact]
    public async Task Handle_CreatePlayerCardForecastCommand_CreatesPlayerCardForecast()
    {
        // Arrange
        var season = SeasonYear.Create(2024);
        var cardExternalId = Faker.FakeCardExternalId();
        var mlbId = MlbId.Create(1);
        const Position position = Position.CenterField;
        var overallRating = Faker.FakeOverallRating(85);

        var mockForecastRepository = Mock.Of<IForecastRepository>();
        var stubUnitOfWork = new Mock<IUnitOfWork<IForecastWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IForecastRepository>())
            .Returns(mockForecastRepository);

        var cToken = CancellationToken.None;
        var command = new CreatePlayerCardForecastCommand(season, cardExternalId, position, overallRating, mlbId);
        var handler = new CreatePlayerCardForecastCommandHandler(stubUnitOfWork.Object);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Mock.Get(mockForecastRepository).Verify(x =>
            x.Add(It.Is<PlayerCardForecast>(f =>
                f.Year == season && f.CardExternalId == cardExternalId && f.MlbId == mlbId
                && f.PrimaryPosition == position && f.OverallRating == overallRating)
            ), Times.Once);
        stubUnitOfWork.Verify(x => x.CommitAsync(cToken), Times.Once);
    }
}