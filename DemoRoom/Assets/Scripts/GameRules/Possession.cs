using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
/// This script will determine who has possession of the ball
/// As a ball collides with a player of a certain team, the possession of the ball will change.
/// This will allow other scripts to know the possesion of the ball properly as if it were
/// a real game.
/// </summary>
public class Possession : NetworkBehaviour {

    public enum Team
    {
        red = 0,
        blue = 1,
    }

    //Player Tags
    public string bluePlayerTag = "BluePlayer";
    public string redPlayerTag = "RedPlayer";

    //Possesion Variables
    [SyncVar]
    public Team possessionOfBall = Team.blue;

    private Team nextToGetBall;

    /// <summary>
    /// Detects when a red or blue player collides with the ball and sets the possesion variable accordingly.
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        /*if (col.gameObject.tag == bluePlayerTag)
        {
            BlueTeamPossession();
        }
        if (col.gameObject.tag == redPlayerTag)
        {
            RedTeamPossession();
        }*/
    }

    /// <summary>
    /// Check who has posession of the ball.
    /// </summary>
    /// <returns></returns>
    public Team HasPossessionOfBall()
    {
        return possessionOfBall;
    }

    /// <summary>
    /// Gives possesion of the ball to the blue team.
    /// </summary>
    public void BlueTeamPossession()
    {
        if (isServer)
        {
            possessionOfBall = Team.blue;
        }
    }

    /// <summary>
    /// Gives possesion of the ball to the red team.
    /// </summary>
    public void RedTeamPossession()
    {
        if (isServer)
        {
            possessionOfBall = Team.red;
        }
    }

    /// <summary>
    /// Retrieves who should get the ball next half
    /// </summary>
    public Team GetNextToGetBall()
    {
        return nextToGetBall;
    }

    /// <summary>
    /// Set the team who should get the ball next half
    /// </summary>
    /// <param name="team">Team that should get the ball</param>
    public void SetNextToGetBall(Team team)
    {
        nextToGetBall = team;
    }
}
