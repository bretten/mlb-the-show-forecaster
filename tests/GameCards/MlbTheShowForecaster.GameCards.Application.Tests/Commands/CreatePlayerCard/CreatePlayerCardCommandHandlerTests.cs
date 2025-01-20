using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCard.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Dtos.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Commands.CreatePlayerCard;

public class CreatePlayerCardCommandHandlerTests
{
    [Fact]
    public async Task Handle_PlayerCardExists_ThrowsException()
    {
        // Arrange
        var fakeExternalPlayerCard = Faker.FakeMlbPlayerCard();

        var mockPlayerSeasonMapper = Mock.Of<IPlayerCardMapper>();

        var stubPlayerCardRepository = new Mock<IPlayerCardRepository>();
        stubPlayerCardRepository.Setup(x => x.Exists(fakeExternalPlayerCard.ExternalUuid))
            .ReturnsAsync(true);

        var stubUnitOfWork = new Mock<IUnitOfWork<ICardWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IPlayerCardRepository>())
            .Returns(stubPlayerCardRepository.Object);

        var cToken = CancellationToken.None;
        var command = new CreatePlayerCardCommand(fakeExternalPlayerCard);
        var handler =
            new CreatePlayerCardCommandHandler(stubUnitOfWork.Object, mockPlayerSeasonMapper);

        var action = async () => await handler.Handle(command, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<PlayerCardAlreadyExistsException>(actual);
    }

    [Fact]
    public async Task Handle_CreatePlayerCardCommand_CreatesPlayerCard()
    {
        // Arrange
        var fakeExternalPlayerCard = Faker.FakeMlbPlayerCard();
        var fakeDomainPlayerCard = Domain.Tests.Cards.TestClasses.Faker.FakePlayerCard();

        var stubPlayerSeasonMapper =
            Mock.Of<IPlayerCardMapper>(x => x.Map(fakeExternalPlayerCard) == fakeDomainPlayerCard);

        var mockPlayerCardRepository = Mock.Of<IPlayerCardRepository>();
        var stubUnitOfWork = new Mock<IUnitOfWork<ICardWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IPlayerCardRepository>())
            .Returns(mockPlayerCardRepository);

        var cToken = CancellationToken.None;
        var command = new CreatePlayerCardCommand(fakeExternalPlayerCard);
        var handler =
            new CreatePlayerCardCommandHandler(stubUnitOfWork.Object, stubPlayerSeasonMapper);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Mock.Get(mockPlayerCardRepository).Verify(x => x.Add(fakeDomainPlayerCard), Times.Once);
        stubUnitOfWork.Verify(x => x.CommitAsync(cToken), Times.Once);
    }
}