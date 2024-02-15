using com.brettnamba.MlbTheShowForecaster.Common.Domain.SeedWork;
using com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Exceptions;

namespace com.brettnamba.MlbTheShowForecaster.Performance.Domain.Statistics.ValueObjects.Pitching;

/// <summary>
/// Models the result of a pitching a game
/// </summary>
public sealed class PitchingResult : ValueObject
{
    /// <summary>
    /// True if the pitcher got the win for this game
    /// </summary>
    public bool Win { get; }

    /// <summary>
    /// True if the pitcher got the loss for this game
    /// </summary>
    public bool Loss { get; }

    /// <summary>
    /// True if the pitcher started this game
    /// </summary>
    public bool GameStarted { get; }

    /// <summary>
    /// True if the pitcher was the last pitcher in the game as a relief pitcher
    /// </summary>
    public bool GameFinished { get; }

    /// <summary>
    /// True if the pitcher pitched the whole game
    /// </summary>
    public bool CompleteGame { get; }

    /// <summary>
    /// True if the pitcher pitched a shutout
    /// </summary>
    public bool Shutout { get; }

    /// <summary>
    /// True if the pitcher earned a hold
    /// </summary>
    public bool Hold { get; }

    /// <summary>
    /// True if the pitcher earned a save
    /// </summary>
    public bool Save { get; }

    /// <summary>
    /// True if the pitcher failed to earn a save
    /// </summary>
    public bool BlownSave { get; }

    /// <summary>
    /// True if this game was a save opportunity for the pitcher
    /// </summary>
    public bool SaveOpportunity { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="win">True if the pitcher got the win for this game</param>
    /// <param name="loss">True if the pitcher got the loss for this game</param>
    /// <param name="gameStarted">True if the pitcher started this game</param>
    /// <param name="gameFinished">True if the pitcher was the last pitcher in the game as a relief pitcher</param>
    /// <param name="completeGame">True if the pitcher pitched the whole game</param>
    /// <param name="shutout">True if the pitcher pitched a shutout</param>
    /// <param name="hold">True if the pitcher earned a hold</param>
    /// <param name="save">True if the pitcher earned a save</param>
    /// <param name="blownSave">True if the pitcher failed to earn a save</param>
    /// <param name="saveOpportunity">True if this game was a save opportunity for the pitcher</param>
    /// <exception cref="InvalidPitchingResultCombinationException">Thrown if an invalid combination of pitching results occurs</exception>
    private PitchingResult(bool win, bool loss, bool gameStarted, bool gameFinished, bool completeGame, bool shutout,
        bool hold, bool save, bool blownSave, bool saveOpportunity)
    {
        if (win && loss)
        {
            throw new InvalidPitchingResultCombinationException("A pitcher cannot receive both a win and loss");
        }

        if (gameStarted && (hold || save || blownSave || saveOpportunity))
        {
            throw new InvalidPitchingResultCombinationException(
                "A pitcher who starts a game is not eligible for a save");
        }

        if (gameFinished && (completeGame || gameStarted))
        {
            throw new InvalidPitchingResultCombinationException(
                "A relief pitcher who finished a game means they did not start the game or pitch a complete game");
        }

        if (shutout && (loss || !win))
        {
            throw new InvalidPitchingResultCombinationException(
                "If a shutout was pitched, it means the pitcher did not receive a loss");
        }

        if ((hold || save) && win)
        {
            throw new InvalidPitchingResultCombinationException(
                "A pitcher cannot receive a win and a hold/save in the same game");
        }

        Win = win;
        Loss = loss;
        GameStarted = gameStarted;
        GameFinished = gameFinished;
        CompleteGame = completeGame;
        Shutout = shutout;
        Hold = hold;
        Save = save;
        BlownSave = blownSave;
        SaveOpportunity = saveOpportunity;
    }

    /// <summary>
    /// Creates <see cref="PitchingResult"/>
    /// </summary>
    /// <param name="win">True if the pitcher got the win for this game</param>
    /// <param name="loss">True if the pitcher got the loss for this game</param>
    /// <param name="gameStarted">True if the pitcher started this game</param>
    /// <param name="gameFinished">True if the pitcher was the last pitcher in the game as a relief pitcher</param>
    /// <param name="completeGame">True if the pitcher pitched the whole game</param>
    /// <param name="shutout">True if the pitcher pitched a shutout</param>
    /// <param name="hold">True if the pitcher earned a hold</param>
    /// <param name="save">True if the pitcher earned a save</param>
    /// <param name="blownSave">True if the pitcher failed to earn a save</param>
    /// <param name="saveOpportunity">True if this game was a save opportunity for the pitcher</param>
    /// <returns><see cref="PitchingResult"/></returns>
    public static PitchingResult Create(bool win, bool loss, bool gameStarted, bool gameFinished, bool completeGame,
        bool shutout, bool hold, bool save, bool blownSave, bool saveOpportunity)
    {
        return new PitchingResult(win: win, loss: loss, gameStarted: gameStarted, gameFinished: gameFinished,
            completeGame: completeGame, shutout: shutout, hold: hold, save: save, blownSave: blownSave,
            saveOpportunity: saveOpportunity);
    }
}