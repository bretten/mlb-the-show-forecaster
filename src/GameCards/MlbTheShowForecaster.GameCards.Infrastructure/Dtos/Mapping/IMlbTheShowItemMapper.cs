using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;

/// <summary>
/// Defines a mapper that maps <see cref="ItemDto"/>s from MLB The Show to application-level DTOs
/// </summary>
public interface IMlbTheShowItemMapper
{
    /// <summary>
    /// Should map a MLB The Show <see cref="ItemDto"/> to an application <see cref="MlbPlayerCard"/>
    /// </summary>
    /// <param name="year">The year of the player card</param>
    /// <param name="item">The <see cref="ItemDto"/> to map</param>
    /// <returns><see cref="MlbPlayerCard"/></returns>
    /// <exception cref="UnexpectedTheShowItemException">Thrown when the <see cref="ItemDto"/> is not of a type that can be mapped to <see cref="MlbPlayerCard"/></exception>
    MlbPlayerCard Map(SeasonYear year, ItemDto item);

    /// <summary>
    /// Should map a rarity value from MLB The Show to <see cref="Rarity"/>
    /// </summary>
    /// <param name="rarity">The rarity string value from MLB The Show</param>
    /// <returns><see cref="Rarity"/></returns>
    /// <exception cref="InvalidTheShowRarityException">Thrown if the rarity string value is invalid</exception>
    Rarity MapRarity(string rarity);

    /// <summary>
    /// Should map a card series value from MLB The Show to <see cref="CardSeries"/>
    /// </summary>
    /// <param name="cardSeries">The card series string value from MLB The Show</param>
    /// <returns><see cref="CardSeries"/></returns>
    /// <exception cref="InvalidTheShowCardSeriesException">Thrown if the card series string value is invalid</exception>
    CardSeries MapCardSeries(string cardSeries);

    /// <summary>
    /// Should map a position value from MLB The Show to <see cref="Position"/>
    /// </summary>
    /// <param name="position">The position value from MLB The Show</param>
    /// <returns><see cref="Position"/></returns>
    /// <exception cref="InvalidTheShowPositionException">Thrown if the position string value is invalid</exception>
    Position MapPosition(string position);
}