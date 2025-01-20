using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.TestClasses;

/// <summary>
/// Fakes a roster entry
/// </summary>
public static class Faker
{
    public const int DefaultMlbId = 1;
    public const int DefaultTeamMlbId = 136;
    public const string DefaultFirstName = "First";
    public const string DefaultLastName = "Last";

    public static RosterEntry FakeRosterEntry(int? mlbId = null, string? firstName = null, string? lastName = null,
        DateOnly birthdate = default, Position position = Position.Catcher, DateOnly mlbDebutDate = default,
        BatSide batSide = BatSide.Left, ThrowArm throwArm = ThrowArm.Left, int? teamMlbId = null, bool active = false)
    {
        return new RosterEntry(
            mlbId == null ? MlbId.Create(DefaultMlbId) : MlbId.Create(mlbId.Value),
            firstName == null ? PersonName.Create(DefaultFirstName) : PersonName.Create(firstName),
            lastName == null ? PersonName.Create(DefaultLastName) : PersonName.Create(lastName),
            birthdate == default ? new DateOnly(1990, 1, 1) : birthdate,
            position,
            mlbDebutDate == default ? new DateOnly(2010, 1, 1) : mlbDebutDate,
            batSide,
            throwArm,
            teamMlbId == null ? MlbId.Create(DefaultTeamMlbId) : MlbId.Create(teamMlbId.Value),
            active
        );
    }
}