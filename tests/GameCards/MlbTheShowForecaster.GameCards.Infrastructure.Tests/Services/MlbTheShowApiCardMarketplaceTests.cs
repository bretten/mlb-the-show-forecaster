using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Listings;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Listings;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Dtos.Mapping.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Services;

public class MlbTheShowApiCardMarketplaceTests
{
    [Fact]
    public async Task GetCardPrice_CardExternalIdWithNoMatch_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);

        var cardExternalId1 = CardExternalId.Create("id1");

        var stubMlbTheShowApi = new Mock<IMlbTheShowApi>();
        stubMlbTheShowApi.Setup(x => x.GetListing(new GetListingRequest(cardExternalId1.Value)))
            .ReturnsAsync((ListingDto<ItemDto>)null!);

        var stubMlbTheShowApiFactory = new Mock<IMlbTheShowApiFactory>();
        stubMlbTheShowApiFactory.Setup(x => x.GetClient(Year.Season2024))
            .Returns(stubMlbTheShowApi.Object);

        var mockListingMapper = Mock.Of<IMlbTheShowListingMapper>();

        var marketplace = new MlbTheShowApiCardMarketplace(stubMlbTheShowApiFactory.Object, mockListingMapper);

        var action = () => marketplace.GetCardPrice(seasonYear, cardExternalId1, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<CardListingNotFoundInMarketplaceException>(actual);
    }

    [Fact]
    public async Task GetCardPrice_SeasonYearAndCardExternalId_ReturnsCardPrice()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);

        var cardExternalId = CardExternalId.Create("id1");
        var listingDto = Faker.FakeListingDto("listing1");
        var expectedCardListing = Dtos.TestClasses.Faker.FakeCardListing(cardExternalId: cardExternalId.Value);

        var stubMlbTheShowApi = new Mock<IMlbTheShowApi>();
        stubMlbTheShowApi.Setup(x => x.GetListing(new GetListingRequest(cardExternalId.Value)))
            .ReturnsAsync(listingDto);

        var stubMlbTheShowApiFactory = new Mock<IMlbTheShowApiFactory>();
        stubMlbTheShowApiFactory.Setup(x => x.GetClient(Year.Season2024))
            .Returns(stubMlbTheShowApi.Object);

        var stubListingMapper = new Mock<IMlbTheShowListingMapper>();
        stubListingMapper.Setup(x => x.Map(seasonYear, listingDto))
            .Returns(expectedCardListing);

        var marketplace = new MlbTheShowApiCardMarketplace(stubMlbTheShowApiFactory.Object, stubListingMapper.Object);

        // Act
        var actual = await marketplace.GetCardPrice(seasonYear, cardExternalId, cToken);

        // Assert
        Assert.Equal(expectedCardListing, actual);
    }
}