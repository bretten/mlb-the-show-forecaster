using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Listings;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Listings;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.Listings;

/// <summary>
/// Response for a <see cref="GetListingsRequest"/>
/// </summary>
/// <param name="Page">The requested page</param>
/// <param name="PerPage">The number of results per page</param>
/// <param name="TotalPages">The total number of pages</param>
/// <param name="Listings">The Listings of Items on this page</param>
public sealed record GetListingsPaginatedResponse(
    int Page,
    int PerPage,
    int TotalPages,
    [property: JsonPropertyName("listings")]
    IEnumerable<ListingDto<ItemDto>> Listings
) : PaginatedResponse(Page: Page, PerPage: PerPage, TotalPages: TotalPages);