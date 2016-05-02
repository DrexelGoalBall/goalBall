using UnityEngine;
using System.Collections;

/// <summary>
///     Break to occur when the game ends
/// </summary>
public class GameEndBreak : GameBreak
{
    /// <summary>
    ///     Default constructor to set the break length
    /// </summary>
    public GameEndBreak()
    {
        breakLength = 43;
    }

    /// <summary>
    ///     Actions to perform when the break begins
    ///     -- No actions necessary for start of the end game break
    /// </summary>
    public override void StartOfBreakActions()
    {

    }

    /// <summary>
    ///     Actions to perform when the break ends
    ///     -- Automatically exit the game when break is finished
    /// </summary>
    public override void EndOfBreakActions()
    {
        GameEnd gameEnd = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameEnd>();
        gameEnd.LeaveGame();
    }
}