using com.brettnamba.MlbTheShowForecaster.Common.Domain.Events;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreateListing;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Repositories;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Commands.CreateListing;

public class CreateListingCommandHandlerTests
{
    [Fact]
    public async Task Handle_CreateListingCommand_CreatesListing()
    {
        // Arrange
        var externalCardListing = Faker.FakeCardListing();
        var domainListing = Domain.Tests.Marketplace.TestClasses.Faker.FakeListing();

        var stubListingMapper = Mock.Of<IListingMapper>(x => x.Map(externalCardListing) == domainListing);
        var mockDomainEventDispatcher = Mock.Of<IDomainEventDispatcher>();

        var mockListingRepository = Mock.Of<IListingRepository>();
        var stubUnitOfWork = new Mock<IUnitOfWork<IMarketplaceWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IListingRepository>())
            .Returns(mockListingRepository);

        var cToken = CancellationToken.None;
        var command = new CreateListingCommand(externalCardListing);
        var handler =
            new CreateListingCommandHandler(stubUnitOfWork.Object, stubListingMapper, mockDomainEventDispatcher);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Mock.Get(mockListingRepository).Verify(x => x.Add(domainListing, cToken), Times.Once);
        stubUnitOfWork.Verify(x => x.CommitAsync(cToken), Times.Once);
        Mock.Get(mockDomainEventDispatcher).Verify(x => x.Dispatch(domainListing.DomainEvents));
    }
}