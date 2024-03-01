using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.UpdatePlayerStatsBySeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Commands.UpdatePlayerStatsBySeason;

public class UpdatePlayerStatsBySeasonTests
{
    [Fact]
    public async Task Handle_UpdatePlayerStatsBySeasonCommand_UpdatesPlayerStatsBySeason()
    {
        // Act
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
        stubPlayerSeasonScorekeeper.Setup(x => x.ScoreSeason(fakePlayerStatsBySeason, It.IsAny<DateTime>(),
                fakeMappedBattingStatsByGame, fakeMappedPitchingStatsByGame, fakeMappedFieldingStatsByGame))
            .Returns(updatedPlayerStatsBySeason);

        var mockPlayerStatsBySeasonRepository = Mock.Of<IPlayerStatsBySeasonRepository>();
        var mockUnitOfWork = Mock.Of<IUnitOfWork>();

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerStatsBySeasonCommand(fakePlayerStatsBySeason, fakePlayerSeason);
        var handler = new UpdatePlayerStatsBySeasonCommandHandler(stubPlayerSeasonMapper.Object,
            stubPlayerSeasonScorekeeper.Object, mockPlayerStatsBySeasonRepository, mockUnitOfWork);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Mock.Get(mockPlayerStatsBySeasonRepository).Verify(x => x.Update(updatedPlayerStatsBySeason), Times.Once);
        Mock.Get(mockUnitOfWork).Verify(x => x.CommitAsync(cToken), Times.Once);
    }
}