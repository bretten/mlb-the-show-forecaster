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

        // Completed orders
        var orders = new List<ListingOrderDto>()
        {
            // Will be grouped together
            new ListingOrderDto("01/16/2025 10:15:19", "1,000"),
            new ListingOrderDto("01/16/2025 10:15:19", "1,000"),

            // Will be grouped due to same minute (only seconds differ)
            new ListingOrderDto("01/16/2025 10:15:20", "1,000"),

            // Not grouped due to different price
            new ListingOrderDto("01/16/2025 10:15:19", "1,001"),

            // Not grouped due to different minute
            new ListingOrderDto("01/16/2025 10:16:19", "1,000"),

            // Another order
            new ListingOrderDto("01/15/2025 17:24:30", "3,210"),
        };
        var expectedOrders = new List<CardListingOrder>()
        {
            // All are the same
            new CardListingOrder(new DateTime(2025, 1, 16, 10, 15, 0, DateTimeKind.Utc), NaturalNumber.Create(1000),
                NaturalNumber.Create(0)),
            new CardListingOrder(new DateTime(2025, 1, 16, 10, 15, 0, DateTimeKind.Utc), NaturalNumber.Create(1000),
                NaturalNumber.Create(1)),
            new CardListingOrder(new DateTime(2025, 1, 16, 10, 15, 0, DateTimeKind.Utc), NaturalNumber.Create(1000),
                NaturalNumber.Create(2)),

            // All are different
            new CardListingOrder(new DateTime(2025, 1, 16, 10, 15, 0, DateTimeKind.Utc), NaturalNumber.Create(1001),
                NaturalNumber.Create(0)),
            new CardListingOrder(new DateTime(2025, 1, 16, 10, 16, 0, DateTimeKind.Utc), NaturalNumber.Create(1000),
                NaturalNumber.Create(0)),
            new CardListingOrder(new DateTime(2025, 1, 15, 17, 24, 0, DateTimeKind.Utc), NaturalNumber.Create(3210),
                NaturalNumber.Create(0)),
        };

        var year = SeasonYear.Create(2025);
        var listing = Faker.FakeListingDto(listingName: "name1",
            bestBuyPrice: 20,
            bestSellPrice: 10,
            itemDto: Faker.FakeMlbCardDto(uuid: Faker.FakeGuid1),
            priceHistory: priceHistory,
            completedOrders: orders);

        var stubCalendar = new Mock<ICalendar>();
        stubCalendar.Setup(x => x.TodayPst()).Returns(new DateOnly(2025, 1, 15));
        var mapper = new MlbTheShowListingMapper(stubCalendar.Object);

        /*
         * Act
         */
        var actual = mapper.Map(year, listing);

        /*
         * Assert
         */
        Assert.Equal(2025, actual.Year.Value);
        Assert.Equal("name1", actual.ListingName);
        Assert.Equal(20, actual.BestBuyPrice.Value);
        Assert.Equal(10, actual.BestSellPrice.Value);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.CardExternalId.Value);
        Assert.Equal(expectedPriceHistory, actual.HistoricalPrices);
        Assert.Equal(expectedOrders, actual.RecentOrders);
    }

    [Fact]
    public void Map_PriceWithInvalidDateString_ThrowsException()
    {
        // Arrange
        var year = SeasonYear.Create(2025);
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

        Action action = () => mapper.Map(year, listing);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidTheShowListingPriceDateFormatException>(actual);
    }

    [Fact]
    public void Map_OrderWithInvalidDateString_ThrowsException()
    {
        // Arrange
        var year = SeasonYear.Create(2025);
        var listing = Faker.FakeListingDto(listingName: "name1",
            bestBuyPrice: 20,
            bestSellPrice: 10,
            itemDto: Faker.FakeMlbCardDto(uuid: Faker.FakeGuid1),
            priceHistory: new List<ListingPriceDto>(),
            completedOrders: new List<ListingOrderDto>()
            {
                new ListingOrderDto("01/15/2025 17:24", "3,021"), // Expects HH:mm:ss not just HH:mm
            });

        var stubCalendar = new Mock<ICalendar>();
        stubCalendar.Setup(x => x.TodayPst()).Returns(new DateOnly(2025, 1, 16));
        var mapper = new MlbTheShowListingMapper(stubCalendar.Object);

        Action action = () => mapper.Map(year, listing);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidTheShowListingOrderDateFormatException>(actual);
    }
}