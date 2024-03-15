using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Marketplace.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Marketplace.Entities;

public class ListingTests
{
    [Fact]
    public void HistoricalPricesChronologically_MultipleHistoricalPrices_ReturnsHistoricalPricesInOrderOfDate()
    {
        // Arrange
        var listing = Faker.FakeListing(buyPrice: 2, sellPrice: 2);
        listing.ArchivePrice(new DateOnly(2024, 4, 2));
        listing.UpdatePrices(NaturalNumber.Create(1), NaturalNumber.Create(1));
        listing.ArchivePrice(new DateOnly(2024, 4, 1));

        // Act
        var actual = listing.HistoricalPricesChronologically;

        // Assert
        Assert.Equal(2, actual.Count);
        Assert.Equal(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1), 1, 1), actual[0]);
        Assert.Equal(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 2), 2, 2), actual[1]);
    }

    [Fact]
    public void ArchivePrice_ArchiveDateAlreadyExists_ThrowsException()
    {
        // Arrange
        var date = new DateOnly(2024, 4, 1);
        var listing = Faker.FakeListing(buyPrice: 1, sellPrice: 1);
        listing.ArchivePrice(date);
        var action = () => listing.ArchivePrice(date);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<ListingHistoricalPriceExistsException>(actual);
    }

    [Fact]
    public void ArchivePrice_ListingWithCurrentPrices_AddsPricesToHistoricalCollection()
    {
        // Arrange
        var date = new DateOnly(2024, 4, 1);
        var listing = Faker.FakeListing(buyPrice: 1, sellPrice: 1);

        // Act
        listing.ArchivePrice(date);

        // Assert
        Assert.Equal(1, listing.HistoricalPricesChronologically.Count);
        Assert.Equal(Faker.FakeListingHistoricalPrice(new DateOnly(2024, 4, 1), 1, 1),
            listing.HistoricalPricesChronologically[0]);
    }

    [Fact]
    public void UpdatePrice_NewPrices_ChangesPricesOnListing()
    {
        // Arrange
        var listing = Faker.FakeListing(buyPrice: 1, sellPrice: 1);

        // Act
        listing.UpdatePrices(NaturalNumber.Create(2), NaturalNumber.Create(3));

        // Assert
        Assert.Equal(2, listing.BuyPrice.Value);
        Assert.Equal(3, listing.SellPrice.Value);
    }
}