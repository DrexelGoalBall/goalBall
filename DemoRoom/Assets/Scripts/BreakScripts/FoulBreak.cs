using UnityEngine;
using System.Collections;

/// <summary>
///     Break to occur after a foul is committed
/// </summary>
public class FoulBreak : GameBreak
{
    /// <summary>
    ///     Default constructor to set the break length
    /// </summary>
    public FoulBreak()
    {
        breakLength = 4;
    }

    /// <summary>
    ///     Actions to perform when the break begins
    ///     -- No actions necessary for start of a foul break
    /// </summary>
    public override void StartOfBreakActions()
    {

    }

    /// <summary>
    ///     Actions to perform when the break ends
    ///     -- Announce who possesses the ball and that play has continued
    /// </summary>
    public override void EndOfBreakActions()
    {
        if (ball.GetComponent<Possession>().HasPossessionOfBall() == Possession.Team.red)
            referee.PlayRedTeam();
        else
            referee.PlayBlueTeam();
        referee.PlayPlay();
    }
}