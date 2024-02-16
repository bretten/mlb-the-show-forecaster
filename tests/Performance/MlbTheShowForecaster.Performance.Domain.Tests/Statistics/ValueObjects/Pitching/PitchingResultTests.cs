using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Exceptions;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Tests.Statistics.ValueObjects.Pitching;

public class PitchingResultTests
{
    [Fact]
    public void Constructor_WinAndLoss_ThrowsException()
    {
        // Arrange
        const bool win = true;
        const bool loss = true;
        const bool gameStarted = false;
        const bool gameFinished = false;
        const bool completeGame = false;
        const bool shutout = false;
        const bool hold = false;
        const bool save = false;
        const bool blownSave = false;
        const bool saveOpportunity = false;
        var action = () => PitchingResult.Create(win: win,
            loss: loss,
            gameStarted: gameStarted,
            gameFinished: gameFinished,
            completeGame: completeGame,
            shutout: shutout,
            hold: hold,
            save: save,
            blownSave: blownSave,
            saveOpportunity: saveOpportunity);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidPitchingResultCombinationException>(actual);
        Assert.Equal("A pitcher cannot receive both a win and loss", actual.Message);
    }

    [Fact]
    public void Constructor_GameStartedAndSave_ThrowsException()
    {
        // Arrange
        const bool win = false;
        const bool loss = false;
        const bool gameStarted = true;
        const bool gameFinished = false;
        const bool completeGame = false;
        const bool shutout = false;
        const bool hold = true;
        const bool save = true;
        const bool blownSave = true;
        const bool saveOpportunity = true;
        var action = () => PitchingResult.Create(win: win,
            loss: loss,
            gameStarted: gameStarted,
            gameFinished: gameFinished,
            completeGame: completeGame,
            shutout: shutout,
            hold: hold,
            save: save,
            blownSave: blownSave,
            saveOpportunity: saveOpportunity);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidPitchingResultCombinationException>(actual);
        Assert.Equal("A pitcher who starts a game is not eligible for a save", actual.Message);
    }

    [Fact]
    public void Constructor_GameFinishedAndStartedOrCompleted_ThrowsException()
    {
        // Arrange
        const bool win = false;
        const bool loss = false;
        const bool gameStarted = true;
        const bool gameFinished = true;
        const bool completeGame = true;
        const bool shutout = false;
        const bool hold = false;
        const bool save = false;
        const bool blownSave = false;
        const bool saveOpportunity = false;
        var action = () => PitchingResult.Create(win: win,
            loss: loss,
            gameStarted: gameStarted,
            gameFinished: gameFinished,
            completeGame: completeGame,
            shutout: shutout,
            hold: hold,
            save: save,
            blownSave: blownSave,
            saveOpportunity: saveOpportunity);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidPitchingResultCombinationException>(actual);
        Assert.Equal("A relief pitcher who finished a game means they did not start the game or pitch a complete game",
            actual.Message);
    }

    [Fact]
    public void Constructor_ShutoutAndLoss_ThrowsException()
    {
        // Arrange
        const bool win = false;
        const bool loss = true;
        const bool gameStarted = false;
        const bool gameFinished = false;
        const bool completeGame = false;
        const bool shutout = true;
        const bool hold = false;
        const bool save = false;
        const bool blownSave = false;
        const bool saveOpportunity = false;
        var action = () => PitchingResult.Create(win: win,
            loss: loss,
            gameStarted: gameStarted,
            gameFinished: gameFinished,
            completeGame: completeGame,
            shutout: shutout,
            hold: hold,
            save: save,
            blownSave: blownSave,
            saveOpportunity: saveOpportunity);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidPitchingResultCombinationException>(actual);
        Assert.Equal("If a shutout was pitched, it means the pitcher did not receive a loss", actual.Message);
    }

    [Fact]
    public void Constructor_WinAndSave_ThrowsException()
    {
        // Arrange
        const bool win = true;
        const bool loss = false;
        const bool gameStarted = false;
        const bool gameFinished = false;
        const bool completeGame = false;
        const bool shutout = false;
        const bool hold = true;
        const bool save = true;
        const bool blownSave = false;
        const bool saveOpportunity = false;
        var action = () => PitchingResult.Create(win: win,
            loss: loss,
            gameStarted: gameStarted,
            gameFinished: gameFinished,
            completeGame: completeGame,
            shutout: shutout,
            hold: hold,
            save: save,
            blownSave: blownSave,
            saveOpportunity: saveOpportunity);

        // Act
        var actual = Record.Exception(action);

        // Assert
        Assert.NotNull(actual);
        Assert.IsType<InvalidPitchingResultCombinationException>(actual);
        Assert.Equal("A pitcher cannot receive a win and a hold/save in the same game", actual.Message);
    }
}