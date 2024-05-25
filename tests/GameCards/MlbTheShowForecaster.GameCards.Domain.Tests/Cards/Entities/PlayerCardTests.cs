using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
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
        var expectedHistoricalRating = Faker.FakePlayerCardHistoricalRating(new DateOnly(2024, 1, 1), date,
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

        var expected1 = Faker.FakePlayerCardHistoricalRating(new DateOnly(2024, 1, 1), date1, previousOverallRating,
            previousPlayerCardAttributes);
        var expected2 =
            Faker.FakePlayerCardHistoricalRating(date1, date2, currentOverallRating, currentPlayerCardAttributes);

        // Act
        card.ChangePlayerRating(date2, newOverallRating, newPlayerAttributes);

        // Assert
        Assert.Equal(51, card.OverallRating.Value);
        Assert.Equal(newPlayerAttributes, card.PlayerCardAttributes);
        Assert.Equal(2, card.HistoricalRatingsChronologically.Count);
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

        var newOverallRating = Faker.FakeOverallRating(70);
        var newPlayerAttributes = Faker.FakePlayerCardAttributes();

        // Act
        card.ChangePlayerRating(new DateOnly(2024, 4, 1), newOverallRating, newPlayerAttributes);

        // Assert
        Assert.Single(card.DomainEvents);
        Assert.IsType<PlayerCardOverallRatingImprovedEvent>(card.DomainEvents[0]);
        var e = card.DomainEvents[0] as PlayerCardOverallRatingImprovedEvent;
        Assert.Equal(card.ExternalId, e!.CardExternalId);
        Assert.Equal(48, e.PreviousOverallRating.Value);
        Assert.Equal(currentPlayerCardAttributes, e.PreviousPlayerCardAttributes);
        Assert.Equal(70, e.NewOverallRating.Value);
        Assert.Equal(newPlayerAttributes, e.NewPlayerCardAttributes);
        Assert.True(e.RarityChanged);
        Assert.Equal(newOverallRating.Rarity, card.Rarity);
    }

    [Fact]
    public void ChangePlayerRating_LowerOverallRating_RaisesOverallRatingDeclinedDomainEvent()
    {
        // Arrange
        var currentOverallRating = Faker.FakeOverallRating(48);
        var currentPlayerCardAttributes = Faker.FakePlayerCardAttributes();
        var card = Faker.FakePlayerCard(overallRating: currentOverallRating,
            playerCardAttributes: currentPlayerCardAttributes, rarity: Rarity.Common);

        var newOverallRating = Faker.FakeOverallRating(47);
        var newPlayerAttributes = Faker.FakePlayerCardAttributes();

        // Act
        card.ChangePlayerRating(new DateOnly(2024, 4, 1), newOverallRating, newPlayerAttributes);

        // Assert
        Assert.Single(card.DomainEvents);
        Assert.IsType<PlayerCardOverallRatingDeclinedEvent>(card.DomainEvents[0]);
        var e = card.DomainEvents[0] as PlayerCardOverallRatingDeclinedEvent;
        Assert.Equal(card.ExternalId, e!.CardExternalId);
        Assert.Equal(48, e.PreviousOverallRating.Value);
        Assert.Equal(currentPlayerCardAttributes, e.PreviousPlayerCardAttributes);
        Assert.Equal(47, e.NewOverallRating.Value);
        Assert.Equal(newPlayerAttributes, e.NewPlayerCardAttributes);
        Assert.False(e.RarityChanged);
        Assert.Equal(newOverallRating.Rarity, card.Rarity);
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
    public void SetTemporaryRating_HigherOverallRating_SetsTempRatingAndAddsToHistoryAndRaisesImprovementEvent()
    {
        // Arrange
        var card = Faker.FakePlayerCard(overallRating: Faker.FakeOverallRating(48));
        var tempOverallRating = Faker.FakeOverallRating(70);
        var date = new DateOnly(2024, 5, 24);

        // Act
        card.SetTemporaryRating(date, tempOverallRating);

        /*
         * Assert
         */
        // OverallRating remains unchanged, TemporaryOverallRating changed
        Assert.Equal(48, card.OverallRating.Value);
        Assert.NotNull(card.TemporaryOverallRating);
        Assert.Equal(70, card.TemporaryOverallRating.Value);

        // Temporary rating added to history
        Assert.Single(card.HistoricalRatingsChronologically);
        var expectedRating = Faker.FakePlayerCardHistoricalRating(date, date, OverallRating.Create(70));
        Assert.Equal(expectedRating, card.HistoricalRatingsChronologically[0]);

        // A temporary improvement event should have been raised
        Assert.Single(card.DomainEvents);
        Assert.IsType<PlayerCardOverallRatingTemporarilyImprovedEvent>(card.DomainEvents[0]);
        var e = card.DomainEvents[0] as PlayerCardOverallRatingTemporarilyImprovedEvent;
        Assert.Equal(card.ExternalId, e!.CardExternalId);
        Assert.Equal(48, e.PreviousOverallRating.Value);
        Assert.Equal(70, e.NewOverallRating.Value);
    }

    [Fact]
    public void SetTemporaryRating_LowerOverallRating_SetsTempRatingAndAddsToHistoryAndRaisesDeclineEvent()
    {
        // Arrange
        var card = Faker.FakePlayerCard(overallRating: Faker.FakeOverallRating(90));
        var tempOverallRating = Faker.FakeOverallRating(80);
        var date = new DateOnly(2024, 5, 24);

        // Act
        card.SetTemporaryRating(date, tempOverallRating);

        /*
         * Assert
         */
        // OverallRating remains unchanged, TemporaryOverallRating changed
        Assert.Equal(90, card.OverallRating.Value);
        Assert.NotNull(card.TemporaryOverallRating);
        Assert.Equal(80, card.TemporaryOverallRating.Value);

        // Temporary rating added to history
        Assert.Single(card.HistoricalRatingsChronologically);
        var expectedRating = Faker.FakePlayerCardHistoricalRating(date, date, OverallRating.Create(80));
        Assert.Equal(expectedRating, card.HistoricalRatingsChronologically[0]);

        // A temporary decline event should have been raised
        Assert.Single(card.DomainEvents);
        Assert.IsType<PlayerCardOverallRatingTemporarilyDeclinedEvent>(card.DomainEvents[0]);
        var e = card.DomainEvents[0] as PlayerCardOverallRatingTemporarilyDeclinedEvent;
        Assert.Equal(card.ExternalId, e!.CardExternalId);
        Assert.Equal(90, e.PreviousOverallRating.Value);
        Assert.Equal(80, e.NewOverallRating.Value);
    }

    [Fact]
    public void RemoveTemporaryRating_HasTemporaryRating_TemporaryRatingRemoved()
    {
        // Arrange
        var card = Faker.FakePlayerCard(overallRating: Faker.FakeOverallRating(60));
        card.SetTemporaryRating(new DateOnly(2024, 5, 23), Faker.FakeOverallRating(80));

        var date = new DateOnly(2024, 5, 24);

        // Act
        card.RemoveTemporaryRating(date);

        // Assert
        Assert.Null(card.TemporaryOverallRating);

        var expectedRating = Faker.FakePlayerCardHistoricalRating(date, date, OverallRating.Create(60));
        Assert.Equal(expectedRating, card.HistoricalRatingsChronologically[1]);
    }

    [Fact]
    public void Boost_BoostedAttributes_BoostsRatingAndAddsToHistoryAndRaisesEvent()
    {
        // Arrange
        var card = Faker.FakePlayerCard(overallRating: Faker.FakeOverallRating(75));
        var boostedAttributes = Faker.FakePlayerCardAttributes(scalar: 2);
        var date = new DateOnly(2024, 5, 24);

        // Act
        card.Boost(date, boostedAttributes);

        /*
         * Assert
         */
        // Boosted flag
        Assert.True(card.IsBoosted);

        // OverallRating remains unchanged, TemporaryOverallRating changed
        Assert.Equal(75, card.OverallRating.Value);
        Assert.NotNull(card.TemporaryOverallRating);
        Assert.Equal(99, card.TemporaryOverallRating.Value);

        // Boost added to history
        Assert.Single(card.HistoricalRatingsChronologically);
        var expectedRating =
            Faker.FakePlayerCardHistoricalRating(date, date, OverallRating.Create(99), boostedAttributes);
        Assert.Equal(expectedRating, card.HistoricalRatingsChronologically[0]);

        // An boost event should have been raised
        Assert.Single(card.DomainEvents);
        Assert.IsType<PlayerCardBoostedEvent>(card.DomainEvents[0]);
        var e = card.DomainEvents[0] as PlayerCardBoostedEvent;
        Assert.Equal(card.ExternalId, e!.CardExternalId);
        Assert.Equal(99, e.NewOverallRating.Value);
        Assert.Equal(boostedAttributes, e.NewPlayerCardAttributes);
    }

    [Fact]
    public void RemoveBoost_BoostedCard_CardIsNoLongerBoosted()
    {
        // Arrange
        var card = Faker.FakePlayerCard(overallRating: OverallRating.Create(50));
        card.Boost(new DateOnly(2024, 5, 23), Faker.FakePlayerCardAttributes(scalar: 4));

        var date = new DateOnly(2024, 5, 24);
        var normalAttributes = Faker.FakePlayerCardAttributes(scalar: 2);

        // Act
        card.RemoveBoost(date, normalAttributes);

        /*
         * Assert
         */
        // Boosted flag
        Assert.False(card.IsBoosted);

        // Temporary rating is null
        Assert.Null(card.TemporaryOverallRating);

        // Attributes returned to normal
        Assert.Equal(normalAttributes, card.PlayerCardAttributes);

        // Added to history
        var expectedRating =
            Faker.FakePlayerCardHistoricalRating(date, date, OverallRating.Create(50), normalAttributes);
        Assert.Equal(expectedRating, card.HistoricalRatingsChronologically[1]);
    }

    [Fact]
    public void AddHistoricalRating_AlreadyExists_ThrowsException()
    {
        // Arrange
        var card = Faker.FakePlayerCard();

        var overallRating = Faker.FakeOverallRating(49);
        var playerAttributes = Faker.FakePlayerCardAttributes(scalar: 2);
        var startDate = new DateOnly(2024, 4, 1);
        var endDate = new DateOnly(2024, 4, 2);
        var historicalRating =
            Faker.FakePlayerCardHistoricalRating(startDate, endDate, overallRating, playerAttributes);

        card.AddHistoricalRating(historicalRating); // Add the historical rating
        var action = () => card.AddHistoricalRating(historicalRating); // Repeat

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<PlayerCardHistoricalRatingExistsException>(actual);
    }

    [Fact]
    public void AddHistoricalRating_NewPlayerCardHistoricalRating_AddsToCollectionOfPlayerCardHistoricalRatings()
    {
        // Arrange
        var card = Faker.FakePlayerCard();

        var overallRating = Faker.FakeOverallRating(49);
        var playerAttributes = Faker.FakePlayerCardAttributes(scalar: 2);
        var startDate = new DateOnly(2024, 4, 1);
        var endDate = new DateOnly(2024, 4, 2);
        var historicalRating =
            Faker.FakePlayerCardHistoricalRating(startDate, endDate, overallRating, playerAttributes);

        // Act
        card.AddHistoricalRating(historicalRating);

        // Assert
        Assert.Single(card.HistoricalRatingsChronologically);
        Assert.Equal(new DateOnly(2024, 4, 1), card.HistoricalRatingsChronologically[0].StartDate);
        Assert.Equal(new DateOnly(2024, 4, 2), card.HistoricalRatingsChronologically[0].EndDate);
        Assert.Equal(49, card.HistoricalRatingsChronologically[0].OverallRating.Value);
        Assert.Equal(playerAttributes, card.HistoricalRatingsChronologically[0].Attributes);
    }

    [Fact]
    public void ChangePosition_NewPosition_ChangesPositionAndRaisesPositionChangedEvent()
    {
        // Arrange
        const Position newPosition = Position.Catcher;
        var card = Faker.FakePlayerCard(position: Position.FirstBase);

        // Act
        card.ChangePosition(newPosition);

        // Assert
        Assert.Equal(Position.Catcher, card.Position);
        Assert.Single(card.DomainEvents);
        Assert.IsType<PlayerCardPositionChangedEvent>(card.DomainEvents[0]);
        var e = card.DomainEvents[0] as PlayerCardPositionChangedEvent;
        Assert.Equal(card.ExternalId, e!.CardExternalId);
        Assert.Equal(Position.Catcher, e.NewPosition);
        Assert.Equal(Position.FirstBase, e.OldPosition);
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
    public void IsRatingAppliedFor_DateOfHistoricalRating_ReturnsTrue()
    {
        // Arrange
        var startDate = new DateOnly(2024, 4, 1);
        var endDate = new DateOnly(2024, 4, 2);
        var card = Faker.FakePlayerCard();
        card.AddHistoricalRating(Faker.FakePlayerCardHistoricalRating(startDate, endDate));

        // Act
        var actual = card.IsRatingAppliedFor(new DateOnly(2024, 4, 1));

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void IsRatingAppliedFor_DateDoesNotMatchAnyHistoricalRatings_ReturnsFalse()
    {
        // Arrange
        var startDate = new DateOnly(2024, 4, 1);
        var endDate = new DateOnly(2024, 4, 2);
        var card = Faker.FakePlayerCard();
        card.AddHistoricalRating(Faker.FakePlayerCardHistoricalRating(startDate, endDate));

        // Act
        var actual = card.IsRatingAppliedFor(new DateOnly(2024, 5, 1));

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Create_ValidValues_ReturnsPlayerCard()
    {
        // Arrange
        var year = SeasonYear.Create(2024);
        var externalId = Faker.FakeCardExternalId(Faker.FakeGuid1);
        const CardType cardType = CardType.MlbCard;
        var cardImage = Faker.FakeCardImage("img.png");
        var cardName = Faker.FakeCardName("cardName");
        const Rarity rarity = Rarity.Silver;
        const CardSeries cardSeries = CardSeries.Live;
        const Position position = Position.CenterField;
        var teamShortName = Faker.FakeTeamShortName("DOT");
        var overallRating = Faker.FakeOverallRating(80);
        var playerAttributes = Faker.FakePlayerCardAttributes(2);

        // Act
        var actual = PlayerCard.Create(year, externalId, cardType, cardImage, cardName, rarity, cardSeries, position,
            teamShortName, overallRating, playerAttributes);

        // Assert
        Assert.Equal(2024, actual.Year.Value);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), actual.ExternalId.Value);
        Assert.Equal(CardType.MlbCard, actual.Type);
        Assert.Equal(new Uri("img.png", UriKind.Relative), actual.ImageLocation.Value);
        Assert.Equal("cardName", actual.Name.Value);
        Assert.Equal(Rarity.Silver, actual.Rarity);
        Assert.Equal(CardSeries.Live, actual.Series);
        Assert.Equal(Position.CenterField, actual.Position);
        Assert.Equal("DOT", actual.TeamShortName.Value);
        Assert.Equal(80, actual.OverallRating.Value);
        Assert.Equal(Faker.FakePlayerCardAttributes(2), actual.PlayerCardAttributes);
    }
}