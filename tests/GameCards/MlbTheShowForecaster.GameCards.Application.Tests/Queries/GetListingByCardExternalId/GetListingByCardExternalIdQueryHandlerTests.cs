﻿using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetListingByCardExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Repositories;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Queries.GetListingByCardExternalId;

public class GetListingByCardExternalIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_QueryWithExternalId_ReturnsListings()
    {
        // Arrange
        var cardExternalId = CardExternalId.Create("1");
        var domainListing = Faker.FakeListing(cardExternalId: cardExternalId.Value);

        var stubListingRepository = new Mock<IListingRepository>();
        stubListingRepository.Setup(x => x.GetByExternalId(cardExternalId))
            .ReturnsAsync(domainListing);

        var cToken = CancellationToken.None;
        var query = new GetListingByCardExternalIdQuery(cardExternalId);
        var handler = new GetListingByCardExternalIdQueryHandler(stubListingRepository.Object);

        // Act
        var actual = await handler.Handle(query, cToken);

        // Assert
        Assert.Equal(domainListing, actual);
    }
}