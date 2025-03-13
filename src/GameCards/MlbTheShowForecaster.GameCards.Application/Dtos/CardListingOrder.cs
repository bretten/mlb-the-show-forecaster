using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

/// <summary>
/// Represents an order for a <see cref="CardListing"/>
/// </summary>
/// <param name="Date">The order date</param>
/// <param name="Price">The order price</param>
/// <param name="SequenceNumber">The 0-indexed sequence number, which will increment for orders with the same date and price</param>
public readonly record struct CardListingOrder(DateTime Date, NaturalNumber Price, NaturalNumber SequenceNumber);