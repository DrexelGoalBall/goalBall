using UnityEngine;
using System.Collections;

public class Possession : MonoBehaviour {

    /// <summary>
    /// This script will determine who has possession of the ball
    /// As a ball collides with a player of a certain team, the possession of the ball will change.
    /// This will allow other scripts to know the possesion of the ball properly as if it were
    /// a real game.
    /// </summary>

    //Player Tags
    public string bluePlayerTag = "BluePlayer";
    public string redPlayerTag = "RedPlayer";

    //Possesion Variables
    public string possessionOfBall = "Blue";

    /// <summary>
    /// Detects when a red or blue player collides with the ball and sets the possesion variable accordingly.
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == bluePlayerTag)
        {
            BlueTeamPossession();
        }
        if (col.gameObject.tag == redPlayerTag)
        {
            RedTeamPossession();
        }
    }

    /// <summary>
    /// Check who has posession of the ball.
    /// </summary>
    /// <returns></returns>
    public string HasPossessionOfBall()
    {
        return possessionOfBall;
    }

    /// <summary>
    /// Gives possesion of the ball to the blue team.
    /// </summary>
    public void BlueTeamPossession()
    {
        possessionOfBall = "Blue";
    }

    /// <summary>
    /// Gives possesion of the ball to the red team.
    /// </summary>
    public void RedTeamPossession()
    {
        possessionOfBall = "Red";
    }
}
