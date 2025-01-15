using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Listings;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Listings;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Dtos.TestClasses;
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

        var cardExternalId = Faker.FakeCardExternalId(Faker.FakeGuid1);

        var stubMlbTheShowApi = new Mock<IMlbTheShowApi>();
        stubMlbTheShowApi.Setup(x => x.GetListing(new GetListingRequest(cardExternalId.AsStringDigits)))
            .ReturnsAsync((ListingDto<ItemDto>)null!);

        var stubMlbTheShowApiFactory = new Mock<IMlbTheShowApiFactory>();
        stubMlbTheShowApiFactory.Setup(x => x.GetClient(Year.Season2024))
            .Returns(stubMlbTheShowApi.Object);

        var mockListingMapper = Mock.Of<IMlbTheShowListingMapper>();

        var marketplace = new MlbTheShowApiCardMarketplace(stubMlbTheShowApiFactory.Object, mockListingMapper);

        var action = () => marketplace.GetCardPrice(seasonYear, cardExternalId, cToken);

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

        var cardExternalId = Faker.FakeCardExternalId(Faker.FakeGuid1);
        var listingDto = Dtos.Mapping.TestClasses.Faker.FakeListingDto("listing1");
        var expectedCardListing = Faker.FakeCardListing(cardExternalId: cardExternalId.Value);

        var stubMlbTheShowApi = new Mock<IMlbTheShowApi>();
        stubMlbTheShowApi.Setup(x => x.GetListing(new GetListingRequest(cardExternalId.AsStringDigits)))
            .ReturnsAsync(listingDto);

        var stubMlbTheShowApiFactory = new Mock<IMlbTheShowApiFactory>();
        stubMlbTheShowApiFactory.Setup(x => x.GetClient(Year.Season2024))
            .Returns(stubMlbTheShowApi.Object);

        var stubListingMapper = new Mock<IMlbTheShowListingMapper>();
        stubListingMapper.Setup(x => x.Map(listingDto))
            .Returns(expectedCardListing);

        var marketplace = new MlbTheShowApiCardMarketplace(stubMlbTheShowApiFactory.Object, stubListingMapper.Object);

        // Act
        var actual = await marketplace.GetCardPrice(seasonYear, cardExternalId, cToken);

        // Assert
        Assert.Equal(expectedCardListing, actual);
    }
}