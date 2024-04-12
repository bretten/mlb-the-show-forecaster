using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Listings;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.RosterUpdates;
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
        Assert.Equal("a71cdf423ea5906c5fa85fff95d90360", actualItem!.Uuid.ValueAsString);
        Assert.Equal("mlb_card", actualItem.Type);
        Assert.Equal(
            "https://mlb24.theshow.com/rails/active_storage/blobs/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCR2MvRFJNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--a6aeae283163aa6cb6bf114e18ab406978aabf4f/2d1fb7af6a8075ea3e67b28d066e2556.webp",
            actualItem.ImageUrl);
        Assert.Equal("Shohei Ohtani", actualItem.Name);
        Assert.Equal("Diamond", actualItem.Rarity);
        Assert.True(actualItem.IsSellable);
        Assert.Equal("Live", actualItem.Series);
        Assert.Equal("LAD", actualItem.TeamShortName);
        Assert.Equal("SP", actualItem.DisplayPosition);
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
        Assert.Equal("7520fa31d14f45add6d61e52df5a03ff", actualItem!.Uuid.ValueAsString);
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
        Assert.Equal("a71cdf423ea5906c5fa85fff95d90360", actualItem!.Uuid.ValueAsString);
        Assert.Equal("mlb_card", actualItem.Type);
        Assert.Equal(
            "https://mlb24.theshow.com/rails/active_storage/blobs/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCR2MvRFJNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--a6aeae283163aa6cb6bf114e18ab406978aabf4f/2d1fb7af6a8075ea3e67b28d066e2556.webp",
            actualItem.ImageUrl);
        Assert.Equal("Shohei Ohtani", actualItem.Name);
        Assert.Equal("Diamond", actualItem.Rarity);
        Assert.Equal("Live", actualItem.Series);
        Assert.Equal("LAD", actualItem.TeamShortName);
        Assert.NotNull(actual.PriceHistory);
        Assert.True(0 < actual.PriceHistory.Count);
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
        Assert.Equal("7520fa31d14f45add6d61e52df5a03ff", actualItem!.Uuid.ValueAsString);
        Assert.Equal("stadium", actualItem.Type);
        Assert.Equal(
            "https://mlb24.theshow.com/rails/active_storage/blobs/redirect/eyJfcmFpbHMiOnsibWVzc2FnZSI6IkJBaHBCRUtsRFJNPSIsImV4cCI6bnVsbCwicHVyIjoiYmxvYl9pZCJ9fQ==--1cde10216309186128b5554d70496996a8598e9f/4d92741ae4ca5675c82b335638d512fa.webp",
            actualItem.ImageUrl);
        Assert.Equal("Crosley Field", actualItem.Name);
        Assert.Equal("Diamond", actualItem.Rarity);
        Assert.Equal("FA", actualItem.TeamShortName);
        Assert.NotNull(actual.PriceHistory);
        Assert.True(0 < actual.PriceHistory.Count);
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
        Assert.Null(actual.Listings.ElementAt(0).PriceHistory); // No price history when getting all listings
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
        Assert.Null(actual.Listings.ElementAt(0).PriceHistory); // No price history when getting all listings
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetRosterUpdate_Id_ReturnsRosterUpdate()
    {
        // Arrange
        var request = new GetRosterUpdateRequest(Id: 4);
        var mlbApi = GetClient(Constants.BaseUrl2023);

        // Act
        var actual = await mlbApi.GetRosterUpdate(request);

        // Assert
        Assert.Equal(384, actual.PlayerAttributeChanges.Count());
        var actualPlayerAttributeChange = actual.PlayerAttributeChanges.ElementAt(0);
        Assert.Equal("f1105c2f23b2c673114e6c8a16b135b2", actualPlayerAttributeChange.Uuid.ValueAsString);
        Assert.Equal("Shohei Ohtani", actualPlayerAttributeChange.Name);
        Assert.Equal("f1105c2f23b2c673114e6c8a16b135b2", actualPlayerAttributeChange.Item.Uuid.ValueAsString);
        Assert.Equal("Angels", actualPlayerAttributeChange.Team);
        Assert.Equal(96, actualPlayerAttributeChange.CurrentRank);
        Assert.Equal("Diamond", actualPlayerAttributeChange.CurrentRarity);
        Assert.Equal(96, actualPlayerAttributeChange.OldRank);
        Assert.Equal("Diamond", actualPlayerAttributeChange.OldRarity);
        var actualAttributeChange = actualPlayerAttributeChange.Changes.ElementAt(0);
        Assert.Equal("H/9", actualAttributeChange.Name);
        Assert.Equal("103", actualAttributeChange.CurrentValue);
        Assert.Equal("positive", actualAttributeChange.Direction);
        Assert.Equal("+5", actualAttributeChange.Delta);
        Assert.Equal("orange", actualAttributeChange.Color);

        Assert.Equal(45, actual.PlayerPositionChanges.Count());
        var actualPlayerPositionChange = actual.PlayerPositionChanges.ElementAt(0);
        Assert.Equal("a6f6b649715373a58392de57da7b4dff", actualPlayerPositionChange.Uuid.ValueAsString);
        Assert.Equal("Yandy Diaz", actualPlayerPositionChange.Name);
        Assert.Equal("a6f6b649715373a58392de57da7b4dff", actualPlayerPositionChange.Item.Uuid.ValueAsString);
        Assert.Equal("1B", actualPlayerPositionChange.Position);
        Assert.Equal("Rays", actualPlayerPositionChange.Team);

        Assert.Equal(15, actual.NewlyAddedPlayers.Count());
        var actualNewlyAddedPlayer = actual.NewlyAddedPlayers.ElementAt(0);
        Assert.Equal("acec9b353f1bb1005cdcec9ec34a0142", actualNewlyAddedPlayer.Uuid.ValueAsString);
        Assert.Equal("Jake Marisnick", actualNewlyAddedPlayer.Name);
        Assert.Equal("White Sox", actualNewlyAddedPlayer.Team);
        Assert.Equal("CF", actualNewlyAddedPlayer.Position);
        Assert.Equal(72, actualNewlyAddedPlayer.CurrentRank);
        Assert.Equal("Bronze", actualNewlyAddedPlayer.CurrentRarity);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetRosterUpdates_NoParameter_ReturnsRosterUpdates()
    {
        // Arrange
        var mlbApi = GetClient(Constants.BaseUrl2023);

        // Act
        var actual = await mlbApi.GetRosterUpdates();

        // Assert
        Assert.Equal(26, actual.RosterUpdates.Count());
        var actualFirstUpdate = actual.RosterUpdates.ElementAt(25);
        Assert.Equal(1, actualFirstUpdate.Id);
        Assert.Equal("April 21, 2023", actualFirstUpdate.Name);
        Assert.Equal(new DateOnly(2023, 4, 21), actualFirstUpdate.Date);
        var actualLastUpdate = actual.RosterUpdates.ElementAt(0);
        Assert.Equal(26, actualLastUpdate.Id);
        Assert.Equal("November 10, 2023", actualLastUpdate.Name);
        Assert.Equal(new DateOnly(2023, 11, 10), actualLastUpdate.Date);
    }

    private static IMlbTheShowApi GetClient(string baseUrl = Constants.BaseUrl2024)
    {
        return RestService.For<IMlbTheShowApi>(baseUrl,
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