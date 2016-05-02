using UnityEngine;
using System.Collections;

/// <summary>
///     Break to occur when it is halftime
/// </summary>
public class HalftimeBreak : GameBreak
{
    /// <summary>
    ///     Default constructor to set the break length
    /// </summary>
    public HalftimeBreak()
    {
        breakLength = 10;
    }

    /// <summary>
    ///     Actions to perform when the break begins
    ///     -- Announce that it is halftime and the current scores
    /// </summary>
    public override void StartOfBreakActions()
    {
        referee.PlayHalfTime();

        ScoreKeeper scoreKeeper = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreKeeper>();
        // Read red score
        referee.PlayRedTeam();
        referee.ReadScore(scoreKeeper.RedScore());
        // Read blue score
        referee.PlayBlueTeam();
        referee.ReadScore(scoreKeeper.BlueScore());
    }

    /// <summary>
    ///     Actions to perform when the break ends
    ///     -- Announce who possesses the ball and that play has continued
    /// </summary>
    public override void EndOfBreakActions()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball.GetComponent<Possession>().HasPossessionOfBall() == Possession.Team.red)
            referee.PlayRedTeam();
        else
            referee.PlayBlueTeam();
        referee.PlayPlay();
    }
}