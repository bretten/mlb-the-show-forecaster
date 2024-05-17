using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.UpdatePlayerStatsBySeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Commands.UpdatePlayerStatsBySeason;

public class UpdatePlayerStatsBySeasonCommandHandlerTests
{
    [Fact]
    public async Task Handle_UpdatePlayerStatsBySeasonCommand_UpdatesPlayerStatsBySeason()
    {
        // Arrange
        var today = new DateOnly(2024, 4, 1);
        var fakePlayerSeason = Faker.FakePlayerSeason();

        var fakePlayerStatsBySeason = TestClasses.Faker.FakePlayerStatsBySeason();
        var updatedPlayerStatsBySeason = TestClasses.Faker.FakePlayerStatsBySeason();

        var fakeMappedBattingStatsByGame = new List<PlayerBattingStatsByGame>()
            { TestClasses.Faker.FakePlayerBattingStats() };
        var fakeMappedPitchingStatsByGame = new List<PlayerPitchingStatsByGame>()
            { TestClasses.Faker.FakePlayerPitchingStats() };
        var fakeMappedFieldingStatsByGame = new List<PlayerFieldingStatsByGame>()
            { TestClasses.Faker.FakePlayerFieldingStats() };

        var stubPlayerSeasonMapper = new Mock<IPlayerSeasonMapper>();
        stubPlayerSeasonMapper.Setup(x => x.MapBattingGames(fakePlayerSeason.GameBattingStats))
            .Returns(fakeMappedBattingStatsByGame);
        stubPlayerSeasonMapper.Setup(x => x.MapPitchingGames(fakePlayerSeason.GamePitchingStats))
            .Returns(fakeMappedPitchingStatsByGame);
        stubPlayerSeasonMapper.Setup(x => x.MapFieldingGames(fakePlayerSeason.GameFieldingStats))
            .Returns(fakeMappedFieldingStatsByGame);

        var stubPlayerSeasonScorekeeper = new Mock<IPlayerSeasonScorekeeper>();
        stubPlayerSeasonScorekeeper.Setup(x => x.ScoreSeason(fakePlayerStatsBySeason, today,
                fakeMappedBattingStatsByGame, fakeMappedPitchingStatsByGame, fakeMappedFieldingStatsByGame))
            .Returns(updatedPlayerStatsBySeason);

        var stubCalendar = new Mock<ICalendar>();
        stubCalendar.Setup(x => x.Today())
            .Returns(today);

        var stubPlayerStatsBySeasonRepository = new Mock<IPlayerStatsBySeasonRepository>();
        stubPlayerStatsBySeasonRepository.Setup(x => x.GetById(fakePlayerStatsBySeason.Id))
            .ReturnsAsync(fakePlayerStatsBySeason);

        var stubUnitOfWork = new Mock<IUnitOfWork<IPlayerSeasonWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IPlayerStatsBySeasonRepository>())
            .Returns(stubPlayerStatsBySeasonRepository.Object);

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerStatsBySeasonCommand(fakePlayerStatsBySeason, fakePlayerSeason);
        var handler = new UpdatePlayerStatsBySeasonCommandHandler(stubUnitOfWork.Object, stubPlayerSeasonMapper.Object,
            stubPlayerSeasonScorekeeper.Object, stubCalendar.Object);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        stubPlayerStatsBySeasonRepository.Verify(x => x.Update(updatedPlayerStatsBySeason), Times.Once);
        stubUnitOfWork.Verify(x => x.CommitAsync(cToken), Times.Once);
    }
}