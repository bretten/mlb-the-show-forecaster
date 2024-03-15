using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Cards.Entities;

public class PlayerCardTests
{
    [Fact]
    public void ChangePlayerRating_HasExistingRatingOnSameDate_ThrowsException()
    {
        // Arrange
        var card = Faker.FakePlayerCard();

        var newOverallRating = Faker.FakeOverallRating(49);
        var newPlayerAttributes = Faker.FakePlayerCardAttributes(scalar: 2);

        var date = new DateOnly(2024, 4, 1);
        card.ChangePlayerRating(date, newOverallRating, newPlayerAttributes); // Set up the historical rating
        var action = () => card.ChangePlayerRating(date.AddDays(-1), newOverallRating, newPlayerAttributes); // Repeat

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<PlayerCardHistoricalRatingExistsException>(actual);
    }

    [Fact]
    public void ChangePlayerRating_HasNoPreviousHistoricalRating_AddsCurrentRatingToHistoricalCollection()
    {
        // Arrange
        var currentOverallRating = Faker.FakeOverallRating(48);
        var currentPlayerCardAttributes = Faker.FakePlayerCardAttributes(scalar: 1);
        var card = Faker.FakePlayerCard(overallRating: currentOverallRating,
            playerCardAttributes: currentPlayerCardAttributes);

        var newOverallRating = Faker.FakeOverallRating(49);
        var newPlayerAttributes = Faker.FakePlayerCardAttributes(scalar: 2);

        var date = new DateOnly(2024, 4, 2);
        var expectedHistoricalRating = Faker.FakePlayerCardHistoricalRating(DateOnly.MinValue, date,
            currentOverallRating, currentPlayerCardAttributes);

        // Act
        card.ChangePlayerRating(date, newOverallRating, newPlayerAttributes);

        // Assert
        Assert.Equal(49, card.OverallRating.Value);
        Assert.Equal(newPlayerAttributes, card.PlayerCardAttributes);
        Assert.Equal(expectedHistoricalRating, card.HistoricalRatingsChronologically[0]);
    }

    [Fact]
    public void ChangePlayerRating_HasPreviousHistoricalRatings_AddsCurrentRatingToHistoricalCollection()
    {
        // Arrange
        var previousOverallRating = Faker.FakeOverallRating(48);
        var previousPlayerCardAttributes = Faker.FakePlayerCardAttributes(scalar: 1);
        var card = Faker.FakePlayerCard(overallRating: previousOverallRating,
            playerCardAttributes: previousPlayerCardAttributes);

        var currentOverallRating = Faker.FakeOverallRating(49);
        var currentPlayerCardAttributes = Faker.FakePlayerCardAttributes(scalar: 2);
        var date1 = new DateOnly(2024, 4, 1);
        card.ChangePlayerRating(date1, currentOverallRating, currentPlayerCardAttributes);

        var newOverallRating = Faker.FakeOverallRating(51);
        var newPlayerAttributes = Faker.FakePlayerCardAttributes(scalar: 3);
        var date2 = new DateOnly(2024, 4, 2);

        var expected1 = Faker.FakePlayerCardHistoricalRating(DateOnly.MinValue, date1, previousOverallRating,
            previousPlayerCardAttributes);
        var expected2 =
            Faker.FakePlayerCardHistoricalRating(date1, date2, currentOverallRating, currentPlayerCardAttributes);

        // Act
        card.ChangePlayerRating(date2, newOverallRating, newPlayerAttributes);

        // Assert
        Assert.Equal(51, card.OverallRating.Value);
        Assert.Equal(newPlayerAttributes, card.PlayerCardAttributes);
        Assert.Equal(expected1, card.HistoricalRatingsChronologically[0]);
        Assert.Equal(expected2, card.HistoricalRatingsChronologically[1]);
    }

    [Fact]
    public void ChangePlayerRating_HigherOverallRating_RaisesOverallRatingImprovedDomainEvent()
    {
        // Arrange
        var currentOverallRating = Faker.FakeOverallRating(48);
        var currentPlayerCardAttributes = Faker.FakePlayerCardAttributes();
        var card = Faker.FakePlayerCard(overallRating: currentOverallRating,
            playerCardAttributes: currentPlayerCardAttributes);

        var newOverallRating = Faker.FakeOverallRating(49);
        var newPlayerAttributes = Faker.FakePlayerCardAttributes();

        // Act
        card.ChangePlayerRating(new DateOnly(2024, 4, 1), newOverallRating, newPlayerAttributes);

        // Assert
        Assert.Single(card.DomainEvents);
        Assert.IsType<PlayerCardOverallRatingImprovedEvent>(card.DomainEvents[0]);
        var e = card.DomainEvents[0] as PlayerCardOverallRatingImprovedEvent;
        Assert.Equal(card.TheShowId, e!.CardId);
        Assert.Equal(48, e.PreviousOverallRating.Value);
        Assert.Equal(currentPlayerCardAttributes, e.PreviousPlayerCardAttributes);
        Assert.Equal(49, e.NewOverallRating.Value);
        Assert.Equal(newPlayerAttributes, e.NewPlayerCardAttributes);
    }

    [Fact]
    public void ChangePlayerRating_LowerOverallRating_RaisesOverallRatingDeclinedDomainEvent()
    {
        // Arrange
        var currentOverallRating = Faker.FakeOverallRating(48);
        var currentPlayerCardAttributes = Faker.FakePlayerCardAttributes();
        var card = Faker.FakePlayerCard(overallRating: currentOverallRating,
            playerCardAttributes: currentPlayerCardAttributes);

        var newOverallRating = Faker.FakeOverallRating(47);
        var newPlayerAttributes = Faker.FakePlayerCardAttributes();

        // Act
        card.ChangePlayerRating(new DateOnly(2024, 4, 1), newOverallRating, newPlayerAttributes);

        // Assert
        Assert.Single(card.DomainEvents);
        Assert.IsType<PlayerCardOverallRatingDeclinedEvent>(card.DomainEvents[0]);
        var e = card.DomainEvents[0] as PlayerCardOverallRatingDeclinedEvent;
        Assert.Equal(card.TheShowId, e!.CardId);
        Assert.Equal(48, e.PreviousOverallRating.Value);
        Assert.Equal(currentPlayerCardAttributes, e.PreviousPlayerCardAttributes);
        Assert.Equal(47, e.NewOverallRating.Value);
        Assert.Equal(newPlayerAttributes, e.NewPlayerCardAttributes);
    }

    [Fact]
    public void ChangePlayerRating_SameOverallRating_NoDomainEventRaised()
    {
        // Arrange
        var currentOverallRating = Faker.FakeOverallRating(48);
        var currentPlayerCardAttributes = Faker.FakePlayerCardAttributes(scalar: 1);
        var card = Faker.FakePlayerCard(overallRating: currentOverallRating,
            playerCardAttributes: currentPlayerCardAttributes);

        var newOverallRating = Faker.FakeOverallRating(48);
        var newPlayerAttributes = Faker.FakePlayerCardAttributes(scalar: 2);

        // Act
        card.ChangePlayerRating(new DateOnly(2024, 4, 1), newOverallRating, newPlayerAttributes);

        // Assert
        Assert.Empty(card.DomainEvents);
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