using com.brettnamba.MlbTheShowForecaster.Common.Application.Cqrs;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.CreatePlayerStatsBySeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Events.NewPlayerSeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Dtos.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Events.NewPlayerSeason;

public class NewPlayerSeasonDomainEventConsumerTests
{
    [Fact]
    public async Task Handle_NewPlayerSeasonEvent_SendsCommandToCreateNewPlayerSeason()
    {
        // Arrange
        var playerMlbId = MlbId.Create(10);
        var season = SeasonYear.Create(2024);

        var playerSeasonStats = Faker.FakePlayerSeason();

        var stubPlayerStats = new Mock<IPlayerStats>();
        stubPlayerStats.Setup(x => x.GetPlayerSeason(playerMlbId, season))
            .ReturnsAsync(playerSeasonStats);

        var stubCalendar = Mock.Of<ICalendar>(x => x.Today() == new DateOnly(season.Value, 1, 1));

        var mockCommandSender = Mock.Of<ICommandSender>();

        var cToken = CancellationToken.None;
        var consumer = new NewPlayerSeasonDomainEventConsumer(stubPlayerStats.Object, mockCommandSender, stubCalendar);
        var e = new NewPlayerSeasonEvent(playerMlbId);

        // Act
        await consumer.Handle(e, cToken);

        // Assert
        var expectedCommand = new CreatePlayerStatsBySeasonCommand(playerSeasonStats);
        Mock.Get(mockCommandSender).Verify(x => x.Send(expectedCommand, cToken), Times.Once);
    }
}