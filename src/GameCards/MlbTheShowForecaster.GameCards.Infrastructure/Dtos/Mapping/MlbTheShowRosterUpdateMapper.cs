using System.Collections.Immutable;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.RosterUpdates;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.RosterUpdates;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;

/// <summary>
/// Mapper that maps roster updates from MLB The Show to application-level DTOs
/// </summary>
public sealed class MlbTheShowRosterUpdateMapper : IMlbTheShowRosterUpdateMapper
{
    /// <summary>
    /// Mapper that maps <see cref="ItemDto"/>s from MLB The Show to application-level DTOs
    /// </summary>
    private readonly IMlbTheShowItemMapper _itemMapper;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="itemMapper">Mapper that maps <see cref="ItemDto"/>s from MLB The Show to application-level DTOs</param>
    public MlbTheShowRosterUpdateMapper(IMlbTheShowItemMapper itemMapper)
    {
        _itemMapper = itemMapper;
    }

    /// <summary>
    /// Maps <see cref="RosterUpdateDto"/> and <see cref="GetRosterUpdateResponse"/> to <see cref="RosterUpdate"/>
    /// </summary>
    /// <param name="rosterUpdate">The roster update information</param>
    /// <param name="rosterUpdateChanges">The player changes in the roster update</param>
    /// <returns><see cref="RosterUpdate"/></returns>
    public RosterUpdate Map(RosterUpdateDto rosterUpdate, GetRosterUpdateResponse rosterUpdateChanges)
    {
        return new RosterUpdate(
            rosterUpdate.Date,
            rosterUpdateChanges.PlayerAttributeChanges.Select(x => Map(rosterUpdate.Date, x)).ToImmutableList(),
            rosterUpdateChanges.PlayerPositionChanges.Select(Map).ToImmutableList(),
            rosterUpdateChanges.NewlyAddedPlayers.Select(Map).ToImmutableList()
        );
    }

    /// <summary>
    /// Maps <see cref="PlayerAttributeChangeDto"/> to <see cref="PlayerRatingChange"/>
    /// </summary>
    /// <param name="date">The date of the change</param>
    /// <param name="playerAttributeChange">The attribute changes for the player</param>
    /// <returns><see cref="PlayerRatingChange"/></returns>
    public PlayerRatingChange Map(DateOnly date, PlayerAttributeChangeDto playerAttributeChange)
    {
        return new PlayerRatingChange(
            date,
            CardExternalId.Create(playerAttributeChange.Uuid.Value ??
                                  throw new InvalidTheShowUuidException(
                                      $"Could not map the {nameof(PlayerAttributeChangeDto)}'s UUID since it is not valid: ${playerAttributeChange.Uuid.RawValue}")),
            NewRating: OverallRating.Create(playerAttributeChange.CurrentRank),
            NewRarity: _itemMapper.MapRarity(playerAttributeChange.CurrentRarity),
            OldRating: OverallRating.Create(playerAttributeChange.OldRank),
            OldRarity: _itemMapper.MapRarity(playerAttributeChange.OldRarity),
            Map(playerAttributeChange.Changes.ToImmutableList())
        );
    }

    /// <summary>
    /// Maps a collection of <see cref="AttributeChangeDto"/> to <see cref="MlbPlayerAttributeChanges"/>
    /// </summary>
    /// <param name="attributeChanges">The individual attribute changes</param>
    /// <returns><see cref="MlbPlayerAttributeChanges"/></returns>
    public MlbPlayerAttributeChanges Map(IEnumerable<AttributeChangeDto> attributeChanges)
    {
        var allAttributeChanges = new MlbPlayerAttributeChanges();

        foreach (var attributeChange in attributeChanges)
        {
            allAttributeChanges = AppendAttributeChanges(allAttributeChanges, attributeChange);
        }

        return allAttributeChanges;
    }

    /// <summary>
    /// Maps <see cref="PlayerPositionChangeDto"/> to <see cref="PlayerPositionChange"/>
    /// </summary>
    /// <param name="positionChange">The position change</param>
    /// <returns><see cref="PlayerPositionChange"/></returns>
    public PlayerPositionChange Map(PlayerPositionChangeDto positionChange)
    {
        return new PlayerPositionChange(
            CardExternalId: CardExternalId.Create(positionChange.Uuid.Value ?? throw new InvalidTheShowUuidException(
                $"Could not map the {nameof(PlayerPositionChangeDto)}'s UUID since it is not valid: ${positionChange.Uuid.RawValue}")),
            NewPosition: _itemMapper.MapPosition(positionChange.Position)
        );
    }

    /// <summary>
    /// Maps <see cref="NewlyAddedPlayerDto"/> to <see cref="PlayerAddition"/>
    /// </summary>
    /// <param name="newPlayer">The new player</param>
    /// <returns><see cref="PlayerAddition"/></returns>
    public PlayerAddition Map(NewlyAddedPlayerDto newPlayer)
    {
        return new PlayerAddition(
            cardExternalId: CardExternalId.Create(newPlayer.Uuid.Value ?? Guid.Empty),
            playerName: newPlayer.Name
        );
    }

    /// <summary>
    /// Appends an individual MLB The Show attribute change (<see cref="AttributeChangeDto"/>) onto the
    /// application-level DTO that represents all attribute changes (<see cref="MlbPlayerAttributeChanges"/>)
    /// </summary>
    /// <param name="changes">The full list of attribute changes to append to</param>
    /// <param name="attributeChangeDto">The individual attribute change to append</param>
    /// <returns><see cref="MlbPlayerAttributeChanges"/></returns>
    private MlbPlayerAttributeChanges AppendAttributeChanges(MlbPlayerAttributeChanges changes,
        AttributeChangeDto attributeChangeDto)
    {
        var amount = Map(attributeChangeDto);
        switch (attributeChangeDto.Name)
        {
            case "ACC":
            case "ARM ACC": // Internal abbreviation
                changes = changes with { ArmAccuracy = amount.Delta };
                break;
            case "ARM":
            case "ARM STR": // Internal abbreviation
                changes = changes with { ArmStrength = amount.Delta };
                break;
            case "BB/9":
                changes = changes with { BbPerBf = amount.Delta };
                break;
            case "BLK":
                changes = changes with { Blocking = amount.Delta };
                break;
            case "BNT":
                changes = changes with { BuntingAbility = amount.Delta };
                break;
            case "BRK":
                changes = changes with { PitchMovement = amount.Delta };
                break;
            case "BR AGG":
                changes = changes with { BaseRunningAggression = amount.Delta };
                break;
            case "CLT":
            case "CLU": // Internal abbreviation
                changes = changes with { BattingClutch = amount.Delta };
                break;
            case "CON L":
                changes = changes with { ContactLeft = amount.Delta };
                break;
            case "CON R":
                changes = changes with { ContactRight = amount.Delta };
                break;
            case "CTRL":
                changes = changes with { PitchControl = amount.Delta };
                break;
            case "DISC":
                changes = changes with { PlateDiscipline = amount.Delta };
                break;
            case "DRG BNT":
                changes = changes with { DragBuntingAbility = amount.Delta };
                break;
            case "DUR":
                changes = changes with
                {
                    HittingDurability = amount.Delta,
                    FieldingDurability = amount.Delta
                };
                break;
            case "FLD":
                changes = changes with { FieldingAbility = amount.Delta };
                break;
            case "H/9":
                changes = changes with { HitsPerBf = amount.Delta };
                break;
            case "HR/9":
                changes = changes with { HrPerBf = amount.Delta };
                break;
            case "K/9":
                changes = changes with { KPerBf = amount.Delta };
                break;
            case "PCLT":
            case "PCLU": // Internal abbreviation
                changes = changes with { PitchingClutch = amount.Delta };
                break;
            case "POW L":
            case "PWR L": // Internal abbreviation
                changes = changes with { PowerLeft = amount.Delta };
                break;
            case "POW R":
            case "PWR R": // Internal abbreviation
                changes = changes with { PowerRight = amount.Delta };
                break;
            case "REAC":
                changes = changes with { ReactionTime = amount.Delta };
                break;
            case "SPD":
                changes = changes with { Speed = amount.Delta };
                break;
            case "STA":
                changes = changes with { Stamina = amount.Delta };
                break;
            case "STEAL":
            case "STL": // Internal abbreviation
                changes = changes with { BaseRunningAbility = amount.Delta };
                break;
            case "VEL":
                changes = changes with { PitchVelocity = amount.Delta };
                break;
            case "VIS":
                changes = changes with { PlateVision = amount.Delta };
                break;
        }

        return changes;
    }

    /// <summary>
    /// Parses the string attribute amounts in <see cref="AttributeChangeDto"/> to numerical values
    /// </summary>
    /// <param name="attributeChange">The attribute change with string amounts</param>
    /// <returns><see cref="AttributeChangeAmount"/></returns>
    /// <exception cref="InvalidTheShowAttributeAmountException">Thrown when the attribute amount string is not a valid numerical value</exception>
    private AttributeChangeAmount Map(AttributeChangeDto attributeChange)
    {
        var wasCurrentValueParsed = int.TryParse(attributeChange.CurrentValue, out var currentValue);
        var wasDeltaParsed = int.TryParse(attributeChange.Delta, out var delta);
        if (!wasCurrentValueParsed || !wasDeltaParsed)
        {
            throw new InvalidTheShowAttributeAmountException(
                $"Invalid attribute amount. Current value = {attributeChange.CurrentValue}, Delta = {attributeChange.Delta}");
        }

        return new AttributeChangeAmount(CurrentValue: currentValue, Delta: delta);
    }

    /// <summary>
    /// Represents attribute change amounts from <see cref="AttributeChangeDto"/> in numerical form
    /// </summary>
    private sealed record AttributeChangeAmount(int CurrentValue, int Delta)
    {
        /// <summary>
        /// The current value of the attribute
        /// </summary>
        public int CurrentValue { get; } = CurrentValue;

        /// <summary>
        /// The amount the attribute changed by
        /// </summary>
        public int Delta { get; } = Delta;
    }
}