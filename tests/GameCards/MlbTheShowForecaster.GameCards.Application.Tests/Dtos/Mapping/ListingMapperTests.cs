using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.Mapping;

public class ListingMapperTests
{
    [Fact]
    public void Map_CardListingDto_ReturnsListing()
    {
        // Arrange
        var cardListing = new CardListing(
            "listingName",
            BestSellPrice: NaturalNumber.Create(10),
            BestBuyPrice: NaturalNumber.Create(20),
            CardExternalId.Create("id1")
        );
        var mapper = new ListingMapper();

        // Act
        var actual = mapper.Map(cardListing);

        // Assert
        Assert.Equal("listingName", cardListing.ListingName);
        Assert.Equal(10, actual.SellPrice.Value);
        Assert.Equal(20, actual.BuyPrice.Value);
        Assert.Equal("id1", actual.CardExternalId.Value);
    }

    [Fact]
    public void Map_CardListingPriceDto_ReturnsListingHistoricalPrice()
    {
        // Arrange
        var cardListingPrice = new CardListingPrice(new DateOnly(2024, 3, 25),
            BestSellPrice: NaturalNumber.Create(10),
            NaturalNumber.Create(20)
        );
        var mapper = new ListingMapper();

        // Act
        var actual = mapper.Map(cardListingPrice);

        // Assert
        Assert.Equal(new DateOnly(2024, 3, 25), actual.Date);
        Assert.Equal(10, actual.SellPrice.Value);
        Assert.Equal(20, actual.BuyPrice.Value);
    }
}