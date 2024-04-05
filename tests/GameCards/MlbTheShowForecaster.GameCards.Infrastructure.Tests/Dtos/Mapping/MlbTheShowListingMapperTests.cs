using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Listings;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Dtos.Mapping.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Dtos.Mapping;

public class MlbTheShowListingMapperTests
{
    [Fact]
    public void Map_ListingDto_ReturnsCardListing()
    {
        // Arrange
        var seasonYear = SeasonYear.Create(2024);
        var listing = Faker.FakeListingDto(listingName: "name1",
            bestBuyPrice: 20,
            bestSellPrice: 10,
            itemDto: Faker.FakeMlbCardDto(uuid: "externalId1"),
            priceHistory: new List<ListingPriceDto>()
            {
                new ListingPriceDto("04/01", BestBuyPrice: 2, BestSellPrice: 1),
                new ListingPriceDto("04/02", BestBuyPrice: 200, BestSellPrice: 100)
            });
        var mapper = new MlbTheShowListingMapper();

        // Act
        var actual = mapper.Map(seasonYear, listing);

        // Assert
        Assert.Equal("name1", actual.ListingName);
        Assert.Equal(20, actual.BestBuyPrice.Value);
        Assert.Equal(10, actual.BestSellPrice.Value);
        Assert.Equal("externalId1", actual.CardExternalId.Value);
        Assert.Equal(2, actual.HistoricalPrices.Count);
        Assert.Equal(new DateOnly(2024, 4, 1), actual.HistoricalPrices[0].Date);
        Assert.Equal(2, actual.HistoricalPrices[0].BestBuyPrice.Value);
        Assert.Equal(1, actual.HistoricalPrices[0].BestSellPrice.Value);
        Assert.Equal(new DateOnly(2024, 4, 2), actual.HistoricalPrices[1].Date);
        Assert.Equal(200, actual.HistoricalPrices[1].BestBuyPrice.Value);
        Assert.Equal(100, actual.HistoricalPrices[1].BestSellPrice.Value);
    }

    [Fact]
    public void MapPrice_PriceWithInvalidDateString_ThrowsException()
    {
        // Arrange
        var seasonYear = SeasonYear.Create(2024);
        var price = new ListingPriceDto("abc", BestBuyPrice: 20, BestSellPrice: 10);
        var mapper = new MlbTheShowListingMapper();
        Action action = () => mapper.MapPrice(seasonYear, price);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidTheShowListingPriceDateFormatException>(actual);
    }

    [Fact]
    public void MapPrice_PriceDto_ReturnsCardListingPrice()
    {
        // Arrange
        var seasonYear = SeasonYear.Create(2024);
        var price = new ListingPriceDto("04/01", BestBuyPrice: 20, BestSellPrice: 10);
        var mapper = new MlbTheShowListingMapper();

        // Act
        var actual = mapper.MapPrice(seasonYear, price);

        // Assert
        Assert.Equal(new DateOnly(2024, 4, 1), actual.Date);
        Assert.Equal(20, actual.BestBuyPrice.Value);
        Assert.Equal(10, actual.BestSellPrice.Value);
    }
}