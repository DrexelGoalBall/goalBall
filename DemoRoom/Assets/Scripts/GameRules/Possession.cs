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

    public string HasPossessionOfBall()
    {
        return possessionOfBall;
    }

    public void BlueTeamPossession()
    {
        possessionOfBall = "Blue";
    }
    public void RedTeamPossession()
    {
        possessionOfBall = "Red";
    }
}
