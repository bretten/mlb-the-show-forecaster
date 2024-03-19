using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.Items;

/// <summary>
/// Response for getting MLB card Items
/// </summary>
/// <param name="Page">The requested page</param>
/// <param name="PerPage">The number of results per page</param>
/// <param name="TotalPages">The total number of pages</param>
/// <param name="Items">The MLB card Items</param>
public sealed record GetMlbCardItemsResponse(
    int Page,
    int PerPage,
    int TotalPages,
    IEnumerable<MlbCardDto> Items
) : PaginatedItemsResponse<MlbCardDto>(Page, PerPage, TotalPages, Items);