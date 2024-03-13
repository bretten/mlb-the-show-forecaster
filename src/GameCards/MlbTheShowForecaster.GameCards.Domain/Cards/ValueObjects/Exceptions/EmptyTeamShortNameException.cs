namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

/// <summary>
/// Thrown when <see cref="TeamShortName"/> is provided an empty value
/// </summary>
public sealed class EmptyTeamShortNameException : Exception
{
    public EmptyTeamShortNameException(string? message) : base(message)
    {
    }
}