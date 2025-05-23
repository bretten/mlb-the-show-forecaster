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
    public void BaselineHistoricalRatingsApplied_OneBaselineHistoricalRatingApplied_ReturnsOne()
    {
        // Arrange
        var card = Faker.FakePlayerCard();
        var historicalRating = Faker.FakeBaselinePlayerCardHistoricalRating();
        card.AddHistoricalRating(historicalRating);

        // Act
        var actual = card.BaselineHistoricalRatingsApplied;

        // Assert
        Assert.Equal(1, actual);
    }

    [Fact]
    public void BaselineHistoricalRatingsApplied_NoBaselineHistoricalRatingApplied_ReturnsZero()
    {
        // Arrange
        var card = Faker.FakePlayerCard();
        var tempHistoricalRating = Faker.FakeTemporaryPlayerCardHistoricalRating();
        var boostHistoricalRating = Faker.FakeBoostPlayerCardHistoricalRating();
        card.AddHistoricalRating(tempHistoricalRating);
        card.AddHistoricalRating(boostHistoricalRating);

        // Act
        var actual = card.BaselineHistoricalRatingsApplied;

        // Assert
        Assert.Equal(0, actual);
    }

    [Fact]
    public void TemporaryOverallRating_TempRatingChangeThatEnded_ReturnsNull()
    {
        // Arrange
        var card = Faker.FakePlayerCard(overallRating: 50);
        card.SetTemporaryRating(new DateOnly(2024, 5, 28), OverallRating.Create(90));
        card.RemoveTemporaryRating(new DateOnly(2024, 5, 29));

        // Act
        var actual = card.TemporaryOverallRating;

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void TemporaryOverallRating_TempRatingChange_ReturnsRecentTemporaryRating()
    {
        // Arrange
        var card = Faker.FakePlayerCard(overallRating: 50);
        card.SetTemporaryRating(new DateOnly(2024, 5, 28), OverallRating.Create(90));

        // Act
        var actual = card.TemporaryOverallRating;

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(90, actual.Value);
    }

    [Fact]
    public void TemporaryOverallRating_Boost_ReturnsRecentTemporaryRatingFromBoost()
    {
        // Arrange
        var card = Faker.FakePlayerCard(overallRating: 50);
        card.Boost(new DateOnly(2024, 5, 28), new DateOnly(2024, 5, 30), "Hit 5 HRs", Faker.FakePlayerCardAttributes());

        // Act
        var actual = card.TemporaryOverallRating;

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(99, actual.Value);
    }

    [Fact]
    public void TemporaryOverallRating_BaselineRatingChange_ReturnsNull()
    {
        // Arrange
        var card = Faker.FakePlayerCard(overallRating: 50);
        card.ChangePlayerRating(new DateOnly(2024, 5, 28), OverallRating.Create(70), Faker.FakePlayerCardAttributes());

        // Act
        var actual = card.TemporaryOverallRating;

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void IsBoosted_BoostThatHasNotEnded_ReturnsTrue()
    {
        // Arrange
        var card = Faker.FakePlayerCard(overallRating: 50);
        card.Boost(new DateOnly(2024, 5, 28), new DateOnly(2024, 5, 30), "Hit 5 HRs", Faker.FakePlayerCardAttributes());

        // Act
        var actual = card.IsBoosted;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void IsBoosted_BoostThatEnded_ReturnsFalse()
    {
        // Arrange
        var card = Faker.FakePlayerCard(overallRating: 50);
        card.Boost(new DateOnly(2024, 5, 28), new DateOnly(2024, 5, 29), "Hit 5 HRs", Faker.FakePlayerCardAttributes());
        card.RemoveBoost(new DateOnly(2024, 5, 29), Faker.FakePlayerCardAttributes());

        // Act
        var actual = card.IsBoosted;

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void ChangePlayerRating_NoPreviousHistoricalRating_AddsOriginalStateToHistory()
    {
        // Arrange
        var originalRating = Faker.FakeOverallRating(70);
        var originalAttributes = Faker.FakePlayerCardAttributes(scalar: 1);
        var card = Faker.FakePlayerCard(overallRating: originalRating.Value, playerCardAttributes: originalAttributes);

        var newRating = Faker.FakeOverallRating(75);
        var newAttributes = Faker.FakePlayerCardAttributes(scalar: 2);

        var date = new DateOnly(2024, 4, 2);

        // Act
        card.ChangePlayerRating(date, newRating, newAttributes);

        /*
         * Assert
         */
        // The current state of the card should reflect the most recent change
        Assert.Equal(75, card.OverallRating.Value);
        Assert.Equal(newAttributes, card.PlayerCardAttributes);
        Assert.Equal(Rarity.Silver, card.Rarity);

        // The original state should now be in the history
        var originalState = Faker.FakeBaselinePlayerCardHistoricalRating(
            new DateOnly(2024, 1, 1), date, originalRating.Value, originalAttributes);
        Assert.Equal(originalState, card.HistoricalRatingsChronologically[0]);

        // The new state should be in the history without an end date
        var newState = Faker.FakeBaselinePlayerCardHistoricalRating(
            date, null, newRating.Value, newAttributes);
        Assert.Equal(newState, card.HistoricalRatingsChronologically[1]);
    }

    [Fact]
    public void ChangePlayerRating_HasPreviousHistoricalRatings_EndsPreviousRatingAndAddsNewStateToHistory()
    {
        /*
         * Arrange
         */
        // The original state
        var originalRating = Faker.FakeOverallRating(70);
        var originalAttributes = Faker.FakePlayerCardAttributes(scalar: 1);
        var card = Faker.FakePlayerCard(overallRating: originalRating.Value, playerCardAttributes: originalAttributes);

        // The first change to the original state
        var previousRating = Faker.FakeOverallRating(75);
        var previousAttributes = Faker.FakePlayerCardAttributes(scalar: 2);
        var previousDate = new DateOnly(2024, 4, 1);
        card.ChangePlayerRating(previousDate, previousRating, previousAttributes);

        // The most recent change
        var newRating = Faker.FakeOverallRating(80);
        var newAttributes = Faker.FakePlayerCardAttributes(scalar: 3);
        var newDate = new DateOnly(2024, 4, 14);

        /*
         * Act
         */
        card.ChangePlayerRating(newDate, newRating, newAttributes);

        /*
         * Assert
         */
        // The current state of the card should reflect the most recent change
        Assert.Equal(80, card.OverallRating.Value);
        Assert.Equal(newAttributes, card.PlayerCardAttributes);
        Assert.Equal(Rarity.Gold, card.Rarity);

        // There are 3 states in the history: original, previous and new/current (which has no end date yet)
        var originalState = Faker.FakeBaselinePlayerCardHistoricalRating(
            new DateOnly(2024, 1, 1), previousDate, originalRating.Value, originalAttributes);
        var previousState = Faker.FakeBaselinePlayerCardHistoricalRating(
            previousDate, newDate, previousRating.Value, previousAttributes);
        var newState = Faker.FakeBaselinePlayerCardHistoricalRating(
            newDate, null, newRating.Value, newAttributes);
        Assert.Equal(3, card.HistoricalRatingsChronologically.Count);
        Assert.Equal(originalState, card.HistoricalRatingsChronologically[0]);
        Assert.Equal(previousState, card.HistoricalRatingsChronologically[1]);
        Assert.Equal(newState, card.HistoricalRatingsChronologically[2]);
    }

    [Fact]
    public void ChangePlayerRating_NewRatingIsHigher_RaisesOverallRatingImprovedDomainEvent()
    {
        // Arrange
        var originalRating = Faker.FakeOverallRating(70);
        var originalAttributes = Faker.FakePlayerCardAttributes(scalar: 1);
        var card = Faker.FakePlayerCard(overallRating: originalRating.Value, playerCardAttributes: originalAttributes,
            rarity: Rarity.Bronze);

        var newRating = Faker.FakeOverallRating(75);
        var newAttributes = Faker.FakePlayerCardAttributes(scalar: 2);

        // Act
        card.ChangePlayerRating(new DateOnly(2024, 4, 1), newRating, newAttributes);

        // Assert
        Assert.Single(card.DomainEvents);
        Assert.IsType<PlayerCardOverallRatingImprovedEvent>(card.DomainEvents[0]);
        var e = card.DomainEvents[0] as PlayerCardOverallRatingImprovedEvent;
        Assert.Equal(card.ExternalId, e!.CardExternalId);
        Assert.Equal(70, e.PreviousOverallRating.Value);
        Assert.Equal(originalAttributes, e.PreviousPlayerCardAttributes);
        Assert.Equal(75, e.NewOverallRating.Value);
        Assert.Equal(newAttributes, e.NewPlayerCardAttributes);
        Assert.True(e.RarityChanged);
        Assert.Equal(newRating.Rarity, card.Rarity);
    }

    [Fact]
    public void ChangePlayerRating_NewRatingIsLower_RaisesOverallRatingDeclinedDomainEvent()
    {
        // Arrange
        var originalRating = Faker.FakeOverallRating(60);
        var originalAttributes = Faker.FakePlayerCardAttributes(scalar: 2);
        var card = Faker.FakePlayerCard(overallRating: originalRating.Value, playerCardAttributes: originalAttributes,
            rarity: Rarity.Common);

        var newRating = Faker.FakeOverallRating(40);
        var newAttributes = Faker.FakePlayerCardAttributes(scalar: 1);

        // Act
        card.ChangePlayerRating(new DateOnly(2024, 4, 1), newRating, newAttributes);

        // Assert
        Assert.Single(card.DomainEvents);
        Assert.IsType<PlayerCardOverallRatingDeclinedEvent>(card.DomainEvents[0]);
        var e = card.DomainEvents[0] as PlayerCardOverallRatingDeclinedEvent;
        Assert.Equal(card.ExternalId, e!.CardExternalId);
        Assert.Equal(60, e.PreviousOverallRating.Value);
        Assert.Equal(originalAttributes, e.PreviousPlayerCardAttributes);
        Assert.Equal(40, e.NewOverallRating.Value);
        Assert.Equal(newAttributes, e.NewPlayerCardAttributes);
        Assert.False(e.RarityChanged);
        Assert.Equal(newRating.Rarity, card.Rarity);
    }

    [Fact]
    public void SetTemporaryRating_NewRatingIsHigher_SetsTempRatingAndAddsToHistoryAndRaisesImprovementEvent()
    {
        // Arrange
        var card = Faker.FakePlayerCard(overallRating: 60);
        var tempOverallRating = Faker.FakeOverallRating(70);
        var date = new DateOnly(2024, 5, 24);

        // Act
        card.SetTemporaryRating(date, tempOverallRating);

        /*
         * Assert
         */
        // OverallRating remains unchanged, TemporaryOverallRating changed
        Assert.Equal(60, card.OverallRating.Value);
        Assert.NotNull(card.TemporaryOverallRating);
        Assert.Equal(70, card.TemporaryOverallRating.Value);

        // Temporary rating added to history
        Assert.Single(card.HistoricalRatingsChronologically);
        var expectedRating = Faker.FakeTemporaryPlayerCardHistoricalRating(date, null, 70);
        Assert.Equal(expectedRating, card.HistoricalRatingsChronologically[0]);

        // Temporary ratings should not raise domain events
        Assert.Empty(card.DomainEvents);
    }

    [Fact]
    public void SetTemporaryRating_NewRatingIsLower_SetsTempRatingAndAddsToHistoryAndRaisesDeclineEvent()
    {
        // Arrange
        var card = Faker.FakePlayerCard(overallRating: 90);
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
        var expectedRating = Faker.FakeTemporaryPlayerCardHistoricalRating(date, null, 80);
        Assert.Equal(expectedRating, card.HistoricalRatingsChronologically[0]);

        // Temporary ratings should not raise domain events
        Assert.Empty(card.DomainEvents);
    }

    [Fact]
    public void RemoveTemporaryRating_HasTemporaryRating_Removed()
    {
        // Arrange
        var card = Faker.FakePlayerCard();
        var startDate = new DateOnly(2024, 5, 23);
        var endDate = new DateOnly(2024, 5, 29);
        card.SetTemporaryRating(startDate, Faker.FakeOverallRating(80));

        // Act
        card.RemoveTemporaryRating(endDate);

        // Assert
        Assert.Null(card.TemporaryOverallRating);

        var expectedRating = Faker.FakeTemporaryPlayerCardHistoricalRating(startDate, endDate, 80);
        Assert.Equal(expectedRating, card.HistoricalRatingsChronologically[0]);
    }

    [Fact]
    public void Boost_BoostedAttributes_BoostsRatingAndAddsToHistoryAndRaisesEvent()
    {
        // Arrange
        var card = Faker.FakePlayerCard(overallRating: 75);
        var boostedAttributes = Faker.FakePlayerCardAttributes(scalar: 2);
        var date = new DateOnly(2024, 5, 24);

        // Act
        card.Boost(date, date.AddDays(2), "Hit 5 HRs", boostedAttributes);

        /*
         * Assert
         */
        // Boosted
        Assert.True(card.IsBoosted);

        // OverallRating remains unchanged, TemporaryOverallRating changed
        Assert.Equal(75, card.OverallRating.Value);
        Assert.NotNull(card.TemporaryOverallRating);
        Assert.Equal(99, card.TemporaryOverallRating.Value);

        // Boost added to history
        Assert.Single(card.HistoricalRatingsChronologically);
        var expectedRating = Faker.FakeBoostPlayerCardHistoricalRating(date, null, 99, boostedAttributes);
        Assert.Equal(expectedRating, card.HistoricalRatingsChronologically[0]);

        // A boost event should have been raised
        Assert.Single(card.DomainEvents);
        Assert.IsType<PlayerCardBoostedEvent>(card.DomainEvents[0]);
        var e = card.DomainEvents[0] as PlayerCardBoostedEvent;
        Assert.Equal(card.ExternalId, e!.CardExternalId);
        Assert.Equal(99, e.NewOverallRating.Value);
        Assert.Equal(boostedAttributes, e.NewPlayerCardAttributes);
        Assert.Equal("Hit 5 HRs", e.BoostReason);
        Assert.Equal(new DateOnly(2024, 5, 26), e.BoostEndDate);
    }

    [Fact]
    public void RemoveBoost_BoostedCard_CardIsNoLongerBoosted()
    {
        // Arrange
        var normalAttributes = Faker.FakePlayerCardAttributes(scalar: 2);
        var card = Faker.FakePlayerCard(overallRating: 50, playerCardAttributes: normalAttributes);

        var boostedAttributes = Faker.FakePlayerCardAttributes(scalar: 4);
        var startDate = new DateOnly(2024, 5, 23);
        var endDate = new DateOnly(2024, 5, 29);
        card.Boost(startDate, endDate, "Hit 5 HRs", boostedAttributes);

        // Act
        card.RemoveBoost(endDate, normalAttributes);

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
        var expectedRating = Faker.FakeBoostPlayerCardHistoricalRating(startDate, endDate, 99, boostedAttributes);
        Assert.Equal(expectedRating, card.HistoricalRatingsChronologically[0]);
    }

    [Fact]
    public void AddHistoricalRating_AlreadyExists_ThrowsException()
    {
        // Arrange
        var card = Faker.FakePlayerCard();

        var playerAttributes = Faker.FakePlayerCardAttributes(scalar: 2);
        var startDate = new DateOnly(2024, 4, 1);
        var endDate = new DateOnly(2024, 4, 2);
        var historicalRating = Faker.FakeBaselinePlayerCardHistoricalRating(startDate, endDate, 49, playerAttributes);

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

        var playerAttributes = Faker.FakePlayerCardAttributes(scalar: 2);
        var startDate = new DateOnly(2024, 4, 1);
        var endDate = new DateOnly(2024, 4, 2);
        var historicalRating = Faker.FakeBaselinePlayerCardHistoricalRating(startDate, endDate, 49, playerAttributes);

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
        card.ChangePosition(newPosition, new DateOnly(2024, 10, 28));

        // Assert
        Assert.Equal(Position.Catcher, card.Position);
        Assert.Single(card.DomainEvents);
        Assert.IsType<PlayerCardPositionChangedEvent>(card.DomainEvents[0]);
        var e = card.DomainEvents[0] as PlayerCardPositionChangedEvent;
        Assert.Equal(card.ExternalId, e!.CardExternalId);
        Assert.Equal(Position.Catcher, e.NewPosition);
        Assert.Equal(Position.FirstBase, e.OldPosition);
        Assert.Equal(new DateOnly(2024, 10, 28), e.Date);
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
        card.AddHistoricalRating(Faker.FakeBaselinePlayerCardHistoricalRating(startDate, endDate));

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
        card.AddHistoricalRating(Faker.FakeBaselinePlayerCardHistoricalRating(startDate, endDate));

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