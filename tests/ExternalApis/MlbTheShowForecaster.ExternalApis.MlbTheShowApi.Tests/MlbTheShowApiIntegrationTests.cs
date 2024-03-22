using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Listings;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Tests;

public class MlbTheShowApiIntegrationTests
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetItem_MlbCardUuid_ReturnsMlbCard()
    {
        // Arrange
        var request = new GetItemRequest(Uuid: "a71cdf423ea5906c5fa85fff95d90360");
        var mlbApi = GetClient();

        // Act
        var actual = await mlbApi.GetItem(request);

        // Assert
        Assert.IsType<MlbCardDto>(actual);
        var actualItem = actual as MlbCardDto;
        Assert.Equal("a71cdf423ea5906c5fa85fff95d90360", actualItem!.Uuid);
        Assert.Equal("mlb_card", actualItem.Type);
        Assert.Equal(
            "https://mlb24.theshow.com/rails/active_storage/blobs/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCR2MvRFJNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--a6aeae283163aa6cb6bf114e18ab406978aabf4f/2d1fb7af6a8075ea3e67b28d066e2556.webp",
            actualItem.ImageUrl);
        Assert.Equal("Shohei Ohtani", actualItem.Name);
        Assert.Equal("Diamond", actualItem.Rarity);
        Assert.True(actualItem.IsSellable);
        Assert.Equal("Live", actualItem.Series);
        Assert.Equal("LAD", actualItem.TeamShortName);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetItem_StadiumUuid_ReturnsStadium()
    {
        // Arrange
        var request = new GetItemRequest(Uuid: "7520fa31d14f45add6d61e52df5a03ff");
        var mlbApi = GetClient();

        // Act
        var actual = await mlbApi.GetItem(request);

        // Assert
        Assert.IsType<StadiumDto>(actual);
        var actualItem = actual as StadiumDto;
        Assert.Equal("7520fa31d14f45add6d61e52df5a03ff", actualItem!.Uuid);
        Assert.Equal("stadium", actualItem.Type);
        Assert.Equal(
            "https://mlb24.theshow.com/rails/active_storage/blobs/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCRUtsRFJNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--1cde10216309186128b5554d70496996a8598e9f/4d92741ae4ca5675c82b335638d512fa.webp",
            actualItem.ImageUrl);
        Assert.Equal("Crosley Field", actualItem.Name);
        Assert.Equal("Diamond", actualItem.Rarity);
        Assert.True(actualItem.IsSellable);
        Assert.Equal("FA", actualItem.TeamShortName);
        Assert.Equal("29,603", actualItem.Capacity);
        Assert.Equal("Grass", actualItem.Surface);
        Assert.Equal("550ft", actualItem.Elevation);
        Assert.Equal(1912, actualItem.Built);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetItems_ItemTypeMlbCard_ReturnsMlbCards()
    {
        // Arrange
        var request = new GetItemsRequest(Page: 1, Type: ItemType.MlbCard);
        var mlbApi = GetClient();

        // Act
        var actual = await mlbApi.GetItems(request);

        // Assert
        Assert.Equal(1, actual.Page);
        Assert.Equal(25, actual.PerPage);
        Assert.True(0 < actual.TotalPages);
        Assert.IsType<MlbCardDto>(actual.Items.ElementAt(0));
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetItems_ItemTypeStadium_ReturnsStadiums()
    {
        // Arrange
        var request = new GetItemsRequest(Page: 1, Type: ItemType.Stadium);
        var mlbApi = GetClient();

        // Act
        var actual = await mlbApi.GetItems(request);

        // Assert
        Assert.Equal(1, actual.Page);
        Assert.Equal(25, actual.PerPage);
        Assert.True(0 < actual.TotalPages);
        Assert.IsType<StadiumDto>(actual.Items.ElementAt(0));
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetListing_MlbCardUuid_ReturnsMlbCardListing()
    {
        // Arrange
        var request = new GetListingRequest(Uuid: "a71cdf423ea5906c5fa85fff95d90360");
        var mlbApi = GetClient();

        // Act
        var actual = await mlbApi.GetListing(request);

        // Assert
        Assert.Equal("Shohei Ohtani", actual.ListingName);
        Assert.True(0 < actual.BestBuyPrice);
        Assert.True(0 < actual.BestSellPrice);
        Assert.IsType<MlbCardDto>(actual.Item);
        var actualItem = actual.Item as MlbCardDto;
        Assert.Equal("a71cdf423ea5906c5fa85fff95d90360", actualItem!.Uuid);
        Assert.Equal("mlb_card", actualItem.Type);
        Assert.Equal(
            "https://mlb24.theshow.com/rails/active_storage/blobs/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCR2MvRFJNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--a6aeae283163aa6cb6bf114e18ab406978aabf4f/2d1fb7af6a8075ea3e67b28d066e2556.webp",
            actualItem.ImageUrl);
        Assert.Equal("Shohei Ohtani", actualItem.Name);
        Assert.Equal("Diamond", actualItem.Rarity);
        Assert.Equal("Live", actualItem.Series);
        Assert.Equal("LAD", actualItem.TeamShortName);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetListing_StadiumUuid_ReturnsStadiumListing()
    {
        // Arrange
        var request = new GetListingRequest(Uuid: "7520fa31d14f45add6d61e52df5a03ff");
        var mlbApi = GetClient();

        // Act
        var actual = await mlbApi.GetListing(request);

        // Assert
        Assert.Equal("Crosley Field", actual.ListingName);
        Assert.True(0 < actual.BestBuyPrice);
        Assert.True(0 < actual.BestSellPrice);
        Assert.IsType<StadiumDto>(actual.Item);
        var actualItem = actual.Item as StadiumDto;
        Assert.Equal("7520fa31d14f45add6d61e52df5a03ff", actualItem!.Uuid);
        Assert.Equal("stadium", actualItem.Type);
        Assert.Equal(
            "https://mlb24.theshow.com/rails/active_storage/blobs/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCRUtsRFJNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--1cde10216309186128b5554d70496996a8598e9f/4d92741ae4ca5675c82b335638d512fa.webp",
            actualItem.ImageUrl);
        Assert.Equal("Crosley Field", actualItem.Name);
        Assert.Equal("Diamond", actualItem.Rarity);
        Assert.Equal("FA", actualItem.TeamShortName);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetListings_ItemTypeMlbCard_ReturnsMlbCards()
    {
        // Arrange
        var request = new GetListingsRequest(Page: 1, Type: ItemType.MlbCard);
        var mlbApi = GetClient();

        // Act
        var actual = await mlbApi.GetListings(request);

        // Assert
        Assert.Equal(1, actual.Page);
        Assert.Equal(25, actual.PerPage);
        Assert.True(0 < actual.TotalPages);
        Assert.IsType<MlbCardDto>(actual.Listings.ElementAt(0).Item);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetListings_ItemTypeStadium_ReturnsStadiums()
    {
        // Arrange
        var request = new GetListingsRequest(Page: 1, Type: ItemType.Stadium);
        var mlbApi = GetClient();

        // Act
        var actual = await mlbApi.GetListings(request);

        // Assert
        Assert.Equal(1, actual.Page);
        Assert.Equal(25, actual.PerPage);
        Assert.True(0 < actual.TotalPages);
        Assert.IsType<StadiumDto>(actual.Listings.ElementAt(0).Item);
    }

    private static IMlbTheShowApi GetClient()
    {
        return RestService.For<IMlbTheShowApi>(Constants.BaseUrl2024,
            new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(
                    new JsonSerializerOptions()
                    {
                        Converters = { new JsonStringEnumConverter() }
                    }
                )
            });
    }
}