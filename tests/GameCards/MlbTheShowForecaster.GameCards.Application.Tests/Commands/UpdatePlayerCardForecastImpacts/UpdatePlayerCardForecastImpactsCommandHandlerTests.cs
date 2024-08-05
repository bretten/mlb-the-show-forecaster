using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCardForecastImpacts.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Commands.UpdatePlayerCardForecastImpacts;

public class UpdatePlayerCardForecastImpactsCommandHandlerTests
{
    [Fact]
    public async Task Handle_CommandHasNoIdentifier_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var command = new UpdatePlayerCardForecastImpactsCommand(SeasonYear.Create(2024), null, null);
        var handler =
            new UpdatePlayerCardForecastImpactsCommandHandler(Mock.Of<IUnitOfWork<IForecastWork>>(),
                Mock.Of<ICalendar>());

        var action = async () => await handler.Handle(command, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<PlayerCardForecastIdentifierNotSpecifiedException>(actual);
    }

    [Fact]
    public async Task Handle_PlayerCardForecastNotFound_ThrowsException()
    {
        // Arrange
        var seasonYear = SeasonYear.Create(2024);
        var cardExternalId = Faker.FakeCardExternalId();

        var stubForecastRepository = new Mock<IForecastRepository>();
        stubForecastRepository.Setup(x => x.GetBy(seasonYear, cardExternalId))
            .ReturnsAsync(null as PlayerCardForecast);

        var stubUnitOfWork = new Mock<IUnitOfWork<IForecastWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IForecastRepository>())
            .Returns(stubForecastRepository.Object);

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerCardForecastImpactsCommand(SeasonYear.Create(2024), cardExternalId, null);
        var handler = new UpdatePlayerCardForecastImpactsCommandHandler(stubUnitOfWork.Object, Mock.Of<ICalendar>());

        var action = async () => await handler.Handle(command, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<PlayerCardForecastNotFoundException>(actual);
    }

    [Fact]
    public async Task Handle_UpdatePlayerCardForecastImpactsCommand_UpdatesImpacts()
    {
        // Arrange
        var forecast = Domain.Tests.Forecasts.TestClasses.Faker.FakePlayerCardForecast();
        var impact = Domain.Tests.Forecasts.TestClasses.Faker.FakeBoostForecastImpact();

        var stubForecastRepository = new Mock<IForecastRepository>();
        stubForecastRepository.Setup(x => x.GetBy(forecast.Year, forecast.CardExternalId))
            .ReturnsAsync(forecast);

        var stubUnitOfWork = new Mock<IUnitOfWork<IForecastWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IForecastRepository>())
            .Returns(stubForecastRepository.Object);

        var stubCalendar = new Mock<ICalendar>();
        stubCalendar.Setup(x => x.Today())
            .Returns(new DateOnly(2024, 8, 5));

        var cToken = CancellationToken.None;
        var command =
            new UpdatePlayerCardForecastImpactsCommand(SeasonYear.Create(2024), forecast.CardExternalId, null, impact);
        var handler = new UpdatePlayerCardForecastImpactsCommandHandler(stubUnitOfWork.Object, Mock.Of<ICalendar>());

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Assert.Single(forecast.ForecastImpactsChronologically);
        Assert.Equal(impact, forecast.ForecastImpactsChronologically[0]);
        stubForecastRepository.Verify(x => x.Update(forecast), Times.Once);
        stubUnitOfWork.Verify(x => x.CommitAsync(cToken), Times.Once);
    }
}