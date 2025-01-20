using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Players.TestClasses;

/// <summary>
/// Fakes a Player
/// </summary>
public static class Faker
{
    public const int DefaultMlbId = 1;
    public const string DefaultFirstName = "First";
    public const string DefaultLastName = "Last";

    public static Player FakePlayer(int? mlbId = null, string? firstName = null, string? lastName = null,
        DateOnly birthdate = default, Position position = Position.Catcher, DateOnly mlbDebutDate = default,
        BatSide batSide = BatSide.Left, ThrowArm throwArm = ThrowArm.Left, Team? team = null, bool active = false)
    {
        return Player.Create(
            mlbId == null ? MlbId.Create(DefaultMlbId) : MlbId.Create(mlbId.Value),
            firstName == null ? PersonName.Create(DefaultFirstName) : PersonName.Create(firstName),
            lastName == null ? PersonName.Create(DefaultLastName) : PersonName.Create(lastName),
            birthdate == default ? new DateOnly(1990, 1, 1) : birthdate,
            position,
            mlbDebutDate == default ? new DateOnly(2010, 1, 1) : mlbDebutDate,
            batSide,
            throwArm,
            team,
            active
        );
    }
}