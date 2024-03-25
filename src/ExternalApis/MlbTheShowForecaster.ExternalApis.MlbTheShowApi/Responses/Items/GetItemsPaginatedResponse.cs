using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Items;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.Items;

/// <summary>
/// Response for a <see cref="GetItemsRequest"/>
/// </summary>
/// <param name="Page">The requested page</param>
/// <param name="PerPage">The number of results per page</param>
/// <param name="TotalPages">The total number of pages</param>
/// <param name="Items">The Items on this page</param>
public sealed record GetItemsPaginatedResponse(
    int Page,
    int PerPage,
    int TotalPages,
    [property: JsonPropertyName("items")]
    IEnumerable<ItemDto> Items
) : PaginatedResponse(Page: Page, PerPage: PerPage, TotalPages: TotalPages);