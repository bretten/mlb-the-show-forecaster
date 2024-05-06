using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Mapping.Exceptions;

/// <summary>
/// Thrown if the throw arm code from the MLB API is invalid when mapping to this domain's <see cref="ThrowArm"/>
/// </summary>
public sealed class InvalidMlbApiThrowArmCodeException : Exception
{
    public InvalidMlbApiThrowArmCodeException(string? message) : base(message)
    {
    }
}