using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Commands.UpdatePlayerStatsBySeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Events.Participation;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.ValueObjects;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Commands.UpdatePlayerStatsBySeason;

public class UpdatePlayerStatsBySeasonTests
{
    [Fact]
    public async Task Handle_UpdatePlayerStatsBySeasonCommand_UpdatesPlayerStatsBySeason()
    {
        /*
         * Arrange
         */

        // The incoming player season data has one new game for each stat on 4/1
        var fakePlayerSeason = Faker.FakePlayerSeason(
            playerGameBattingStats: new List<PlayerGameBattingStats>()
            {
                Faker.FakePlayerGameBattingStats(gameDate: new DateTime(2024, 3, 31)),
                Faker.FakePlayerGameBattingStats(gameDate: new DateTime(2024, 4, 1))
            },
            playerGamePitchingStats: new List<PlayerGamePitchingStats>()
            {
                Faker.FakePlayerGamePitchingStats(gameDate: new DateTime(2024, 3, 31)),
                Faker.FakePlayerGamePitchingStats(gameDate: new DateTime(2024, 4, 1))
            },
            playerGameFieldingStats: new List<PlayerGameFieldingStats>()
            {
                Faker.FakePlayerGameFieldingStats(gameDate: new DateTime(2024, 3, 31)),
                Faker.FakePlayerGameFieldingStats(gameDate: new DateTime(2024, 4, 1))
            }
        );
        // The entity in the system only has a game on 3/31, so the incoming data has one new game in each stat category on 4/1
        var fakePlayerStatsBySeason = TestClasses.Faker.FakePlayerStatsBySeason(
            battingStatsByGames: new List<PlayerBattingStatsByGame>()
                { TestClasses.Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 3, 31)) },
            pitchingStatsByGames: new List<PlayerPitchingStatsByGame>()
                { TestClasses.Faker.FakePlayerPitchingStats(gameDate: new DateTime(2024, 3, 31)) },
            fieldingStatsByGames: new List<PlayerFieldingStatsByGame>()
                { TestClasses.Faker.FakePlayerFieldingStats(gameDate: new DateTime(2024, 3, 31)) }
        );

        // These will be returned from the stubbed mapper
        var fakeMappedBattingStatsByGame = new List<PlayerBattingStatsByGame>()
        {
            TestClasses.Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 3, 31)),
            TestClasses.Faker.FakePlayerBattingStats(gameDate: new DateTime(2024, 4, 1))
        };
        var fakeMappedPitchingStatsByGame = new List<PlayerPitchingStatsByGame>()
        {
            TestClasses.Faker.FakePlayerPitchingStats(gameDate: new DateTime(2024, 3, 31)),
            TestClasses.Faker.FakePlayerPitchingStats(gameDate: new DateTime(2024, 4, 1))
        };
        var fakeMappedFieldingStatsByGame = new List<PlayerFieldingStatsByGame>()
        {
            TestClasses.Faker.FakePlayerFieldingStats(gameDate: new DateTime(2024, 3, 31)),
            TestClasses.Faker.FakePlayerFieldingStats(gameDate: new DateTime(2024, 4, 1))
        };

        var stubPlayerSeasonMapper = new Mock<IPlayerSeasonMapper>();
        stubPlayerSeasonMapper.Setup(x => x.MapBattingGames(fakePlayerSeason.GameBattingStats))
            .Returns(fakeMappedBattingStatsByGame);
        stubPlayerSeasonMapper.Setup(x => x.MapPitchingGames(fakePlayerSeason.GamePitchingStats))
            .Returns(fakeMappedPitchingStatsByGame);
        stubPlayerSeasonMapper.Setup(x => x.MapFieldingGames(fakePlayerSeason.GameFieldingStats))
            .Returns(fakeMappedFieldingStatsByGame);

        var mockPlayerStatsBySeasonRepository = Mock.Of<IPlayerStatsBySeasonRepository>();
        var mockUnitOfWork = Mock.Of<IUnitOfWork>();

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerStatsBySeasonCommand(fakePlayerStatsBySeason, fakePlayerSeason);
        var handler = new UpdatePlayerStatsBySeasonCommandHandler(stubPlayerSeasonMapper.Object,
            mockPlayerStatsBySeasonRepository, mockUnitOfWork);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Mock.Get(mockPlayerStatsBySeasonRepository).Verify(x => x.Update(fakePlayerStatsBySeason), Times.Once);
        Mock.Get(mockUnitOfWork).Verify(x => x.CommitAsync(cToken), Times.Once);

        Assert.Equal(fakeMappedBattingStatsByGame.OrderBy(x => x.GameDate),
            fakePlayerStatsBySeason.BattingStatsByGamesChronologically);
        Assert.Equal(fakeMappedPitchingStatsByGame.OrderBy(x => x.GameDate),
            fakePlayerStatsBySeason.PitchingStatsByGamesChronologically);
        Assert.Equal(fakeMappedFieldingStatsByGame.OrderBy(x => x.GameDate),
            fakePlayerStatsBySeason.FieldingStatsByGamesChronologically);

        Assert.Equal(3, fakePlayerStatsBySeason.DomainEvents.Count);
        Assert.Equal(1,
            fakePlayerStatsBySeason.DomainEvents.Count(x =>
                x is PlayerBattedInGameEvent gameEvent && gameEvent.Date == new DateTime(2024, 4, 1)));
        Assert.Equal(1,
            fakePlayerStatsBySeason.DomainEvents.Count(x =>
                x is PlayerPitchedInGameEvent gameEvent && gameEvent.Date == new DateTime(2024, 4, 1)));
        Assert.Equal(1,
            fakePlayerStatsBySeason.DomainEvents.Count(x =>
                x is PlayerFieldedInGameEvent gameEvent && gameEvent.Date == new DateTime(2024, 4, 1)));
    }
}