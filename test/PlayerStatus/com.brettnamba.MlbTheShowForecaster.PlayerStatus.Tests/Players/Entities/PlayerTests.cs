using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Players.Entities;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Teams.ValueObjects;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Tests.Players.Entities;

public class PlayerTests
{
    [Fact]
    public void Create_ValidValues_Created()
    {
        // Arrange
        var mlbId = MlbId.Create(1);
        var firstName = PersonName.Create("First");
        var lastName = PersonName.Create("Last");
        var birthdate = new DateTime(1990, 1, 1);
        var position = Position.CenterField;
        var mlbDebutDate = new DateTime(2010, 1, 1);
        var batSide = BatSide.Left;
        var throwArm = ThrowArm.Right;
        var team = Team.Create(MlbId.Create(2), TeamName.Create("Mariners"), TeamAbbreviation.Create("SEA"));
        var active = true;

        // Act
        var actual = Player.Create(mlbId, firstName, lastName, birthdate, position, mlbDebutDate, batSide, throwArm,
            team, active);

        // Assert
        Assert.Equal(1, actual.MlbId.Value);
        Assert.Equal("First", actual.FirstName.Value);
        Assert.Equal("Last", actual.LastName.Value);
        Assert.Equal(new DateTime(1990, 1, 1), actual.Birthdate);
        Assert.Equal(Position.CenterField, actual.Position);
        Assert.Equal(new DateTime(2010, 1, 1), actual.MlbDebutDate);
        Assert.Equal(BatSide.Left, actual.BatSide);
        Assert.Equal(ThrowArm.Right, actual.ThrowArm);
        Assert.Equal(2, actual.Team?.MlbId.Value);
        Assert.True(actual.Active);
    }
}