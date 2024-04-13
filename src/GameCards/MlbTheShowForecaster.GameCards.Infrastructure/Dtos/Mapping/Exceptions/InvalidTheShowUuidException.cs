using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;

/// <summary>
/// Thrown when trying to map a <see cref="UuidDto"/> to the application-level DTOs but it is not a valid UUID
/// </summary>
public sealed class InvalidTheShowUuidException : Exception
{
    public InvalidTheShowUuidException(string? message) : base(message)
    {
    }
}