using Refit;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Items;

/// <summary>
/// Request for getting an Item
/// </summary>
/// <param name="Uuid">The ID of the Item</param>
public sealed record GetItemRequest(
    [property: AliasAs("uuid")]
    string Uuid
);