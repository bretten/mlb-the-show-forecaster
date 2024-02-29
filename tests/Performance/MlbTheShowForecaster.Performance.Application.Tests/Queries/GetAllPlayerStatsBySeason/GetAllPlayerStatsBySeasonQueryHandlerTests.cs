using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Queries.GetAllPlayerStatsBySeason;
using com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.TestClasses;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Entities;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.PlayerSeasons.Repositories;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Application.Tests.Queries.GetAllPlayerStatsBySeason;

public class GetAllPlayerStatsBySeasonQueryHandlerTests
{
    [Fact]
    public async Task Handle_GetAllPlayerStatsBySeasonQuery_ReturnsAllForSeason()
    {
        // Arrange
        var seasonYear = SeasonYear.Create(2024);
        IEnumerable<PlayerStatsBySeason> storedPlayerStatsBySeason = new List<PlayerStatsBySeason>()
        {
            Faker.FakePlayerStatsBySeason(playerMlbId: 1, seasonYear: 2024),
            Faker.FakePlayerStatsBySeason(playerMlbId: 2, seasonYear: 2024),
            Faker.FakePlayerStatsBySeason(playerMlbId: 3, seasonYear: 2024)
        };

        var mockPlayerStatsBySeasonRepository =
            Mock.Of<IPlayerStatsBySeasonRepository>(x =>
                x.GetAll(seasonYear) == Task.FromResult(storedPlayerStatsBySeason));

        var cToken = CancellationToken.None;
        var query = new GetAllPlayerStatsBySeasonQuery(seasonYear);
        var handler = new GetAllPlayerStatsBySeasonQueryHandler(mockPlayerStatsBySeasonRepository);

        // Act
        var actual = await handler.Handle(query, cToken);

        // Assert
        Mock.Get(mockPlayerStatsBySeasonRepository).Verify(x => x.GetAll(seasonYear), Times.Once);
        Assert.NotNull(storedPlayerStatsBySeason);
        Assert.Equal(storedPlayerStatsBySeason, actual!);
    }
}