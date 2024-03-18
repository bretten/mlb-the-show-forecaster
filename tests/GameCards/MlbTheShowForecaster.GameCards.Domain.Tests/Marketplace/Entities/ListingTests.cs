using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Entities.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Marketplace.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Marketplace.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Marketplace.Entities;

public class ListingTests
{
    [Fact]
    public void HistoricalPricesChronologically_MultipleHistoricalPrices_ReturnsHistoricalPricesInOrderOfDate()
    {
        // Arrange
        var listing = Faker.FakeListing(buyPrice: 2, sellPrice: 2);
        listing.ArchivePrice(new DateOnly(2024, 4, 2));
        listing.UpdatePrices(NaturalNumber.Create(1), NaturalNumber.Create(1),
            Mock.Of<IListingPriceSignificantChangeThreshold>());
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
        listing.UpdatePrices(NaturalNumber.Create(2), NaturalNumber.Create(3),
            Mock.Of<IListingPriceSignificantChangeThreshold>());

        // Assert
        Assert.Equal(2, listing.BuyPrice.Value);
        Assert.Equal(3, listing.SellPrice.Value);
    }

    [Fact]
    public void UpdatePrice_BuyPriceIncreasedSignificantly_RaisesBuyPriceIncreasedDomainEvent()
    {
        // Arrange
        var listing = Faker.FakeListing(buyPrice: 10, sellPrice: 10);
        var stubPriceChangeThreshold = new Mock<IListingPriceSignificantChangeThreshold>();
        stubPriceChangeThreshold.Setup(x => x.BuyPricePercentageChangeThreshold)
            .Returns(50);

        // Act
        listing.UpdatePrices(NaturalNumber.Create(20), NaturalNumber.Create(10), stubPriceChangeThreshold.Object);

        // Assert
        Assert.Single(listing.DomainEvents);
        Assert.IsType<ListingBuyPriceIncreasedEvent>(listing.DomainEvents[0]);
        var e = listing.DomainEvents[0] as ListingBuyPriceIncreasedEvent;
        Assert.Equal(10, e!.OriginalPrice.Value);
        Assert.Equal(20, e.NewPrice.Value);
        Assert.Equal(100, e.PercentageChange.PercentageChangeValue);
    }

    [Fact]
    public void UpdatePrice_BuyPriceDecreasedSignificantly_RaisesBuyPriceDecreasedDomainEvent()
    {
        // Arrange
        var listing = Faker.FakeListing(buyPrice: 20, sellPrice: 10);
        var stubPriceChangeThreshold = new Mock<IListingPriceSignificantChangeThreshold>();
        stubPriceChangeThreshold.Setup(x => x.BuyPricePercentageChangeThreshold)
            .Returns(50);

        // Act
        listing.UpdatePrices(NaturalNumber.Create(10), NaturalNumber.Create(10), stubPriceChangeThreshold.Object);

        // Assert
        Assert.Single(listing.DomainEvents);
        Assert.IsType<ListingBuyPriceDecreasedEvent>(listing.DomainEvents[0]);
        var e = listing.DomainEvents[0] as ListingBuyPriceDecreasedEvent;
        Assert.Equal(20, e!.OriginalPrice.Value);
        Assert.Equal(10, e.NewPrice.Value);
        Assert.Equal(-50m, e.PercentageChange.PercentageChangeValue);
    }

    [Fact]
    public void UpdatePrice_BuyPriceHadNegligibleChange_NoDomainEventRaised()
    {
        // Arrange
        var listing = Faker.FakeListing(buyPrice: 10, sellPrice: 10);
        var stubPriceChangeThresholds = new Mock<IListingPriceSignificantChangeThreshold>();
        stubPriceChangeThresholds.Setup(x => x.BuyPricePercentageChangeThreshold)
            .Returns(50);

        // Act
        listing.UpdatePrices(NaturalNumber.Create(10), NaturalNumber.Create(11), stubPriceChangeThresholds.Object);

        // Assert
        Assert.Empty(listing.DomainEvents);
    }

    [Fact]
    public void UpdatePrice_SellPriceIncreasedSignificantly_RaisesSellPriceIncreasedDomainEvent()
    {
        // Arrange
        var listing = Faker.FakeListing(buyPrice: 10, sellPrice: 10);
        var stubPriceChangeThreshold = new Mock<IListingPriceSignificantChangeThreshold>();
        stubPriceChangeThreshold.Setup(x => x.SellPricePercentageChangeThreshold)
            .Returns(50);

        // Act
        listing.UpdatePrices(NaturalNumber.Create(10), NaturalNumber.Create(20), stubPriceChangeThreshold.Object);

        // Assert
        Assert.Single(listing.DomainEvents);
        Assert.IsType<ListingSellPriceIncreasedEvent>(listing.DomainEvents[0]);
        var e = listing.DomainEvents[0] as ListingSellPriceIncreasedEvent;
        Assert.Equal(10, e!.OriginalPrice.Value);
        Assert.Equal(20, e.NewPrice.Value);
        Assert.Equal(100, e.PercentageChange.PercentageChangeValue);
    }

    [Fact]
    public void UpdatePrice_SellPriceDecreasedSignificantly_RaisesSellPriceDecreasedDomainEvent()
    {
        // Arrange
        var listing = Faker.FakeListing(buyPrice: 10, sellPrice: 20);
        var stubPriceChangeThreshold = new Mock<IListingPriceSignificantChangeThreshold>();
        stubPriceChangeThreshold.Setup(x => x.SellPricePercentageChangeThreshold)
            .Returns(50);

        // Act
        listing.UpdatePrices(NaturalNumber.Create(10), NaturalNumber.Create(10), stubPriceChangeThreshold.Object);

        // Assert
        Assert.Single(listing.DomainEvents);
        Assert.IsType<ListingSellPriceDecreasedEvent>(listing.DomainEvents[0]);
        var e = listing.DomainEvents[0] as ListingSellPriceDecreasedEvent;
        Assert.Equal(20, e!.OriginalPrice.Value);
        Assert.Equal(10, e.NewPrice.Value);
        Assert.Equal(-50m, e.PercentageChange.PercentageChangeValue);
    }

    [Fact]
    public void UpdatePrice_SellPriceHadNegligibleChange_NoDomainEventRaised()
    {
        // Arrange
        var listing = Faker.FakeListing(buyPrice: 10, sellPrice: 10);
        var stubPriceChangeThresholds = new Mock<IListingPriceSignificantChangeThreshold>();
        stubPriceChangeThresholds.Setup(x => x.SellPricePercentageChangeThreshold)
            .Returns(50);

        // Act
        listing.UpdatePrices(NaturalNumber.Create(10), NaturalNumber.Create(11), stubPriceChangeThresholds.Object);

        // Assert
        Assert.Empty(listing.DomainEvents);
    }
}