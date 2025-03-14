﻿using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.UpdatePlayerStatsBySeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.UpdatePlayerStatsBySeason.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Services;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Commands.UpdatePlayerStatsBySeason;

public class UpdatePlayerStatsBySeasonCommandHandlerTests
{
    [Fact]
    public async Task Handle_MissingPlayerStatsBySeason_ThrowsException()
    {
        // Arrange
        var fakePlayerSeason = Faker.FakePlayerSeason();
        var fakePlayerStatsBySeason = Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerStatsBySeason();

        var stubPlayerStatsBySeasonRepository = new Mock<IPlayerStatsBySeasonRepository>();
        stubPlayerStatsBySeasonRepository.Setup(x => x.GetById(fakePlayerStatsBySeason.Id))
            .ReturnsAsync(null as PlayerStatsBySeason);

        var stubUnitOfWork = new Mock<IUnitOfWork<IPlayerSeasonWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IPlayerStatsBySeasonRepository>())
            .Returns(stubPlayerStatsBySeasonRepository.Object);

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerStatsBySeasonCommand(fakePlayerStatsBySeason, fakePlayerSeason);
        var handler = new UpdatePlayerStatsBySeasonCommandHandler(stubUnitOfWork.Object, Mock.Of<IPlayerSeasonMapper>(),
            Mock.Of<IPlayerSeasonScorekeeper>());

        var action = () => handler.Handle(command, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<PlayerStatsBySeasonNotFoundException>(actual);
    }

    [Fact]
    public async Task Handle_UpdatePlayerStatsBySeasonCommand_UpdatesPlayerStatsBySeason()
    {
        // Arrange
        var fakePlayerSeason = Faker.FakePlayerSeason();

        var fakePlayerStatsBySeason = Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerStatsBySeason();
        var updatedPlayerStatsBySeason = Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerStatsBySeason();

        var fakeMappedBattingStatsByGame = new List<PlayerBattingStatsByGame>()
            { Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerBattingStats() };
        var fakeMappedPitchingStatsByGame = new List<PlayerPitchingStatsByGame>()
            { Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerPitchingStats() };
        var fakeMappedFieldingStatsByGame = new List<PlayerFieldingStatsByGame>()
            { Domain.Tests.PlayerSeasons.TestClasses.Faker.FakePlayerFieldingStats() };

        var stubPlayerSeasonMapper = new Mock<IPlayerSeasonMapper>();
        stubPlayerSeasonMapper.Setup(x => x.MapBattingGames(fakePlayerSeason.GameBattingStats))
            .Returns(fakeMappedBattingStatsByGame);
        stubPlayerSeasonMapper.Setup(x => x.MapPitchingGames(fakePlayerSeason.GamePitchingStats))
            .Returns(fakeMappedPitchingStatsByGame);
        stubPlayerSeasonMapper.Setup(x => x.MapFieldingGames(fakePlayerSeason.GameFieldingStats))
            .Returns(fakeMappedFieldingStatsByGame);

        var stubPlayerSeasonScorekeeper = new Mock<IPlayerSeasonScorekeeper>();
        stubPlayerSeasonScorekeeper.Setup(x => x.ScoreSeason(fakePlayerStatsBySeason,
                fakeMappedBattingStatsByGame, fakeMappedPitchingStatsByGame, fakeMappedFieldingStatsByGame))
            .Returns(updatedPlayerStatsBySeason);

        var stubPlayerStatsBySeasonRepository = new Mock<IPlayerStatsBySeasonRepository>();
        stubPlayerStatsBySeasonRepository.Setup(x => x.GetById(fakePlayerStatsBySeason.Id))
            .ReturnsAsync(fakePlayerStatsBySeason);

        var stubUnitOfWork = new Mock<IUnitOfWork<IPlayerSeasonWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IPlayerStatsBySeasonRepository>())
            .Returns(stubPlayerStatsBySeasonRepository.Object);

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerStatsBySeasonCommand(fakePlayerStatsBySeason, fakePlayerSeason);
        var handler = new UpdatePlayerStatsBySeasonCommandHandler(stubUnitOfWork.Object, stubPlayerSeasonMapper.Object,
            stubPlayerSeasonScorekeeper.Object);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        stubPlayerStatsBySeasonRepository.Verify(x => x.Update(updatedPlayerStatsBySeason), Times.Once);
        stubUnitOfWork.Verify(x => x.CommitAsync(cToken), Times.Once);
    }
}