using AngleSharp;
using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Services;

public class MlbTheShowComCardMarketplaceIntegrationTests
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetCardPrice_CardExternalIdWithNoMatch_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var season = SeasonYear.Create(2024);
        var cardExternalId = CardExternalId.Create(Faker.FakeGuid1);

        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var calendar = new Calendar();

        var marketplace = new MlbTheShowComCardMarketplace(context, calendar);

        var action = () => marketplace.GetCardPrice(season, cardExternalId, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<CardListingNotFoundInMarketplaceException>(actual);
    }

    [Theory]
    [Trait("Category", "Integration")]
    [InlineData(2024, "7d6c7d95a1e5e861c54d20002585a809", "Aaron Judge")]
    [InlineData(2023, "0fbc90ea428a06eae66e4e7807decf8b", "Aaron Judge")]
    [InlineData(2022, "bacd19322bb8542d67d13c121efc3fea", "Aaron Judge")]
    //[InlineData(2021, "9ed1f55ac4f3b402b1d08b26870c34a6", "Aaron Judge")] // Data is inconsistent
    public async Task GetCardPrice_SeasonYearAndCardExternalId_ReturnsCardPrice(ushort year, string guid, string name)
    {
        // Arrange
        var cToken = CancellationToken.None;
        var season = SeasonYear.Create(year);
        var cardExternalId = CardExternalId.Create(new Guid(guid));

        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var calendar = new Calendar();

        var marketplace = new MlbTheShowComCardMarketplace(context, calendar);

        // Act
        var actual = await marketplace.GetCardPrice(season, cardExternalId, cToken);

        // Assert
        Assert.Equal(name, actual.ListingName);
        Assert.Equal(new Guid(guid), actual.CardExternalId.Value);
        var mostRecentPrice = actual.HistoricalPrices.OrderByDescending(x => x.Date).First();
        Assert.Equal(mostRecentPrice.BestBuyPrice, actual.BestBuyPrice);
        Assert.Equal(mostRecentPrice.BestSellPrice, actual.BestSellPrice);
        var oldestPrice = actual.HistoricalPrices.OrderBy(x => x.Date).First();
        Assert.Equal(year, oldestPrice.Date.Year);
    }
}