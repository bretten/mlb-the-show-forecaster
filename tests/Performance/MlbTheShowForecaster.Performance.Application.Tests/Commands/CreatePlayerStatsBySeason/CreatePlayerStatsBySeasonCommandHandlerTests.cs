using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.CreatePlayerStatsBySeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Commands.CreatePlayerStatsBySeason;

public class CreatePlayerStatsBySeasonCommandHandlerTests
{
    [Fact]
    public async Task Handle_CreatePlayerStatsBySeasonCommand_CreatesPlayerStatsBySeason()
    {
        // Arrange
        var fakePlayerSeason = Faker.FakePlayerSeason();
        var fakePlayerStatsBySeason = Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerStatsBySeason();
        var scoredPlayerStatsBySeason = Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerStatsBySeason();

        var fakeMappedBattingStatsByGame = new List<PlayerBattingStatsByGame>()
            { Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerBattingStats() };
        var fakeMappedPitchingStatsByGame = new List<PlayerPitchingStatsByGame>()
            { Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerPitchingStats() };
        var fakeMappedFieldingStatsByGame = new List<PlayerFieldingStatsByGame>()
            { Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerFieldingStats() };

        var stubPlayerSeasonMapper = new Mock<IPlayerSeasonMapper>();
        stubPlayerSeasonMapper.Setup(x => x.Map(fakePlayerSeason))
            .Returns(fakePlayerStatsBySeason);
        stubPlayerSeasonMapper.Setup(x => x.MapBattingGames(fakePlayerSeason.GameBattingStats))
            .Returns(fakeMappedBattingStatsByGame);
        stubPlayerSeasonMapper.Setup(x => x.MapPitchingGames(fakePlayerSeason.GamePitchingStats))
            .Returns(fakeMappedPitchingStatsByGame);
        stubPlayerSeasonMapper.Setup(x => x.MapFieldingGames(fakePlayerSeason.GameFieldingStats))
            .Returns(fakeMappedFieldingStatsByGame);

        var stubPlayerSeasonScorekeeper = new Mock<IPlayerSeasonScorekeeper>();
        stubPlayerSeasonScorekeeper.Setup(x => x.ScoreSeason(fakePlayerStatsBySeason,
                fakeMappedBattingStatsByGame, fakeMappedPitchingStatsByGame, fakeMappedFieldingStatsByGame))
            .Returns(scoredPlayerStatsBySeason);

        var mockPlayerStatsBySeasonRepository = Mock.Of<IPlayerStatsBySeasonRepository>();
        var stubUnitOfWork = new Mock<IUnitOfWork<IPlayerSeasonWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IPlayerStatsBySeasonRepository>())
            .Returns(mockPlayerStatsBySeasonRepository);

        var cToken = CancellationToken.None;
        var command = new CreatePlayerStatsBySeasonCommand(fakePlayerSeason);
        var handler = new CreatePlayerStatsBySeasonCommandHandler(stubUnitOfWork.Object, stubPlayerSeasonMapper.Object,
            stubPlayerSeasonScorekeeper.Object);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Mock.Get(mockPlayerStatsBySeasonRepository).Verify(x => x.Add(scoredPlayerStatsBySeason), Times.Once);
        stubUnitOfWork.Verify(x => x.CommitAsync(cToken), Times.Once);
    }
}