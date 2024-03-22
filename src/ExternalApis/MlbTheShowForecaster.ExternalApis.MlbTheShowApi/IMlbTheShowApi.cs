using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Listings;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Listings;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.Listings;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi;

/// <summary>
/// MLB The Show API - https://mlb24.theshow.com/apis/docs/item (subdomains exist for mlb21 - mlb24)
///
/// <para>Allows integration with the MLB The Show API so that Items, Listings and Roster Updates can be retrieved</para>
/// </summary>
public interface IMlbTheShowApi
{
    /// <summary>
    /// Requests a single Item by UUID
    /// </summary>
    /// <param name="request">The request query parameters</param>
    /// <returns>The corresponding Item</returns>
    [Get("/apis/item.json")]
    Task<ItemDto> GetItem([Query] GetItemRequest request);

    /// <summary>
    /// Requests Items of a specified type
    /// </summary>
    /// <param name="request">The request query parameters</param>
    /// <returns>The matching Items</returns>
    [Get("/apis/items.json")]
    Task<GetItemsPaginatedResponse> GetItems([Query] GetItemsRequest request);

    /// <summary>
    /// Requests a single Listing by an Item UUID
    /// </summary>
    /// <param name="request">The request query parameters</param>
    /// <returns>The matching Listing</returns>
    [Get("/apis/listing.json")]
    Task<ListingDto<ItemDto>> GetListing([Query] GetListingRequest request);

    /// <summary>
    /// Requests Listings that match the specified request query parameters
    /// </summary>
    /// <param name="request">The request query parameters</param>
    /// <returns>The matching Listings</returns>
    [Get("/apis/listings.json")]
    Task<GetListingsPaginatedResponse> GetListings([Query] GetListingsRequest request);
}