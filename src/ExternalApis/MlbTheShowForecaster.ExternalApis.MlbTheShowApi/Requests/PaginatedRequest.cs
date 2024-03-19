using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests;

/// <summary>
/// Defines a paginated request
/// </summary>
/// <param name="Page">The desired page</param>
public abstract record PaginatedRequest(
    [property: JsonPropertyName("page")]
    int Page
);