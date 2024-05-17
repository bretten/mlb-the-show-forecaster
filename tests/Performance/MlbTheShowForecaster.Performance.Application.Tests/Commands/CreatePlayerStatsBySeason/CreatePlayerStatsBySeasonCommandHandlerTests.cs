using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.CreatePlayerStatsBySeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Commands.CreatePlayerStatsBySeason;

public class CreatePlayerStatsBySeasonCommandHandlerTests
{
    [Fact]
    public async Task Handle_CreatePlayerStatsBySeasonCommand_CreatesPlayerStatsBySeason()
    {
        // Arrange
        var fakePlayerSeason = Faker.FakePlayerSeason();
        var fakePlayerStatsBySeason = TestClasses.Faker.FakePlayerStatsBySeason();

        var stubPlayerSeasonMapper =
            Mock.Of<IPlayerSeasonMapper>(x => x.Map(fakePlayerSeason) == fakePlayerStatsBySeason);

        var mockPlayerStatsBySeasonRepository = Mock.Of<IPlayerStatsBySeasonRepository>();
        var stubUnitOfWork = new Mock<IUnitOfWork<IPlayerSeasonWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IPlayerStatsBySeasonRepository>())
            .Returns(mockPlayerStatsBySeasonRepository);

        var cToken = CancellationToken.None;
        var command = new CreatePlayerStatsBySeasonCommand(fakePlayerSeason);
        var handler = new CreatePlayerStatsBySeasonCommandHandler(stubUnitOfWork.Object, stubPlayerSeasonMapper);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Mock.Get(mockPlayerStatsBySeasonRepository).Verify(x => x.Add(fakePlayerStatsBySeason), Times.Once);
        stubUnitOfWork.Verify(x => x.CommitAsync(cToken), Times.Once);
    }
}