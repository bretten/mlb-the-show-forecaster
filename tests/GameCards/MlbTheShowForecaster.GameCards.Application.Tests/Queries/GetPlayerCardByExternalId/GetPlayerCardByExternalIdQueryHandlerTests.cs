using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetPlayerCardByExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Queries.GetPlayerCardByExternalId;

public class GetPlayerCardByExternalIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ExternalIdQuery_ReturnsPlayerCard()
    {
        // Arrange
        var cardExternalId = Faker.FakeCardExternalId();
        var fakeDomainPlayerCard = Faker.FakePlayerCard(externalId: cardExternalId.Value);

        var stubPlayerCardRepository = Mock.Of<IPlayerCardRepository>(x =>
            x.GetByExternalId(fakeDomainPlayerCard.Year, cardExternalId) == Task.FromResult(fakeDomainPlayerCard));

        var stubUnitOfWork = new Mock<IUnitOfWork<ICardWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IPlayerCardRepository>())
            .Returns(stubPlayerCardRepository);

        var cToken = CancellationToken.None;
        var query = new GetPlayerCardByExternalIdQuery(fakeDomainPlayerCard.Year, cardExternalId);
        var handler = new GetPlayerCardByExternalIdQueryHandler(stubUnitOfWork.Object);

        // Act
        var actual = await handler.Handle(query, cToken);

        // Assert
        Mock.Get(stubPlayerCardRepository)
            .Verify(x => x.GetByExternalId(fakeDomainPlayerCard.Year, cardExternalId), Times.Once);
        Assert.NotNull(actual);
        Assert.Equal(fakeDomainPlayerCard, actual);
    }
}