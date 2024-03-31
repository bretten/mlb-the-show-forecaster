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
            BestBuyPrice: NaturalNumber.Create(10),
            BestSellPrice: NaturalNumber.Create(20),
            CardExternalId.Create("id1"),
            new List<CardListingPrice>()
            {
                new(new DateOnly(2024, 3, 25), BestBuyPrice: NaturalNumber.Create(15),
                    BestSellPrice: NaturalNumber.Create(25)),
                new(new DateOnly(2024, 3, 24), BestBuyPrice: NaturalNumber.Create(10),
                    BestSellPrice: NaturalNumber.Create(20))
            }
        );
        var mapper = new ListingMapper();

        // Act
        var actual = mapper.Map(cardListing);

        // Assert
        Assert.Equal("listingName", cardListing.ListingName);
        Assert.Equal(10, actual.BuyPrice.Value);
        Assert.Equal(20, actual.SellPrice.Value);
        Assert.Equal("id1", actual.CardExternalId.Value);
        Assert.Equal(2, actual.HistoricalPricesChronologically.Count);
        Assert.Equal(new DateOnly(2024, 3, 24), actual.HistoricalPricesChronologically[0].Date);
        Assert.Equal(10, actual.HistoricalPricesChronologically[0].BuyPrice.Value);
        Assert.Equal(20, actual.HistoricalPricesChronologically[0].SellPrice.Value);
        Assert.Equal(new DateOnly(2024, 3, 25), actual.HistoricalPricesChronologically[1].Date);
        Assert.Equal(15, actual.HistoricalPricesChronologically[1].BuyPrice.Value);
        Assert.Equal(25, actual.HistoricalPricesChronologically[1].SellPrice.Value);
    }

    [Fact]
    public void Map_CardListingPriceDto_ReturnsListingHistoricalPrice()
    {
        // Arrange
        var cardListingPrice = new CardListingPrice(new DateOnly(2024, 3, 25),
            BestBuyPrice: NaturalNumber.Create(10),
            BestSellPrice: NaturalNumber.Create(20)
        );
        var mapper = new ListingMapper();

        // Act
        var actual = mapper.Map(cardListingPrice);

        // Assert
        Assert.Equal(new DateOnly(2024, 3, 25), actual.Date);
        Assert.Equal(10, actual.BuyPrice.Value);
        Assert.Equal(20, actual.SellPrice.Value);
    }
}