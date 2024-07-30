using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Events;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.Entities;

public class PlayerCardForecastTests
{
    [Fact]
    public void Reassess_Boost_RaisesDemandIncreasedEvent()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast();
        var impact = Faker.FakeBoostForecastImpact();

        // Act
        forecast.Reassess(impact);

        // Assert
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandIncreasedEvent>(forecast.DomainEvents[0]);
    }

    [Fact]
    public void Reassess_PositionChangeToDesiredPosition_RaisesDemandIncreasedEvent()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast();
        var impact = Faker.FakePositionChangeForecastImpact(newPosition: Position.Catcher);

        // Act
        forecast.Reassess(impact);

        // Assert
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandIncreasedEvent>(forecast.DomainEvents[0]);
    }

    [Fact]
    public void Reassess_PositionChangeToCommonPosition_NoEventsRaised()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast();
        var impact = Faker.FakePositionChangeForecastImpact(newPosition: Position.RightField);

        // Act
        forecast.Reassess(impact);

        // Assert
        Assert.Empty(forecast.DomainEvents);
    }

    [Fact]
    public void Reassess_PriceChange_RaisesDemandIncreasedEvent()
    {
        throw new Exception();
        // Arrange
        var forecast = Faker.FakePlayerCardForecast();
        var impact = Faker.FakePositionChangeForecastImpact(newPosition: Position.Catcher);

        // Act
        forecast.Reassess(impact);

        // Assert
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandIncreasedEvent>(forecast.DomainEvents[0]);
    }

    [Fact]
    public void Reassess_BatterScoreIncreaseByNonBatter_NoEventsRaised()
    {
        // Arrange
        var forecast = Faker.FakePlayerCardForecast(position: Position.StartingPitcher);
        var impact = Faker.FakeBattingStatsForecastImpact(oldScore: 0.1m, newScore: 0.9m);

        // Act
        forecast.Reassess(impact);

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
        forecast.Reassess(impact);

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
        forecast.Reassess(impact);

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
        forecast.Reassess(impact);

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
        forecast.Reassess(impact);

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
        forecast.Reassess(impact);

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
        forecast.Reassess(impact);

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
        forecast.Reassess(impact);

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
        forecast.Reassess(impact);

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
        forecast.Reassess(impact);

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
        forecast.Reassess(impact);

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
        forecast.Reassess(impact);

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
        forecast.Reassess(impact);

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
        forecast.Reassess(impact);

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
        forecast.Reassess(impact);

        // Assert
        Assert.Single(forecast.DomainEvents);
        Assert.IsType<CardDemandIncreasedEvent>(forecast.DomainEvents[0]);
    }
}