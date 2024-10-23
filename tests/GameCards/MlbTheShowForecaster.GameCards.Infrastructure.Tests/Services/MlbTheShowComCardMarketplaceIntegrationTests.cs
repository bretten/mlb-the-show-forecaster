using AngleSharp;
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

        var marketplace = new MlbTheShowComCardMarketplace(context);

        var action = () => marketplace.GetCardPrice(season, cardExternalId, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<CardListingNotFoundInMarketplaceException>(actual);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetCardPrice_SeasonYearAndCardExternalId_ReturnsCardPrice()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var season = SeasonYear.Create(2024);
        var cardExternalId = CardExternalId.Create(new Guid("7d6c7d95a1e5e861c54d20002585a809"));

        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);

        var marketplace = new MlbTheShowComCardMarketplace(context);

        // Act
        var actual = await marketplace.GetCardPrice(season, cardExternalId, cToken);

        // Assert
        Assert.Equal("Aaron Judge", actual.ListingName);
        Assert.Equal(new Guid("7d6c7d95a1e5e861c54d20002585a809"), actual.CardExternalId.Value);
        var mostRecentPrice = actual.HistoricalPrices.OrderByDescending(x => x.Date).First();
        Assert.Equal(mostRecentPrice.BestBuyPrice, actual.BestBuyPrice);
        Assert.Equal(mostRecentPrice.BestSellPrice, actual.BestSellPrice);
    }
}