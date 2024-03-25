namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.Exceptions;

/// <summary>
/// Thrown when <see cref="AbilityAttribute"/> is provided a value outside of the expected range
/// </summary>
public sealed class AbilityAttributeOutOfRangeException : Exception
{
    public AbilityAttributeOutOfRangeException(string? message) : base(message)
    {
    }
}