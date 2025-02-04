using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastMlbId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastMlbId.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Commands.UpdatePlayerCardForecastMlbId;

public class UpdatePlayerCardForecastMlbIdCommandHandlerTests
{
    [Fact]
    public async Task Handle_NoMatchingForecast_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var playerCard = Faker.FakePlayerCard();

        var stubForecastRepository = new Mock<IForecastRepository>();
        stubForecastRepository.Setup(x => x.GetBy(playerCard.Year, playerCard.ExternalId))
            .ReturnsAsync((PlayerCardForecast?)null);

        var stubUnitOfWork = new Mock<IUnitOfWork<IForecastWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IForecastRepository>())
            .Returns(stubForecastRepository.Object);

        var command = new UpdatePlayerCardForecastMlbIdCommand(playerCard);
        var handler = new UpdatePlayerCardForecastMlbIdCommandHandler(stubUnitOfWork.Object, Mock.Of<IPlayerMatcher>());

        var action = async () => await handler.Handle(command, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<MissingForecastForMlbIdUpdateException>(actual);
    }

    [Fact]
    public async Task Handle_NoMatchingPlayer_NoActionTaken()
    {
        // Arrange
        var playerCard = Faker.FakePlayerCard();
        var forecast = Domain.Tests.Forecasts.TestClasses.Faker.FakePlayerCardForecast(mlbId: null);

        var stubForecastRepository = new Mock<IForecastRepository>();
        stubForecastRepository.Setup(x => x.GetBy(playerCard.Year, playerCard.ExternalId))
            .ReturnsAsync(forecast);

        var stubUnitOfWork = new Mock<IUnitOfWork<IForecastWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IForecastRepository>())
            .Returns(stubForecastRepository.Object);

        var stubPlayerMatcher = new Mock<IPlayerMatcher>();
        stubPlayerMatcher.Setup(x => x.GetPlayerByName(playerCard.Name, playerCard.TeamShortName))
            .ReturnsAsync((Player?)null);

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerCardForecastMlbIdCommand(playerCard);
        var handler = new UpdatePlayerCardForecastMlbIdCommandHandler(stubUnitOfWork.Object, stubPlayerMatcher.Object);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Assert.Null(forecast.MlbId);
        stubForecastRepository.Verify(x => x.Update(forecast), Times.Never);
        stubUnitOfWork.Verify(x => x.CommitAsync(cToken), Times.Never);
    }

    [Fact]
    public async Task Handle_UpdatePlayerCardForecastMlbIdCommand_UpdatesForecastMlbId()
    {
        // Arrange
        var playerCard = Faker.FakePlayerCard();
        var forecast = Domain.Tests.Forecasts.TestClasses.Faker.FakePlayerCardForecast(mlbId: null);
        var player = Dtos.TestClasses.Faker.FakePlayer(mlbId: 123);

        var stubForecastRepository = new Mock<IForecastRepository>();
        stubForecastRepository.Setup(x => x.GetBy(playerCard.Year, playerCard.ExternalId))
            .ReturnsAsync(forecast);

        var stubUnitOfWork = new Mock<IUnitOfWork<IForecastWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IForecastRepository>())
            .Returns(stubForecastRepository.Object);

        var stubPlayerMatcher = new Mock<IPlayerMatcher>();
        stubPlayerMatcher.Setup(x => x.GetPlayerByName(playerCard.Name, playerCard.TeamShortName))
            .ReturnsAsync(player);

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerCardForecastMlbIdCommand(playerCard);
        var handler = new UpdatePlayerCardForecastMlbIdCommandHandler(stubUnitOfWork.Object, stubPlayerMatcher.Object);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Assert.NotNull(forecast.MlbId);
        Assert.Equal(123, forecast.MlbId.Value);
        stubForecastRepository.Verify(x => x.Update(forecast), Times.Once);
        stubUnitOfWork.Verify(x => x.CommitAsync(cToken), Times.Once);
    }
}