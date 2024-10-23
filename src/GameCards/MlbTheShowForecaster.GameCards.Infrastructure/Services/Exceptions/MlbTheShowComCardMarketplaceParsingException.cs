namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services.Exceptions;

/// <summary>
/// Thrown when <see cref="MlbTheShowComCardMarketplace"/> cannot parse the required information
/// </summary>
public sealed class MlbTheShowComCardMarketplaceParsingException : Exception
{
    public MlbTheShowComCardMarketplaceParsingException(string? message) : base(message)
    {
    }
}