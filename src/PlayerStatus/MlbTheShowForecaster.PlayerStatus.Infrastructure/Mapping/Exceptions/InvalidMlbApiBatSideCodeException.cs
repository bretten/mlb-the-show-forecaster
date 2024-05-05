using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Mapping.Exceptions;

/// <summary>
/// Thrown if the bat side code from the MLB API is invalid when mapping to this domain's <see cref="BatSide"/>
/// </summary>
public sealed class InvalidMlbApiBatSideCodeException : Exception
{
    public InvalidMlbApiBatSideCodeException(string? message) : base(message)
    {
    }
}