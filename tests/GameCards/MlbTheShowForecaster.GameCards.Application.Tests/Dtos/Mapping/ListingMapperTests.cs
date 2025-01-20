using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;

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
            Faker.FakeCardExternalId(Faker.FakeGuid1),
            new List<CardListingPrice>()
            {
                new(new DateOnly(2024, 3, 25), BestBuyPrice: NaturalNumber.Create(15),
                    BestSellPrice: NaturalNumber.Create(25)),
                new(new DateOnly(2024, 3, 24), BestBuyPrice: NaturalNumber.Create(10),
                    BestSellPrice: NaturalNumber.Create(20))
            },
            new List<CardListingOrder>()
            {
                new CardListingOrder(new DateTime(2025, 1, 17, 10, 20, 0),
                    Price: NaturalNumber.Create(10),
                    Quantity: NaturalNumber.Create(20)
                ),
                new CardListingOrder(new DateTime(2025, 1, 17, 10, 21, 0),
                    Price: NaturalNumber.Create(100),
                    Quantity: NaturalNumber.Create(200)
                ),
            }
        );
        var mapper = new ListingMapper();

        // Act
        var actual = mapper.Map(cardListing);

        // Assert
        Assert.Equal("listingName", cardListing.ListingName);
        Assert.Equal(10, actual.BuyPrice.Value);
        Assert.Equal(20, actual.SellPrice.Value);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.CardExternalId.Value);

        Assert.Equal(2, actual.HistoricalPricesChronologically.Count);
        Assert.Equal(new DateOnly(2024, 3, 24), actual.HistoricalPricesChronologically[0].Date);
        Assert.Equal(10, actual.HistoricalPricesChronologically[0].BuyPrice.Value);
        Assert.Equal(20, actual.HistoricalPricesChronologically[0].SellPrice.Value);
        Assert.Equal(new DateOnly(2024, 3, 25), actual.HistoricalPricesChronologically[1].Date);
        Assert.Equal(15, actual.HistoricalPricesChronologically[1].BuyPrice.Value);
        Assert.Equal(25, actual.HistoricalPricesChronologically[1].SellPrice.Value);

        Assert.Equal(2, actual.OrdersChronologically.Count);
        Assert.Equal(new DateTime(2025, 1, 17, 10, 20, 0), actual.OrdersChronologically[0].Date);
        Assert.Equal(10, actual.OrdersChronologically[0].Price.Value);
        Assert.Equal(20, actual.OrdersChronologically[0].Quantity.Value);
        Assert.Equal(new DateTime(2025, 1, 17, 10, 21, 0), actual.OrdersChronologically[1].Date);
        Assert.Equal(100, actual.OrdersChronologically[1].Price.Value);
        Assert.Equal(200, actual.OrdersChronologically[1].Quantity.Value);
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

    [Fact]
    public void Map_CardListingOrder_ReturnsListingOrder()
    {
        // Arrange
        var order = new CardListingOrder(new DateTime(2025, 1, 17, 10, 20, 0),
            Price: NaturalNumber.Create(10),
            Quantity: NaturalNumber.Create(20)
        );
        var mapper = new ListingMapper();

        // Act
        var actual = mapper.Map(order);

        // Assert
        Assert.Equal(new DateTime(2025, 1, 17, 10, 20, 0), actual.Date);
        Assert.Equal(10, actual.Price.Value);
        Assert.Equal(20, actual.Quantity.Value);
    }
}