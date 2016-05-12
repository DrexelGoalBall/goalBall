using UnityEngine;
using System.Collections;

/// <summary>
///     Base class for different types of breaks
/// </summary>
public abstract class GameBreak
{
    // Reference for Referee script
    protected Referee referee;

    // Reference to the ball GameObject
    protected GameObject ball;

    // Number of seconds for the break to last
    protected int breakLength;

    /// <summary>
    ///     Default constructor to initialize objects/scripts
    /// </summary>
    public GameBreak()
    {
        referee = GameObject.FindGameObjectWithTag("Referee").GetComponent<Referee>();
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    /// <summary>
    ///     Allows outside classes to access the length for this break
    /// </summary>
    /// <returns>Number of seconds for this break</returns>
    public int GetBreakLength()
    {
        return breakLength;
    }

    /// <summary>
    ///     Actions to perform when the break begins
    /// </summary>
    public abstract void StartOfBreakActions();

    /// <summary>
    ///     Actions to perform when the break ends
    /// </summary>
    public abstract void EndOfBreakActions();
}
