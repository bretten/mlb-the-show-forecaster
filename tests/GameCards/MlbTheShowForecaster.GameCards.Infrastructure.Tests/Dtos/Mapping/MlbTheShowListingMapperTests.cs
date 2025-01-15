using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Listings;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Dtos.Mapping.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Dtos.Mapping;

public class MlbTheShowListingMapperTests
{
    [Fact]
    public void Map_ListingDto_ReturnsCardListing()
    {
        /*
         * Arrange
         */
        // Listing prices that emulate the erroneous data from the MLB The Show API: https://github.com/bretten/mlb-the-show-forecaster/issues/420
        var priceHistory = new List<ListingPriceDto>();
        var expectedPriceHistory = new List<CardListingPrice>();
        // Erroneous December prices
        for (int i = 31; i >= 3; i--)
        {
            priceHistory.Add(new ListingPriceDto($"12/{i}", BestBuyPrice: i * 10, BestSellPrice: i * 100));
        }

        // Actual price history
        var today = new DateOnly(2025, 1, 15);
        while (today >= new DateOnly(2024, 11, 20))
        {
            // The DTO input
            priceHistory.Add(new ListingPriceDto(today.ToString("MM/dd"), BestBuyPrice: today.DayOfYear,
                BestSellPrice: today.DayOfYear * 10));
            // The expect price output
            expectedPriceHistory.Add(new CardListingPrice(today, BestBuyPrice: NaturalNumber.Create(today.DayOfYear),
                BestSellPrice: NaturalNumber.Create(today.DayOfYear * 10)));

            today = today.AddDays(-1);
        }

        var listing = Faker.FakeListingDto(listingName: "name1",
            bestBuyPrice: 20,
            bestSellPrice: 10,
            itemDto: Faker.FakeMlbCardDto(uuid: Faker.FakeGuid1),
            priceHistory: priceHistory);

        var stubCalendar = new Mock<ICalendar>();
        stubCalendar.Setup(x => x.TodayPst()).Returns(new DateOnly(2025, 1, 15));
        var mapper = new MlbTheShowListingMapper(stubCalendar.Object);

        /*
         * Act
         */
        var actual = mapper.Map(listing);

        /*
         * Assert
         */
        Assert.Equal("name1", actual.ListingName);
        Assert.Equal(20, actual.BestBuyPrice.Value);
        Assert.Equal(10, actual.BestSellPrice.Value);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.CardExternalId.Value);
        Assert.Equal(expectedPriceHistory, actual.HistoricalPrices);
    }

    [Fact]
    public void Map_PriceWithInvalidDateString_ThrowsException()
    {
        // Arrange
        var listing = Faker.FakeListingDto(listingName: "name1",
            bestBuyPrice: 20,
            bestSellPrice: 10,
            itemDto: Faker.FakeMlbCardDto(uuid: Faker.FakeGuid1),
            priceHistory: new List<ListingPriceDto>()
            {
                new ListingPriceDto("01/15", BestBuyPrice: 20, BestSellPrice: 10),
                new ListingPriceDto("abc", BestBuyPrice: 20, BestSellPrice: 10) // Invalid date string
            });

        var stubCalendar = new Mock<ICalendar>();
        stubCalendar.Setup(x => x.TodayPst()).Returns(new DateOnly(2025, 1, 15));
        var mapper = new MlbTheShowListingMapper(stubCalendar.Object);

        Action action = () => mapper.Map(listing);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidTheShowListingPriceDateFormatException>(actual);
    }
}