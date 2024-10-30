using com.brettnamba.MlbTheShowForecaster.Common.DateAndTime;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.Enums;
using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Commands.UpdatePlayerCard.Exceptions;
using com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.TestClasses;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Entities;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.Repositories;
using com.brettnamba.MlbTheShowForecaster.GameCards.Domain.Cards.ValueObjects.PlayerCards;
using Moq;

namespace com.brettnamba.MlbTheShowForecaster.GameCards.Application.Tests.Commands.UpdatePlayerCard;

public class UpdatePlayerCardCommandHandlerTests
{
    [Fact]
    public async Task Handle_MissingPlayerCard_ThrowsException()
    {
        // Arrange
        var playerCard = Faker.FakePlayerCard();
        var externalPlayerCard = Dtos.TestClasses.Faker.FakeMlbPlayerCard();
        var ratingChange = Dtos.TestClasses.Faker.FakePlayerRatingChange();
        var positionChange = Dtos.TestClasses.Faker.FakePlayerPositionChange();

        var stubPlayerCardRepository = new Mock<IPlayerCardRepository>();
        stubPlayerCardRepository.Setup(x => x.GetByExternalId(playerCard.ExternalId))
            .ReturnsAsync(null as PlayerCard);

        var stubUnitOfWork = new Mock<IUnitOfWork<ICardWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IPlayerCardRepository>())
            .Returns(stubPlayerCardRepository.Object);

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerCardCommand(playerCard, externalPlayerCard, ratingChange, positionChange);
        var handler = new UpdatePlayerCardCommandHandler(stubUnitOfWork.Object, Mock.Of<ICalendar>());

        var action = () => handler.Handle(command, cToken);

        // Act
        var actual = await Record.ExceptionAsync(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<PlayerCardNotFoundException>(actual);
    }

    [Fact]
    public async Task Handle_UpdatePlayerCardCommand_UpdatesPlayerCard()
    {
        /*
         * Arrange
         */
        // The PlayerCard that is included in the command
        var playerCardInCommand = Faker.FakePlayerCard(overallRating: 80, position: Position.RightField);
        // Add the original state to the history
        var originalState = Faker.FakeBaselinePlayerCardHistoricalRating(
            startDate: new DateOnly(2024, 1, 1), endDate: null);
        // Add the current state
        var currentState = Faker.FakeBaselinePlayerCardHistoricalRating(
            startDate: new DateOnly(2024, 4, 1), endDate: null);
        var historicalRatings = new Stack<PlayerCardHistoricalRating>();
        // Newest are in the stack first
        historicalRatings.Push(currentState);
        historicalRatings.Push(originalState);

        // The card data from the external card catalog
        var externalPlayerCard = Dtos.TestClasses.Faker.FakeMlbPlayerCard(speed: 20);

        // The rating change that will be applied to the card
        var ratingChangeDate = new DateOnly(2024, 5, 29);
        var ratingChange = Dtos.TestClasses.Faker.FakePlayerRatingChange(date: ratingChangeDate, newOverallRating: 90);

        // The card is changing its position
        var positionChange = Dtos.TestClasses.Faker.FakePlayerPositionChange(newPosition: Position.FirstBase);

        // Returns the matching card from the repository
        var stubPlayerCardRepository = new Mock<IPlayerCardRepository>();
        var playerCard = Faker.FakePlayerCard(overallRating: 80, position: Position.RightField);
        stubPlayerCardRepository.Setup(x => x.GetByExternalId(playerCardInCommand.ExternalId))
            .ReturnsAsync(playerCard);

        // Unit of work
        var stubUnitOfWork = new Mock<IUnitOfWork<ICardWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IPlayerCardRepository>())
            .Returns(stubPlayerCardRepository.Object);

        // The command and handler
        var cToken = CancellationToken.None;
        var command = new UpdatePlayerCardCommand(playerCardInCommand, externalPlayerCard, ratingChange, positionChange,
            historicalRatings);
        var handler = new UpdatePlayerCardCommandHandler(stubUnitOfWork.Object, Mock.Of<ICalendar>());

        /*
         * Act
         */
        await handler.Handle(command, cToken);

        /*
         * Assert
         */
        // The repository should have updated the PlayerCard
        stubPlayerCardRepository.Verify(x => x.Update(playerCard), Times.Once);
        // The unit of work should have committed the changes
        stubUnitOfWork.Verify(x => x.CommitAsync(cToken), Times.Once);
        // The rating and attributes should have been updated
        Assert.Equal(90, playerCard.OverallRating.Value);
        Assert.Equal(20, playerCard.PlayerCardAttributes.Speed.Value);
        // The position should have been updated
        Assert.Equal(Position.FirstBase, playerCard.Position);
        // The historical ratings should have been updated
        Assert.Equal(3, playerCard.HistoricalRatingsChronologically.Count);
        // The states that previously had no end date should now be equal to start date of the following historical rating
        originalState.End(new DateOnly(2024, 4, 1));
        currentState.End(new DateOnly(2024, 5, 29));
        Assert.Equal(originalState, playerCard.HistoricalRatingsChronologically[0]);
        Assert.Equal(currentState, playerCard.HistoricalRatingsChronologically[1]);
        // The most recent rating change should have ended the previous one and started a new one
        var newRating = playerCard.HistoricalRatingsChronologically[2];
        Assert.Equal(new DateOnly(2024, 5, 29), newRating.StartDate);
        Assert.Null(newRating.EndDate);
    }

    [Fact]
    public async Task Handle_BoostedExternalCard_BoostsPlayerCard()
    {
        // Arrange
        var domainPlayerCard = Faker.FakePlayerCard();
        var externalPlayerCard = Dtos.TestClasses.Faker.FakeMlbPlayerCard(boostReason: "Hit 5 HRs");

        var (stubUnitOfWork, stubPlayerCardRepository) = GetStubUnitOfWorkForCard(domainPlayerCard);

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerCardCommand(domainPlayerCard, externalPlayerCard);
        var handler = new UpdatePlayerCardCommandHandler(stubUnitOfWork.Object, Mock.Of<ICalendar>());

        // Act
        await handler.Handle(command, cToken);

        // Assert
        stubPlayerCardRepository.Verify(x => x.Update(It.Is<PlayerCard>(c => c.IsBoosted)), Times.Once);
    }

    [Fact]
    public async Task Handle_ExternalCardNoLongerBoosted_RemovesBoostFromPlayerCard()
    {
        // Arrange
        var domainPlayerCard = Faker.FakePlayerCard(isBoosted: true);
        var externalPlayerCard = Dtos.TestClasses.Faker.FakeMlbPlayerCard(boostReason: null);

        var (stubUnitOfWork, stubPlayerCardRepository) = GetStubUnitOfWorkForCard(domainPlayerCard);

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerCardCommand(domainPlayerCard, externalPlayerCard);
        var handler = new UpdatePlayerCardCommandHandler(stubUnitOfWork.Object, Mock.Of<ICalendar>());

        // Act
        await handler.Handle(command, cToken);

        // Assert
        stubPlayerCardRepository.Verify(x => x.Update(It.Is<PlayerCard>(c => c.IsBoosted == false)), Times.Once);
    }

    [Fact]
    public async Task Handle_TempRatingExternalCard_AddsTempRatingToPlayerCard()
    {
        // Arrange
        var domainPlayerCard = Faker.FakePlayerCard();
        var externalPlayerCard = Dtos.TestClasses.Faker.FakeMlbPlayerCard(temporaryOverallRating: 50);

        var (stubUnitOfWork, stubPlayerCardRepository) = GetStubUnitOfWorkForCard(domainPlayerCard);

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerCardCommand(domainPlayerCard, externalPlayerCard);
        var handler = new UpdatePlayerCardCommandHandler(stubUnitOfWork.Object, Mock.Of<ICalendar>());

        // Act
        await handler.Handle(command, cToken);

        // Assert
        stubPlayerCardRepository.Verify(x => x.Update(It.Is<PlayerCard>(c => c.HasTemporaryRating)), Times.Once);
    }

    [Fact]
    public async Task Handle_ExternalCardNoLongerHasTempRating_RemovesTempRatingFromPlayerCard()
    {
        // Arrange
        var domainPlayerCard = Faker.FakePlayerCard(temporaryRating: 70);
        var externalPlayerCard = Dtos.TestClasses.Faker.FakeMlbPlayerCard();

        var (stubUnitOfWork, stubPlayerCardRepository) = GetStubUnitOfWorkForCard(domainPlayerCard);

        var cToken = CancellationToken.None;
        var command = new UpdatePlayerCardCommand(domainPlayerCard, externalPlayerCard);
        var handler = new UpdatePlayerCardCommandHandler(stubUnitOfWork.Object, Mock.Of<ICalendar>());

        // Act
        await handler.Handle(command, cToken);

        // Assert
        stubPlayerCardRepository.Verify(x => x.Update(It.Is<PlayerCard>(c => c.HasTemporaryRating == false)),
            Times.Once);
    }

    private (Mock<IUnitOfWork<ICardWork>>, Mock<IPlayerCardRepository>) GetStubUnitOfWorkForCard(PlayerCard card)
    {
        var stubPlayerCardRepository = new Mock<IPlayerCardRepository>();
        stubPlayerCardRepository.Setup(x => x.GetByExternalId(card.ExternalId))
            .ReturnsAsync(card);
        var stubUnitOfWork = new Mock<IUnitOfWork<ICardWork>>();
        stubUnitOfWork.Setup(x => x.GetContributor<IPlayerCardRepository>())
            .Returns(stubPlayerCardRepository.Object);
        return (stubUnitOfWork, stubPlayerCardRepository);
    }
}