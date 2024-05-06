using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Mapping.Exceptions;

/// <summary>
/// Thrown when a string value for a position from the MLB API cannot be mapped to this domain's <see cref="Position"/>
/// </summary>
public sealed class InvalidMlbApiPositionException : Exception
{
    public InvalidMlbApiPositionException(string? message) : base(message)
    {
    }
}