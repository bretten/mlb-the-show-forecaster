using System.Text.Json;
using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Listings;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Tests;

public class MlbTheShowApiIntegrationTests
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetMlbCardItems_FirstPage_ReturnsFirstPageResults()
    {
        // Arrange
        var request = new GetMlbCardItemsRequest(Page: 1);
        var mlbApi = RestService.For<IMlbTheShowApi>(Constants.BaseUrl2024,
            new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(
                    new JsonSerializerOptions()
                    {
                        Converters = { new JsonStringEnumConverter() }
                    }
                )
            });

        // Act
        var actual = await mlbApi.GetMlbCardItems(request);

        // Assert
        Assert.Equal(1, actual.Page);
        Assert.Equal(25, actual.PerPage);
        Assert.True(50 < actual.TotalPages);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetListings_MlbCards_ReturnsMlbCards()
    {
        // Arrange
        var request = new GetListingsRequest(Page: 1);
        var mlbApi = RestService.For<IMlbTheShowApi>(Constants.BaseUrl2024,
            new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(
                    new JsonSerializerOptions()
                    {
                        Converters = { new JsonStringEnumConverter() }
                    }
                )
            });

        // Act
        var actual = await mlbApi.GetListings(request);

        // Assert
    }
}