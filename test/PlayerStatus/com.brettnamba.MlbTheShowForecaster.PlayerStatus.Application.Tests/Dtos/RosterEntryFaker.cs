using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.Dtos;

/// <summary>
/// Fakes a roster entry
/// </summary>
public static class RosterEntryFaker
{
    public const int DefaultMlbId = 1;
    public const int DefaultTeamMlbId = 100;
    public const string DefaultFirstName = "First";
    public const string DefaultLastName = "Last";

    public static RosterEntry Fake(int? mlbId = null, string? firstName = null, string? lastName = null,
        DateTime birthdate = default, Position position = Position.Catcher, DateTime mlbDebutDate = default,
        BatSide batSide = BatSide.Left, ThrowArm throwArm = ThrowArm.Left, int? teamMlbId = null, bool active = false)
    {
        return new RosterEntry(
            mlbId == null ? MlbId.Create(DefaultMlbId) : MlbId.Create(mlbId.Value),
            firstName == null ? PersonName.Create(DefaultFirstName) : PersonName.Create(firstName),
            lastName == null ? PersonName.Create(DefaultLastName) : PersonName.Create(lastName),
            birthdate == default ? new DateTime(1990, 1, 1) : birthdate,
            position,
            mlbDebutDate == default ? new DateTime(2010, 1, 1) : mlbDebutDate,
            batSide,
            throwArm,
            teamMlbId == null ? MlbId.Create(DefaultTeamMlbId) : MlbId.Create(teamMlbId.Value),
            active
        );
    }
}