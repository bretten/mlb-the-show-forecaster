using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.CreatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Dtos.Mapping;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using Moq;
using Faker = com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.TestClasses.Faker;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Commands.CreatePlayerCard;

public class CreatePlayerCardCommandHandlerTests
{
    [Fact]
    public async Task Handle_CreatePlayerCardCommand_CreatesPlayerCard()
    {
        // Arrange
        var fakeExternalPlayerCard = Dtos.TestClasses.Faker.FakeMlbPlayerCard();
        var fakeDomainPlayerCard = Faker.FakePlayerCard();

        var stubPlayerSeasonMapper =
            Mock.Of<IPlayerCardMapper>(x => x.Map(fakeExternalPlayerCard) == fakeDomainPlayerCard);

        var mockPlayerCardRepository = Mock.Of<IPlayerCardRepository>();
        var mockUnitOfWork = Mock.Of<IUnitOfWork>();

        var cToken = CancellationToken.None;
        var command = new CreatePlayerCardCommand(fakeExternalPlayerCard);
        var handler =
            new CreatePlayerCardCommandHandler(stubPlayerSeasonMapper, mockPlayerCardRepository, mockUnitOfWork);

        // Act
        await handler.Handle(command, cToken);

        // Assert
        Mock.Get(mockPlayerCardRepository).Verify(x => x.Add(fakeDomainPlayerCard), Times.Once);
        Mock.Get(mockUnitOfWork).Verify(x => x.CommitAsync(cToken), Times.Once);
    }
}