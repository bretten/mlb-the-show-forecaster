using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.Entities;

public class PlayerCardTests
{
    [Fact]
    public void ChangePlayerRating_HasExistingRatingOnSameDate_ThrowsException()
    {
        // Arrange
        var card = Faker.FakePlayerCard();
        var date = new DateOnly(2024, 4, 1);
        var newOverallRating = Faker.FakeOverallRating(49);
        ;
        var newPlayerAttributes = Faker.FakePlayerCardAttributes(scalar: 2);
        card.ChangePlayerRating(date, newOverallRating, newPlayerAttributes);
        var action = () => card.ChangePlayerRating(date.AddDays(-1), newOverallRating, newPlayerAttributes);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<PlayerCardHistoricalRatingExistsException>(actual);
    }

    [Fact]
    public void ChangePlayerRating_HasNoPreviousRating_AddsRatingToHistoricalCollection()
    {
        // Arrange
        var card = Faker.FakePlayerCard();
        var date = new DateOnly(2024, 4, 2);
        var newOverallRating = Faker.FakeOverallRating(49);

        var newPlayerAttributes = Faker.FakePlayerCardAttributes(scalar: 2);
        var expected =
            Faker.FakePlayerCardHistoricalRating(DateOnly.MinValue, date, newOverallRating, newPlayerAttributes);

        // Act
        card.ChangePlayerRating(date, newOverallRating, newPlayerAttributes);

        // Assert
        Assert.Equal(expected, card.HistoricalRatingsChronologically[0]);
    }

    [Fact]
    public void ChangePlayerRating_HasPreviousRatings_AddsRatingToHistoricalCollection()
    {
        // Arrange
        var card = Faker.FakePlayerCard();
        var date1 = new DateOnly(2024, 4, 1);
        card.ChangePlayerRating(date1, Faker.FakeOverallRating(), Faker.FakePlayerCardAttributes());

        var date2 = new DateOnly(2024, 4, 2);
        var newOverallRating = Faker.FakeOverallRating(49);

        var newPlayerAttributes = Faker.FakePlayerCardAttributes(scalar: 2);

        var expected1 = Faker.FakePlayerCardHistoricalRating(DateOnly.MinValue, date1, Faker.FakeOverallRating(),
            Faker.FakePlayerCardAttributes());
        var expected2 = Faker.FakePlayerCardHistoricalRating(date1, date2, newOverallRating, newPlayerAttributes);

        // Act
        card.ChangePlayerRating(date2, newOverallRating, newPlayerAttributes);

        // Assert
        Assert.Equal(expected1, card.HistoricalRatingsChronologically[0]);
        Assert.Equal(expected2, card.HistoricalRatingsChronologically[1]);
    }

    [Fact]
    public void ChangeTeam_NewTeam_ChangesTeam()
    {
        // Arrange
        var newTeam = Faker.FakeTeamShortName("DOT");
        var card = Faker.FakePlayerCard();

        // Act
        card.ChangeTeam(newTeam);

        // Assert
        Assert.Equal("DOT", card.TeamShortName.Value);
    }

    [Fact]
    public void Create_ValidValues_ReturnsPlayerCard()
    {
        // Arrange
        var cardId = Faker.FakeCardId("id1");
        const CardType cardType = CardType.MlbCard;
        var cardImage = Faker.FakeCardImage("img.png");
        const string cardName = "cardName";
        const Rarity rarity = Rarity.Silver;
        const CardSeries cardSeries = CardSeries.Live;
        var teamShortName = Faker.FakeTeamShortName("DOT");
        var overallRating = Faker.FakeOverallRating(80);
        var playerAttributes = Faker.FakePlayerCardAttributes(2);

        // Act
        var actual = PlayerCard.Create(cardId, cardType, cardImage, cardName, rarity, cardSeries, teamShortName,
            overallRating, playerAttributes);

        // Assert
        Assert.Equal("id1", actual.TheShowId.Value);
        Assert.Equal(CardType.MlbCard, actual.Type);
        Assert.Equal("img.png", actual.Image.Value);
        Assert.Equal("cardName", actual.Name);
        Assert.Equal(Rarity.Silver, actual.Rarity);
        Assert.Equal(CardSeries.Live, actual.Series);
        Assert.Equal("DOT", actual.TeamShortName.Value);
        Assert.Equal(80, actual.OverallRating.Value);
        Assert.Equal(Faker.FakePlayerCardAttributes(2), actual.PlayerCardAttributes);
    }
}