using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard.Exceptions;

/// <summary>
/// Thrown when <see cref="UpdatePlayerCardCommandHandler"/> cannot find the specified <see cref="PlayerCard"/>
/// to update
/// </summary>
public sealed class PlayerCardNotFoundException : Exception
{
    public PlayerCardNotFoundException(string? message) : base(message)
    {
    }
}