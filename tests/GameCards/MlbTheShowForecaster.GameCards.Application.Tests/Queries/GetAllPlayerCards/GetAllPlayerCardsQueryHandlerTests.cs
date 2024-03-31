using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetAllPlayerCards;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Queries.GetAllPlayerCards;

public class GetAllPlayerCardsQueryHandlerTests
{
    [Fact]
    public async Task Handle_QueryWithYear_ReturnsPlayerCardsForYear()
    {
        // Arrange
        var year = SeasonYear.Create(2024);
        var playerCard1 = Faker.FakePlayerCard(year: 2024, cardExternalId: "1");
        var playerCard2 = Faker.FakePlayerCard(year: 2023, cardExternalId: "2"); // Year is different, 2023
        var playerCard3 = Faker.FakePlayerCard(year: 2024, cardExternalId: "3");

        var stubPlayerCardRepository = new Mock<IPlayerCardRepository>();
        stubPlayerCardRepository.Setup(x => x.GetAll(year))
            .ReturnsAsync(new List<PlayerCard>() { playerCard1, playerCard3 }); // Only return 2024 player cards

        var cToken = CancellationToken.None;
        var query = new GetAllPlayerCardsQuery(year);
        var handler = new GetAllPlayerCardsQueryHandler(stubPlayerCardRepository.Object);

        // Act
        var actual = await handler.Handle(query, cToken);

        // Assert
        stubPlayerCardRepository.Verify(x => x.GetAll(year), Times.Once);
        Assert.NotNull(actual);
        Assert.Contains(playerCard1, actual);
        Assert.DoesNotContain(playerCard2, actual); // No 2023 player card
        Assert.Contains(playerCard3, actual);
    }
}