using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using Refit;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Items;

/// <summary>
/// Request for getting Items
/// </summary>
/// <param name="Page">The desired page</param>
/// <param name="Type">The type of Item</param>
public sealed record GetItemsRequest(
    int Page,
    [property: AliasAs("type")]
    ItemType? Type = null
) : PaginatedRequest(Page);