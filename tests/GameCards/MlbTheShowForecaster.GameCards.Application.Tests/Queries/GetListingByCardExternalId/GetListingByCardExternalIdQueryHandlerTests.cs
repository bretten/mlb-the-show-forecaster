﻿using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Queries.GetListingByCardExternalId;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Repositories;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Queries.GetListingByCardExternalId;

public class GetListingByCardExternalIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_QueryWithExternalId_ReturnsListings()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var cardExternalId = Faker.FakeCardExternalId();
        var domainListing = Faker.FakeListing(cardExternalId: cardExternalId.Value);

        var stubListingRepository = new Mock<IListingRepository>();
        stubListingRepository.Setup(x => x.GetByExternalId(cardExternalId, cToken))
            .ReturnsAsync(domainListing);

        var stubUnitOfWork = new Mock<IUnitOfWork<IMarketplaceWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IListingRepository>())
            .Returns(stubListingRepository.Object);

        var query = new GetListingByCardExternalIdQuery(cardExternalId);
        var handler = new GetListingByCardExternalIdQueryHandler(stubUnitOfWork.Object);

        // Act
        var actual = await handler.Handle(query, cToken);

        // Assert
        Assert.Equal(domainListing, actual);
    }
}