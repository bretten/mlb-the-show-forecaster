namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Items;

/// <summary>
/// Request for getting MLB card Items
/// </summary>
/// <param name="Page">The desired page</param>
public sealed record GetMlbCardItemsRequest(
    int Page
) : PaginatedRequest(Page);