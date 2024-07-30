using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.AdministrativeImpacts;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Forecasts.ValueObjects.StatImpacts;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Tests.Forecasts.TestClasses;

public static class Faker
{
    public static Guid FakeGuid1 = new("00000000-0000-0000-0000-000000000001");
    public static DateOnly EndDate = new DateOnly(2024, 7, 29);

    public static PlayerCardForecast FakePlayerCardForecast(ushort year = 2024, Guid? externalId = null, int mlbId = 1,
        Position position = Position.RightField, int overallRating = 50, BoostForecastImpact? boostImpact = null,
        PositionChangeForecastImpact? positionChangeImpact = null, PriceForecastImpact? priceImpact = null,
        BattingStatsForecastImpact? battingStatsImpact = null, PitchingStatsForecastImpact? pitchingStatsImpact = null,
        FieldingStatsForecastImpact? fieldingStatsImpact = null,
        PlayerActivationForecastImpact? activationImpact = null,
        PlayerDeactivationForecastImpact? deactivationImpact = null,
        PlayerFreeAgencyForecastImpact? freeAgencyImpact = null,
        PlayerTeamSigningForecastImpact? teamSigningImpact = null)
    {
        return PlayerCardForecast.Create(SeasonYear.Create(year),
            Cards.TestClasses.Faker.FakeCardExternalId(externalId), MlbId.Create(mlbId), position,
            OverallRating.Create(overallRating), boostImpact, positionChangeImpact, priceImpact, battingStatsImpact,
            pitchingStatsImpact, fieldingStatsImpact, activationImpact, deactivationImpact, freeAgencyImpact,
            teamSigningImpact);
    }

    public static BoostForecastImpact FakeBoostForecastImpact(string boostReason = "Hit 5 HRs",
        DateOnly? endDate = null)
    {
        return new BoostForecastImpact(boostReason, endDate ?? EndDate);
    }

    public static PositionChangeForecastImpact FakePositionChangeForecastImpact(
        Position oldPosition = Position.RightField, Position newPosition = Position.CenterField,
        DateOnly? endDate = null)
    {
        return new PositionChangeForecastImpact(oldPosition, newPosition, endDate ?? EndDate);
    }

    public static PriceForecastImpact FakePriceForecastImpact(int oldPrice = 1, int newPrice = 10,
        DateOnly? endDate = null)
    {
        return new PriceForecastImpact(NaturalNumber.Create(oldPrice), NaturalNumber.Create(newPrice),
            endDate ?? EndDate);
    }

    public static BattingStatsForecastImpact FakeBattingStatsForecastImpact(decimal oldScore = 0.2m,
        decimal newScore = 0.5m, DateOnly? endDate = null)
    {
        return new BattingStatsForecastImpact(oldScore, newScore, endDate ?? EndDate);
    }

    public static PitchingStatsForecastImpact FakePitchingStatsForecastImpact(decimal oldScore = 0.2m,
        decimal newScore = 0.5m, DateOnly? endDate = null)
    {
        return new PitchingStatsForecastImpact(oldScore, newScore, endDate ?? EndDate);
    }

    public static FieldingStatsForecastImpact FakeFieldingStatsForecastImpact(decimal oldScore = 0.2m,
        decimal newScore = 0.5m, DateOnly? endDate = null)
    {
        return new FieldingStatsForecastImpact(oldScore, newScore, endDate ?? EndDate);
    }

    public static PlayerActivationForecastImpact FakePlayerActivationForecastImpact(DateOnly? endDate = null)
    {
        return new PlayerActivationForecastImpact(endDate ?? EndDate);
    }

    public static PlayerDeactivationForecastImpact FakePlayerDeactivationForecastImpact(DateOnly? endDate = null)
    {
        return new PlayerDeactivationForecastImpact(endDate ?? EndDate);
    }

    public static PlayerFreeAgencyForecastImpact FakePlayerFreeAgencyForecastImpact(DateOnly? endDate = null)
    {
        return new PlayerFreeAgencyForecastImpact(endDate ?? EndDate);
    }

    public static PlayerTeamSigningForecastImpact FakePlayerTeamSigningForecastImpact(DateOnly? endDate = null)
    {
        return new PlayerTeamSigningForecastImpact(endDate ?? EndDate);
    }
}