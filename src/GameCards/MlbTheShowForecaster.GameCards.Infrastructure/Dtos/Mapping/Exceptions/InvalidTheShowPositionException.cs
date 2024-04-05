using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;

/// <summary>
/// Thrown when a string value for a position from MLB The Show cannot be mapped to this domain's <see cref="Position"/>
/// </summary>
public sealed class InvalidTheShowPositionException : Exception
{
    public InvalidTheShowPositionException(string? message) : base(message)
    {
    }
}