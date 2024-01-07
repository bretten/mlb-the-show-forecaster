using AutoMapper;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Common.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Mapping.AutoMapper;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Tests.TestClasses;
using Position = com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbApi.Dtos.Position;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Infrastructure.Tests.Mapping.AutoMapper;

public class MlbApiProfileTests
{
    [Fact]
    public void Constructor_MappingConfiguration_ChecksForValidConfiguration()
    {
        // Arrange
        var config = new MapperConfiguration(x => x.AddMaps(typeof(MlbApiProfile)));
        var action = () => config.AssertConfigurationIsValid();

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.Null(actual);
    }

    [Fact]
    public void Map_PlayerDto_ReturnsRosterEntry()
    {
        // Arrange
        var config = new MapperConfiguration(x => x.AddMaps(typeof(MlbApiProfile)));
        var mapper = config.CreateMapper();
        var fakePlayerDto = Faker.FakePlayerDto(
            mlbId: 1,
            firstName: "FirstName",
            lastName: "LastName",
            birthdate: new DateTime(1999, 10, 31),
            position: new Position("Right Field", "RF"),
            mlbDebutDate: new DateTime(2022, 3, 1),
            batSide: new ArmSide("S", "Switch"),
            throwArm: new ArmSide("R", "Right"),
            team: new CurrentTeam(100),
            active: true
        );

        // Act
        var actual = mapper.Map<Player, RosterEntry>(fakePlayerDto);

        // Assert
        Assert.Equal(MlbId.Create(1), actual.MlbId);
        Assert.Equal(PersonName.Create("FirstName"), actual.FirstName);
        Assert.Equal(PersonName.Create("LastName"), actual.LastName);
        Assert.Equal(new DateTime(1999, 10, 31), actual.Birthdate);
        Assert.Equal(Domain.Players.Enums.Position.RightField, actual.Position);
        Assert.Equal(new DateTime(2022, 3, 1), actual.MlbDebutDate);
        Assert.Equal(BatSide.Switch, actual.BatSide);
        Assert.Equal(ThrowArm.Right, actual.ThrowArm);
        Assert.Equal(MlbId.Create(100), actual.CurrentTeamMlbId);
        Assert.True(actual.Active);
    }
}