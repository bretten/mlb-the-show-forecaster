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
    public static Guid FakeGuid2 = new("00000000-0000-0000-0000-000000000002");
    public static Guid FakeGuid3 = new("00000000-0000-0000-0000-000000000003");
    public static DateOnly StartDate = new DateOnly(2024, 7, 25);
    public static DateOnly EndDate = new DateOnly(2024, 7, 29);

    public static PlayerCardForecast FakePlayerCardForecast(ushort year = 2024, Guid? externalId = null, int mlbId = 1,
        Position position = Position.RightField, int overallRating = 50)
    {
        return PlayerCardForecast.Create(SeasonYear.Create(year),
            Cards.TestClasses.Faker.FakeCardExternalId(externalId), MlbId.Create(mlbId), position,
            OverallRating.Create(overallRating));
    }

    public static OverallRatingChangeForecastImpact FakeOverallRatingChangeForecastImpact(int oldOverallRating = 50,
        int newOverallRating = 50, DateOnly? startDate = null, DateOnly? endDate = null)
    {
        return new OverallRatingChangeForecastImpact(oldRating: OverallRating.Create(oldOverallRating),
            newRating: OverallRating.Create(newOverallRating), startDate ?? StartDate, endDate ?? EndDate);
    }

    public static BoostForecastImpact FakeBoostForecastImpact(string boostReason = "Hit 5 HRs",
        DateOnly? startDate = null, DateOnly? endDate = null)
    {
        return new BoostForecastImpact(boostReason, startDate ?? StartDate, endDate ?? EndDate);
    }

    public static PositionChangeForecastImpact FakePositionChangeForecastImpact(
        Position oldPosition = Position.RightField, Position newPosition = Position.CenterField,
        DateOnly? startDate = null, DateOnly? endDate = null)
    {
        return new PositionChangeForecastImpact(oldPosition, newPosition, startDate ?? StartDate, endDate ?? EndDate);
    }

    public static BattingStatsForecastImpact FakeBattingStatsForecastImpact(decimal oldScore = 0.2m,
        decimal newScore = 0.5m, DateOnly? startDate = null, DateOnly? endDate = null)
    {
        return new BattingStatsForecastImpact(oldScore, newScore, startDate ?? StartDate, endDate ?? EndDate);
    }

    public static PitchingStatsForecastImpact FakePitchingStatsForecastImpact(decimal oldScore = 0.2m,
        decimal newScore = 0.5m, DateOnly? startDate = null, DateOnly? endDate = null)
    {
        return new PitchingStatsForecastImpact(oldScore, newScore, startDate ?? StartDate, endDate ?? EndDate);
    }

    public static FieldingStatsForecastImpact FakeFieldingStatsForecastImpact(decimal oldScore = 0.2m,
        decimal newScore = 0.5m, DateOnly? startDate = null, DateOnly? endDate = null)
    {
        return new FieldingStatsForecastImpact(oldScore, newScore, startDate ?? StartDate, endDate ?? EndDate);
    }

    public static PlayerActivationForecastImpact FakePlayerActivationForecastImpact(DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        return new PlayerActivationForecastImpact(startDate ?? StartDate, endDate ?? EndDate);
    }

    public static PlayerDeactivationForecastImpact FakePlayerDeactivationForecastImpact(DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        return new PlayerDeactivationForecastImpact(startDate ?? StartDate, endDate ?? EndDate);
    }

    public static PlayerFreeAgencyForecastImpact FakePlayerFreeAgencyForecastImpact(DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        return new PlayerFreeAgencyForecastImpact(startDate ?? StartDate, endDate ?? EndDate);
    }

    public static PlayerTeamSigningForecastImpact FakePlayerTeamSigningForecastImpact(DateOnly? startDate = null,
        DateOnly? endDate = null)
    {
        return new PlayerTeamSigningForecastImpact(startDate ?? StartDate, endDate ?? EndDate);
    }
}