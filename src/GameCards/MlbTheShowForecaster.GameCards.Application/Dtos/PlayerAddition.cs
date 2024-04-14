using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;

/// <summary>
/// Represents a player card being added to MLB The Show
/// </summary>
public readonly record struct PlayerAddition
{
    /// <summary>
    /// The ID of the player card
    /// </summary>
    private readonly CardExternalId _cardExternalId;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="cardExternalId">The ID of the player card</param>
    /// <param name="playerName">The name of the player</param>
    public PlayerAddition(CardExternalId cardExternalId, string playerName)
    {
        _cardExternalId = cardExternalId;
        PlayerName = playerName;
    }

    /// <summary>
    /// The ID of the player card
    /// </summary>
    /// <exception cref="EmptyPlayerAdditionCardExternalIdException">Thrown if the ID is empty</exception>
    public CardExternalId CardExternalId
    {
        get
        {
            if (_cardExternalId.Value == Guid.Empty)
            {
                throw new EmptyPlayerAdditionCardExternalIdException(
                    $"No card external ID for player addition: {PlayerName}");
            }

            return _cardExternalId;
        }
    }

    /// <summary>
    /// The name of the player
    /// </summary>
    public string PlayerName { get; }
};