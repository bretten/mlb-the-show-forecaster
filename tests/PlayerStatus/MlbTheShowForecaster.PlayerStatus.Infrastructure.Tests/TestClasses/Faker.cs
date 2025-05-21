using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.RosterEntries;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Tests.TestClasses;

/// <summary>
/// Fakes players and teams
/// </summary>
public static class Faker
{
    public const int DefaultMlbId = 1;
    public const int DefaultTeamMlbId = 136;
    public const string DefaultFirstName = "First";
    public const string DefaultLastName = "Last";

    public static PlayerDto FakePlayerDto(int? mlbId = null, string? firstName = null, string? lastName = null,
        DateOnly birthdate = default, PositionDto? position = null, DateOnly mlbDebutDate = default,
        ArmSideDto? batSide = null, ArmSideDto? throwArm = null, CurrentTeamDto? team = null, bool active = false)
    {
        return new PlayerDto()
        {
            Id = mlbId ?? DefaultMlbId,
            FirstName = firstName ?? DefaultFirstName,
            LastName = lastName ?? DefaultLastName,
            Birthdate = birthdate == default ? new DateOnly(1990, 1, 1) : birthdate,
            Position = position ?? new PositionDto("Catcher", "C"),
            MlbDebutDate = mlbDebutDate == default ? new DateOnly(2010, 1, 1) : mlbDebutDate,
            BatSide = batSide ?? new ArmSideDto("L", "Left"),
            ThrowArm = throwArm ?? new ArmSideDto("L", "Left"),
            CurrentTeam = team ?? new CurrentTeamDto(DefaultTeamMlbId),
            Active = active
        };
    }

    public static RosterEntryDto FakeRosterEntryDto(string statusCode = "A", string statusDescription = "Active",
        TeamDto? team = null, bool isActive = false, DateOnly startDate = default, DateOnly endDate = default,
        DateOnly statusDate = default, bool isActiveFortyMan = false)
    {
        return new RosterEntryDto()
        {
            Status = new RosterEntryStatusDto(statusCode, statusDescription),
            Team = team ?? new TeamDto(DefaultTeamMlbId, "Default"),
            IsActive = isActive,
            StartDate = startDate == default ? new DateOnly(2025, 5, 21) : startDate,
            EndDate = endDate == default ? new DateOnly(2025, 5, 22) : endDate,
            StatusDate = statusDate == default ? new DateOnly(2025, 5, 23) : statusDate,
            IsActiveFortyMan = isActiveFortyMan
        };
    }
}