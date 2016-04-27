using UnityEngine;
using System.Collections;

/// <summary>
///     Break to occur when a goal is scored
/// </summary>
public class GoalBreak : GameBreak
{
    /// <summary>
    ///     Default constructor to set the break length
    /// </summary>
    public GoalBreak()
    {
        breakLength = 2;
    }

    /// <summary>
    ///     Actions to perform when the break begins
    ///     -- No actions necessary for start of the goal break
    /// </summary>
    public override void StartOfBreakActions()
    {

    }

    /// <summary>
    ///     Actions to perform when the break ends
    ///     -- Announce that play has continued
    /// </summary>
    public override void EndOfBreakActions()
    {
        referee.PlayPlay();
    }
}