using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;

/// <summary>
/// Thrown when a mapper is trying to map a MLB The Show <see cref="ItemDto"/> but the item is of an unexpected type
/// </summary>
public sealed class UnexpectedTheShowItemException : Exception
{
    public UnexpectedTheShowItemException(string? message) : base(message)
    {
    }
}