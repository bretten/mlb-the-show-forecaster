using com.brettnamba.MlbTheShowForecaster.Common.Domain.ValueObjects;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Enums;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Dtos.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Requests.Items;
using com.brettnamba.MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Responses.Items;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Services.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Services;
using com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Dtos.Mapping.TestClasses;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Infrastructure.Tests.Services;

public class MlbTheShowApiCardCatalogTests
{
    [Fact]
    public async Task GetActiveRosterMlbPlayerCards_SeasonWithNoActiveRosterCards_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);

        var stubMlbTheShowApi = new Mock<IMlbTheShowApi>();
        stubMlbTheShowApi.Setup(x => x.GetItems(new GetItemsRequest(1, ItemType.MlbCard)))
            .ReturnsAsync(new GetItemsPaginatedResponse(1, 1, 1, new List<ItemDto>()));

        var stubMlbTheShowApiFactory = new Mock<IMlbTheShowApiFactory>();
        stubMlbTheShowApiFactory.Setup(x => x.GetClient(Year.Season2024))
            .Returns(stubMlbTheShowApi.Object);

        var mockItemMapper = Mock.Of<IMlbTheShowItemMapper>();

        var catalog = new MlbTheShowApiCardCatalog(stubMlbTheShowApiFactory.Object, mockItemMapper);

        var action = () => catalog.GetActiveRosterMlbPlayerCards(seasonYear, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<ActiveRosterMlbPlayerCardsNotFoundInCatalogException>(actual);
    }

    [Fact]
    public async Task GetActiveRosterMlbPlayerCards_SeasonYear_ReturnsCardsForSeason()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);

        var cardDto1 = Faker.FakeMlbCardDto(uuid: Faker.FakeGuid1);
        var cardDto2 = Faker.FakeMlbCardDto(uuid: Faker.FakeGuid2);
        var cardDto3 = Faker.FakeMlbCardDto(uuid: Faker.FakeGuid3);

        var externalCard1 = Dtos.TestClasses.Faker.FakeMlbPlayerCard(cardExternalId: cardDto1.Uuid.Value);
        var externalCard2 = Dtos.TestClasses.Faker.FakeMlbPlayerCard(cardExternalId: cardDto2.Uuid.Value);
        var externalCard3 = Dtos.TestClasses.Faker.FakeMlbPlayerCard(cardExternalId: cardDto3.Uuid.Value);

        var stubMlbTheShowApi = new Mock<IMlbTheShowApi>();
        stubMlbTheShowApi.Setup(x => x.GetItems(new GetItemsRequest(1, ItemType.MlbCard)))
            .ReturnsAsync(new GetItemsPaginatedResponse(1, 1, 3, new List<ItemDto>()
            {
                cardDto1
            }));
        stubMlbTheShowApi.Setup(x => x.GetItems(new GetItemsRequest(2, ItemType.MlbCard)))
            .ReturnsAsync(new GetItemsPaginatedResponse(2, 1, 3, new List<ItemDto>()
            {
                cardDto2
            }));
        stubMlbTheShowApi.Setup(x => x.GetItems(new GetItemsRequest(3, ItemType.MlbCard)))
            .ReturnsAsync(new GetItemsPaginatedResponse(3, 1, 3, new List<ItemDto>()
            {
                cardDto3
            }));
        stubMlbTheShowApi.Setup(x => x.GetItems(new GetItemsRequest(4, ItemType.MlbCard)))
            .ReturnsAsync(new GetItemsPaginatedResponse(4, 1, 3, new List<ItemDto>()));

        var stubMlbTheShowApiFactory = new Mock<IMlbTheShowApiFactory>();
        stubMlbTheShowApiFactory.Setup(x => x.GetClient(Year.Season2024))
            .Returns(stubMlbTheShowApi.Object);

        var stubItemMapper = new Mock<IMlbTheShowItemMapper>();
        stubItemMapper.Setup(x => x.Map(seasonYear, cardDto1))
            .Returns(externalCard1);
        stubItemMapper.Setup(x => x.Map(seasonYear, cardDto2))
            .Returns(externalCard2);
        stubItemMapper.Setup(x => x.Map(seasonYear, cardDto3))
            .Returns(externalCard3);

        var catalog = new MlbTheShowApiCardCatalog(stubMlbTheShowApiFactory.Object, stubItemMapper.Object);

        // Act
        var actual = await catalog.GetActiveRosterMlbPlayerCards(seasonYear, cToken);

        // Assert
        Assert.Equal(new List<MlbPlayerCard>() { externalCard1, externalCard2, externalCard3 }, actual);
    }

    [Fact]
    public async Task GetMlbPlayerCard_CardExternalIdWithNoMatch_ThrowsException()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);

        var cardExternalId = Dtos.TestClasses.Faker.FakeCardExternalId(Dtos.TestClasses.Faker.FakeGuid1);

        var stubMlbTheShowApi = new Mock<IMlbTheShowApi>();
        stubMlbTheShowApi.Setup(x => x.GetItem(new GetItemRequest(cardExternalId.ValueStringDigits)))
            .ReturnsAsync((ItemDto)null!);

        var stubMlbTheShowApiFactory = new Mock<IMlbTheShowApiFactory>();
        stubMlbTheShowApiFactory.Setup(x => x.GetClient(Year.Season2024))
            .Returns(stubMlbTheShowApi.Object);

        var mockItemMapper = Mock.Of<IMlbTheShowItemMapper>();

        var catalog = new MlbTheShowApiCardCatalog(stubMlbTheShowApiFactory.Object, mockItemMapper);

        var action = () => catalog.GetMlbPlayerCard(seasonYear, cardExternalId, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<MlbPlayerCardNotFoundInCatalogException>(actual);
    }

    [Fact]
    public async Task GetMlbPlayerCard_SeasonYearAndCardExternalId_ReturnsCard()
    {
        // Arrange
        var cToken = CancellationToken.None;
        var seasonYear = SeasonYear.Create(2024);

        var cardExternalId = Dtos.TestClasses.Faker.FakeCardExternalId(Dtos.TestClasses.Faker.FakeGuid1);
        var cardDto = Faker.FakeMlbCardDto(uuid: cardExternalId.Value);

        var externalCard1 = Dtos.TestClasses.Faker.FakeMlbPlayerCard(cardExternalId: cardDto.Uuid.Value);

        var stubMlbTheShowApi = new Mock<IMlbTheShowApi>();
        stubMlbTheShowApi.Setup(x => x.GetItem(new GetItemRequest(cardDto.Uuid.ValueAsString)))
            .ReturnsAsync(cardDto);

        var stubMlbTheShowApiFactory = new Mock<IMlbTheShowApiFactory>();
        stubMlbTheShowApiFactory.Setup(x => x.GetClient(Year.Season2024))
            .Returns(stubMlbTheShowApi.Object);

        var stubItemMapper = new Mock<IMlbTheShowItemMapper>();
        stubItemMapper.Setup(x => x.Map(seasonYear, cardDto))
            .Returns(externalCard1);

        var catalog = new MlbTheShowApiCardCatalog(stubMlbTheShowApiFactory.Object, stubItemMapper.Object);

        // Act
        var actual = await catalog.GetMlbPlayerCard(seasonYear, cardExternalId, cToken);

        // Assert
        Assert.Equal(externalCard1, actual);
    }
}