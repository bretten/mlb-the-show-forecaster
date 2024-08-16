using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.Entities;

public class PlayerCardForecastTests
{
    [Fact]
    public void ForecastImpactsChronologically_Forecast_ReturnsForecastInOrder()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast();
        var boostImpact = Faker.FakeBoostForecastImpact(endDate: new DateOnly(2024, 8, 5));
        var activationImpact = Faker.FakePlayerActivationForecastImpact(endDate: new DateOnly(2024, 8, 1));
        forecast.Reassess(boostImpact, new DateOnly(2024, 8, 5));
        forecast.Reassess(activationImpact, new DateOnly(2024, 8, 5));

        // Act
        var actual = forecast.ForecastImpactsChronologically;

        // Assert
        Assert.Equal(activationImpact, actual[0]);
        Assert.Equal(boostImpact, actual[1]);
    }

    [Fact]
    public void Reassess_BetterOverallRatingRarity_RaisesDemandIncreasedEvent()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast();
        const int oldOvr = 50; // Common rarity
        const int newOvr = 70; // Bronze rarity
        var impact = Faker.FakeOverallRatingChangeForecastImpact(oldOverallRating: oldOvr, newOverallRating: newOvr);

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandIncreasedEvent>(forecast.DomainEvents[0]);
    }

    [Fact]
    public void Reassess_WorseOverallRatingRarity_RaisesDemandDecreasedEvent()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast();
        const int oldOvr = 70; // Bronze rarity
        const int newOvr = 50; // Common rarity
        var impact = Faker.FakeOverallRatingChangeForecastImpact(oldOverallRating: oldOvr, newOverallRating: newOvr);

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandDecreasedEvent>(forecast.DomainEvents[0]);
    }

    [Fact]
    public void Reassess_DifferentOverallRatingSameRarity_NoEventsRaised()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast();
        const int oldOvr = 50; // Common rarity
        const int newOvr = 60; // Still common rarity
        var impact = Faker.FakeOverallRatingChangeForecastImpact(oldOverallRating: oldOvr, newOverallRating: newOvr);

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Empty(forecast.DomainEvents);
    }

    [Fact]
    public void Reassess_Boost_RaisesDemandIncreasedEvent()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast();
        var impact = Faker.FakeBoostForecastImpact();

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandIncreasedEvent>(forecast.DomainEvents[0]);
    }

    [Fact]
    public void Reassess_PositionChangeToDesiredPosition_RaisesDemandIncreasedEvent()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast(position: Position.RightField);
        var impact = Faker.FakePositionChangeForecastImpact(newPosition: Position.Catcher);

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Equal(Position.Catcher, forecast.PrimaryPosition);
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandIncreasedEvent>(forecast.DomainEvents[0]);
    }

    [Fact]
    public void Reassess_PositionChangeToCommonPosition_NoEventsRaised()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast(position: Position.CenterField);
        var impact = Faker.FakePositionChangeForecastImpact(newPosition: Position.RightField);

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Equal(Position.RightField, forecast.PrimaryPosition);
        Assert.Empty(forecast.DomainEvents);
    }

    [Fact]
    public void Reassess_BatterScoreIncreaseByNonBatter_NoEventsRaised()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast(position: Position.StartingPitcher);
        var impact = Faker.FakeBattingStatsForecastImpact(oldScore: 0.1m, newScore: 0.9m);

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Empty(forecast.DomainEvents);
    }

    [Fact]
    public void Reassess_InsignificantBattingScoreChange_NoEventsRaised()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast(position: Position.Catcher);
        var impact = Faker.FakeBattingStatsForecastImpact(oldScore: 0.1m, newScore: 0.11m);

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Empty(forecast.DomainEvents);
    }

    [Fact]
    public void Reassess_BattingScoreIncrease_RaisesDemandIncreasedEvent()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast(position: Position.Catcher);
        var impact = Faker.FakeBattingStatsForecastImpact(oldScore: 0.1m, newScore: 0.9m);

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandIncreasedEvent>(forecast.DomainEvents[0]);
    }

    [Fact]
    public void Reassess_BattingScoreDecrease_RaisesDemandDecreasedEvent()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast(position: Position.Catcher);
        var impact = Faker.FakeBattingStatsForecastImpact(oldScore: 0.9m, newScore: 0.1m);

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandDecreasedEvent>(forecast.DomainEvents[0]);
    }

    [Fact]
    public void Reassess_PitcherScoreIncreaseByNonPitcher_NoEventsRaised()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast(position: Position.SecondBase);
        var impact = Faker.FakePitchingStatsForecastImpact(oldScore: 0.1m, newScore: 0.9m);

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Empty(forecast.DomainEvents);
    }

    [Fact]
    public void Reassess_InsignificantPitchingScoreChange_NoEventsRaised()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast(position: Position.StartingPitcher);
        var impact = Faker.FakePitchingStatsForecastImpact(oldScore: 0.1m, newScore: 0.11m);

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Empty(forecast.DomainEvents);
    }

    [Fact]
    public void Reassess_PitchingScoreIncrease_RaisesDemandIncreasedEvent()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast(position: Position.StartingPitcher);
        var impact = Faker.FakePitchingStatsForecastImpact(oldScore: 0.1m, newScore: 0.9m);

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandIncreasedEvent>(forecast.DomainEvents[0]);
    }

    [Fact]
    public void Reassess_PitchingScoreDecrease_RaisesDemandDecreasedEvent()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast(position: Position.StartingPitcher);
        var impact = Faker.FakePitchingStatsForecastImpact(oldScore: 0.9m, newScore: 0.1m);

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandDecreasedEvent>(forecast.DomainEvents[0]);
    }

    [Fact]
    public void Reassess_InsignificantFieldingScoreChange_NoEventsRaised()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast();
        var impact = Faker.FakeFieldingStatsForecastImpact(oldScore: 0.1m, newScore: 0.11m);

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Empty(forecast.DomainEvents);
    }

    [Fact]
    public void Reassess_FieldingScoreIncrease_RaisesDemandIncreasedEvent()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast();
        var impact = Faker.FakeFieldingStatsForecastImpact(oldScore: 0.1m, newScore: 0.9m);

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandIncreasedEvent>(forecast.DomainEvents[0]);
    }

    [Fact]
    public void Reassess_FieldingScoreDecrease_RaisesDemandDecreasedEvent()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast();
        var impact = Faker.FakeFieldingStatsForecastImpact(oldScore: 0.9m, newScore: 0.1m);

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandDecreasedEvent>(forecast.DomainEvents[0]);
    }

    [Fact]
    public void Reassess_PlayerActivated_RaisesDemandIncreasedEvent()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast();
        var impact = Faker.FakePlayerActivationForecastImpact();

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandIncreasedEvent>(forecast.DomainEvents[0]);
    }

    [Fact]
    public void Reassess_PlayerDeactivated_RaisesDemandDecreasedEvent()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast();
        var impact = Faker.FakePlayerDeactivationForecastImpact();

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandDecreasedEvent>(forecast.DomainEvents[0]);
    }

    [Fact]
    public void Reassess_PlayerFreeAgency_RaisesDemandDecreasedEvent()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast();
        var impact = Faker.FakePlayerFreeAgencyForecastImpact();

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandDecreasedEvent>(forecast.DomainEvents[0]);
    }

    [Fact]
    public void Reassess_PlayerTeamSigning_RaisesDemandIncreasedEvent()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast();
        var impact = Faker.FakePlayerTeamSigningForecastImpact();

        // Act
        forecast.Reassess(impact, Faker.EndDate);

        // Assert
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandIncreasedEvent>(forecast.DomainEvents[0]);
    }

    [Fact]
    public void Create_ValidValues_Created()
    {
        // Arrange
        var seasonYear = SeasonYear.Create(2024);
        var externalId = Cards.TestClasses.Faker.FakeCardExternalId(Faker.FakeGuid1);
        var mlbId = MlbId.Create(1);
        const Position position = Position.CenterField;
        var overallRating = OverallRating.Create(80);

        // Act
        var actual = PlayerCardForecast.Create(seasonYear, externalId, mlbId, position, overallRating);

        // Assert
        Assert.Equal(2024, actual.Year.Value);
        Assert.Equal("00000000-0000-0000-0000-000000000001", actual.CardExternalId.Value.ToString("D"));
        Assert.Equal(1, actual.MlbId!.Value);
        Assert.Equal(Position.CenterField, actual.PrimaryPosition);
        Assert.Equal(80, actual.OverallRating.Value);
    }
}