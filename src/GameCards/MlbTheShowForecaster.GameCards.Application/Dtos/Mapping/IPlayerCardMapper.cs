using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Mapping;

/// <summary>
/// Defines a mapper that maps the Application-level player card to the Domain's <see cref="PlayerCard"/>
/// </summary>
public interface IPlayerCardMapper
{
    /// <summary>
    /// Should map <see cref="MlbPlayerCard"/> to <see cref="PlayerCard"/>
    /// </summary>
    /// <param name="card">The player card to map</param>
    /// <returns><see cref="PlayerCard"/></returns>
    PlayerCard Map(MlbPlayerCard card);
}