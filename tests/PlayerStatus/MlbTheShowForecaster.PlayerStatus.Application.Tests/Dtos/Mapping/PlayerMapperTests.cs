using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.TestClasses;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Tests.Dtos.Mapping;

public class PlayerMapperTests
{
    [Fact]
    public void Map_RosterEntry_ReturnsPlayer()
    {
        // Arrange
        var rosterEntry = Faker.FakeRosterEntry(mlbId: 10,
            firstName: "firstN",
            lastName: "lastN",
            birthdate: new DateTime(2024, 4, 1),
            position: Position.CenterField,
            mlbDebutDate: new DateTime(2022, 2, 1),
            batSide: BatSide.Left,
            throwArm: ThrowArm.Switch,
            teamMlbId: 140,
            active: true
        );
        var mapper = new PlayerMapper();

        // Act
        var actual = mapper.Map(rosterEntry);

        // Assert
        Assert.Equal(10, actual.MlbId.Value);
        Assert.Equal("firstN", actual.FirstName.Value);
        Assert.Equal("lastN", actual.LastName.Value);
        Assert.Equal(new DateTime(2024, 4, 1), actual.Birthdate);
        Assert.Equal(Position.CenterField, actual.Position);
        Assert.Equal(new DateTime(2022, 2, 1), actual.MlbDebutDate);
        Assert.Equal(BatSide.Left, actual.BatSide);
        Assert.Equal(ThrowArm.Switch, actual.ThrowArm);
        Assert.Equal(Team.Create((TeamInfo)140), actual.Team);
        Assert.True(actual.Active);
    }
}