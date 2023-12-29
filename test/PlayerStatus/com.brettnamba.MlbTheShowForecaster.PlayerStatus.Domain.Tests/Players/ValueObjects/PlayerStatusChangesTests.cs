using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.Enums;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Players.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Teams.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.PlayerStatus.Domain.Tests.Players.ValueObjects;

public class PlayerStatusChangesTests
{
    [Fact]
    public void SignedContractWithTeam_ContainsNewTeamChange_ReturnsTrue()
    {
        // Arrange
        var statusChanges = new PlayerStatusChanges(new List<PlayerStatusChangeType>()
        {
            PlayerStatusChangeType.SignedContractWithTeam
        }, TeamFaker.Fake());

        // Act
        var actual = statusChanges.SignedContractWithTeam;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void SignedContractWithTeam_NoChanges_ReturnsFalse()
    {
        // Arrange
        var statusChanges = new PlayerStatusChanges(new List<PlayerStatusChangeType>(), TeamFaker.NoTeam);

        // Act
        var actual = statusChanges.SignedContractWithTeam;

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void EnteredFreeAgency_ContainsFreeAgencyChange_ReturnsTrue()
    {
        // Arrange
        var statusChanges = new PlayerStatusChanges(new List<PlayerStatusChangeType>()
        {
            PlayerStatusChangeType.EnteredFreeAgency
        }, TeamFaker.NoTeam);

        // Act
        var actual = statusChanges.EnteredFreeAgency;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void EnteredFreeAgency_NoChanges_ReturnsFalse()
    {
        // Arrange
        var statusChanges = new PlayerStatusChanges(new List<PlayerStatusChangeType>(), TeamFaker.NoTeam);

        // Act
        var actual = statusChanges.EnteredFreeAgency;

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Activated_ContainsActiveChange_ReturnsTrue()
    {
        // Arrange
        var statusChanges = new PlayerStatusChanges(new List<PlayerStatusChangeType>()
        {
            PlayerStatusChangeType.Activated
        }, TeamFaker.NoTeam);

        // Act
        var actual = statusChanges.Activated;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Activated_NoChanges_ReturnsFalse()
    {
        // Arrange
        var statusChanges = new PlayerStatusChanges(new List<PlayerStatusChangeType>(), TeamFaker.NoTeam);

        // Act
        var actual = statusChanges.Activated;

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Inactivated_ContainsInactiveChange_ReturnsTrue()
    {
        // Arrange
        var statusChanges = new PlayerStatusChanges(new List<PlayerStatusChangeType>()
        {
            PlayerStatusChangeType.Inactivated
        }, TeamFaker.NoTeam);

        // Act
        var actual = statusChanges.Inactivated;

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Inactivated_NoChanges_ReturnsFalse()
    {
        // Arrange
        var statusChanges = new PlayerStatusChanges(new List<PlayerStatusChangeType>(), TeamFaker.NoTeam);

        // Act
        var actual = statusChanges.Inactivated;

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void Any_Changes_ReturnsTrue()
    {
        // Arrange
        var statusChanges = new PlayerStatusChanges(new List<PlayerStatusChangeType>()
        {
            PlayerStatusChangeType.Activated
        }, TeamFaker.NoTeam);

        // Act
        var actual = statusChanges.Any();

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void Any_NoChanges_ReturnsFalse()
    {
        // Arrange
        var statusChanges = new PlayerStatusChanges(new List<PlayerStatusChangeType>(), TeamFaker.NoTeam);

        // Act
        var actual = statusChanges.Any();

        // Assert
        Assert.False(actual);
    }
}