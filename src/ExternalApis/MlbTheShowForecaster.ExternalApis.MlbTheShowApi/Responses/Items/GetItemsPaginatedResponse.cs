using System.Text.Json.Serialization;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.Items;

/// <summary>
/// Defines a paginated response for Items
/// </summary>
/// <param name="Page">The requested page</param>
/// <param name="PerPage">The number of results per page</param>
/// <param name="TotalPages">The total number of pages</param>
/// <param name="Items">The Items on this page</param>
/// <typeparam name="T">Any derivation of <see cref="ItemDto"/></typeparam>
public abstract record GetItemsPaginatedResponse<T>(
    int Page,
    int PerPage,
    int TotalPages,
    [property: JsonPropertyName("items")]
    IEnumerable<T> Items
) : PaginatedResponse(Page: Page, PerPage: PerPage, TotalPages: TotalPages)
    where T : ItemDto;