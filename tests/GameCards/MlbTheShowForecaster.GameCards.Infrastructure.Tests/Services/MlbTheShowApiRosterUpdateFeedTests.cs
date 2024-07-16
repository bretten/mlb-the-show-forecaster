using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.RosterUpdates;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.RosterUpdates;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.RosterUpdates;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Dtos.Mapping.TestClasses;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Services;

public class MlbTheShowApiRosterUpdateFeedTests
{
    [Fact]
    public async Task GetNewRosterUpdates_Season_ReturnsRosterUpdatesForSeason()
    {
        /*
         * Arrange
         */
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);
        var rosterUpdateId1 = new RosterUpdateDto(1, "April 1, 2024");
        var rosterUpdateId2 = new RosterUpdateDto(2, "April 14, 2024");
        var rosterUpdateId3 = new RosterUpdateDto(3, "April 28, 2024");
        // MLB The Show returns newest first
        var getRosterUpdatesResponse = new GetRosterUpdatesResponse(new List<RosterUpdateDto>()
        {
            rosterUpdateId3, // Will be a new roster update
            rosterUpdateId2, // Will be a new roster update
            rosterUpdateId1 // Has already been completed by the domain
        });
        // Roster Update 2 data from MLB The Show
        var externalRosterUpdate2 = Faker.FakeGetRosterUpdateResponse();
        var externalRosterUpdate3 = Faker.FakeGetRosterUpdateResponse();

        var stubMemoryCache = new Mock<IMemoryCache>();
        object? t = true;
        object? f = false;
        // Roster Update 1 is already complete
        stubMemoryCache.Setup(x => x.TryGetValue(new DateOnly(2024, 4, 1), out t))
            .Returns(true);
        // Roster Update 2 is not yet complete
        stubMemoryCache.Setup(x => x.TryGetValue(new DateOnly(2024, 4, 14), out f))
            .Returns(false);
        // Roster Update 3 is not yet complete
        stubMemoryCache.Setup(x => x.TryGetValue(new DateOnly(2024, 4, 28), out f))
            .Returns(false);

        var stubMlbTheShowApi = new Mock<IMlbTheShowApi>();
        // Returns all roster updates
        stubMlbTheShowApi.Setup(x => x.GetRosterUpdates())
            .ReturnsAsync(getRosterUpdatesResponse);
        // Gets Roster Update 2 data from MLB The Show
        stubMlbTheShowApi.Setup(x => x.GetRosterUpdate(new GetRosterUpdateRequest(2)))
            .ReturnsAsync(externalRosterUpdate2);
        // Gets Roster Update 3 data from MLB The Show
        stubMlbTheShowApi.Setup(x => x.GetRosterUpdate(new GetRosterUpdateRequest(3)))
            .ReturnsAsync(externalRosterUpdate3);

        // Mock the MLB The Show client factory to return the stubbed client
        var stubMlbTheShowApiFactory = new Mock<IMlbTheShowApiFactory>();
        stubMlbTheShowApiFactory.Setup(x => x.GetClient(Year.Season2024))
            .Returns(stubMlbTheShowApi.Object);

        // Mocks mapping Roster Update 2 to application-level DTOs
        var stubRosterUpdateMapper = new Mock<IMlbTheShowRosterUpdateMapper>();
        // The mapped Roster Update 2
        var expectedRosterUpdate2 = Dtos.TestClasses.Faker.FakeRosterUpdate(rosterUpdateId2.Date);
        stubRosterUpdateMapper.Setup(x => x.Map(rosterUpdateId2, externalRosterUpdate2))
            .Returns(expectedRosterUpdate2);
        // The mapped Roster Update 3
        var expectedRosterUpdate3 = Dtos.TestClasses.Faker.FakeRosterUpdate(rosterUpdateId3.Date);
        stubRosterUpdateMapper.Setup(x => x.Map(rosterUpdateId3, externalRosterUpdate3))
            .Returns(expectedRosterUpdate3);

        var feed = new MlbTheShowApiRosterUpdateFeed(stubMlbTheShowApiFactory.Object, stubRosterUpdateMapper.Object,
            stubMemoryCache.Object);

        /*
         * Act
         */
        var actual = await feed.GetNewRosterUpdates(seasonYear, cToken);

        /*
         * Assert
         */
        // No data should have been requested from MLB The Show for Roster Update 1
        stubMlbTheShowApi.Verify(x => x.GetRosterUpdate(new GetRosterUpdateRequest(1)), Times.Never);
        // Data for Roster Update 2 should have been requested from MLB The Show
        stubMlbTheShowApi.Verify(x => x.GetRosterUpdate(new GetRosterUpdateRequest(2)), Times.Once);
        // Data for Roster Update 3 should have been requested from MLB The Show
        stubMlbTheShowApi.Verify(x => x.GetRosterUpdate(new GetRosterUpdateRequest(3)), Times.Once);
        // There should be only two roster updates that need to be applied
        var actualList = actual.OldToNew.ToList();
        Assert.Equal(2, actualList.Count);
        Assert.Equal(new DateOnly(2024, 4, 14), actualList[0].Date);
        Assert.Equal(new DateOnly(2024, 4, 28), actualList[1].Date);
    }

    [Fact]
    public async Task CompleteRosterUpdate_RosterUpdate_SetsRosterUpdateAsComplete()
    {
        // Arrange
        var cToken = CancellationToken.None;
        object key = new DateOnly(2024, 4, 1);
        var mockMemoryCache = new Mock<IMemoryCache>();
        mockMemoryCache.Setup(x => x.CreateEntry(key))
            .Returns(Mock.Of<ICacheEntry>());
        var feed = new MlbTheShowApiRosterUpdateFeed(Mock.Of<IMlbTheShowApiFactory>(),
            Mock.Of<IMlbTheShowRosterUpdateMapper>(), mockMemoryCache.Object);
        var rosterUpdate = Dtos.TestClasses.Faker.FakeRosterUpdate(new DateOnly(2024, 4, 1));

        // Act
        await feed.CompleteRosterUpdate(rosterUpdate, cToken);

        // Assert
        mockMemoryCache.Verify(x => x.CreateEntry(new DateOnly(2024, 4, 1)), Times.Once);
    }
}