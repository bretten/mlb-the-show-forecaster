﻿using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetPlayerCardByExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Queries.GetPlayerCardByExternalId;

public class GetPlayerCardByExternalIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ExternalIdQuery_ReturnsPlayerCard()
    {
        // Arrange
        var cardExternalId = Faker.FakeCardExternalId();
        var fakeDomainPlayerCard = Faker.FakePlayerCard(cardExternalId: cardExternalId.Value);

        var stubPlayerCardRepository = Mock.Of<IPlayerCardRepository>(x =>
            x.GetByExternalId(cardExternalId) == Task.FromResult(fakeDomainPlayerCard));

        var cToken = CancellationToken.None;
        var query = new GetPlayerCardByExternalIdQuery(cardExternalId);
        var handler = new GetPlayerCardByExternalIdQueryHandler(stubPlayerCardRepository);

        // Act
        var actual = await handler.Handle(query, cToken);

        // Assert
        Mock.Get(stubPlayerCardRepository).Verify(x => x.GetByExternalId(cardExternalId), Times.Once);
        Assert.NotNull(actual);
        Assert.Equal(fakeDomainPlayerCard, actual);
    }
}