using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos;

public class AllRosterUpdatesTests
{
    [Fact]
    public void OldToNew_RosterUpdateList_ReturnsOldestToNewest()
    {
        // Arrange
        var rosterUpdate1 = Faker.FakeRosterUpdate(date: new DateOnly(2024, 7, 1));
        var rosterUpdate2 = Faker.FakeRosterUpdate(date: new DateOnly(2024, 7, 15));
        var allRosterUpdates = new AllRosterUpdates(new List<RosterUpdate>() { rosterUpdate1, rosterUpdate2 });

        // Act
        var actual = allRosterUpdates.OldToNew;

        // Assert
        Assert.Equal(rosterUpdate1, actual.First());
        Assert.Equal(rosterUpdate2, actual.Last());
    }

    [Fact]
    public void NewToOld_RosterUpdateList_ReturnsNewestToOldest()
    {
        // Arrange
        var rosterUpdate1 = Faker.FakeRosterUpdate(date: new DateOnly(2024, 7, 1));
        var rosterUpdate2 = Faker.FakeRosterUpdate(date: new DateOnly(2024, 7, 15));
        var allRosterUpdates = new AllRosterUpdates(new List<RosterUpdate>() { rosterUpdate1, rosterUpdate2 });

        // Act
        var actual = allRosterUpdates.NewToOld;

        // Assert
        Assert.Equal(rosterUpdate2, actual.First());
        Assert.Equal(rosterUpdate1, actual.Last());
    }
}