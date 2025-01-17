using System.Text.Json.Serialization;

namespace com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Listings;

/// <summary>
/// Represents a purchase/sale of a listing
/// </summary>
/// <param name="Date">The date and time of the order (API uses UTC)</param>
/// <param name="Price">The price of the order</param>
public record ListingOrderDto(
    [property: JsonPropertyName("date")] string Date,
    [property: JsonPropertyName("price")] string Price
);