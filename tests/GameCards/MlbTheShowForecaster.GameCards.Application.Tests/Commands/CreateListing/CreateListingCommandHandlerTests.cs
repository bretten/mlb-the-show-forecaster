using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreateListing;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities;
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
        var domainListing = TestClasses.Faker.FakeListing();

        var stubListingMapper = Mock.Of<IListingMapper>(x => x.Map(externalCardListing) == domainListing);

        var mockListingRepository = Mock.Of<IListingRepository>();
        var mockUnitOfWork = Mock.Of<IUnitOfWork<Listing>>();

        var cToken = CancellationToken.None;
        var command = new CreateListingCommand(externalCardListing);
        var handler = new CreateListingCommandHandler(stubListingMapper, mockListingRepository, mockUnitOfWork);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Mock.Get(mockListingRepository).Verify(x => x.Add(domainListing, cToken), Times.Once);
        Mock.Get(mockUnitOfWork).Verify(x => x.CommitAsync(cToken), Times.Once);
    }
}