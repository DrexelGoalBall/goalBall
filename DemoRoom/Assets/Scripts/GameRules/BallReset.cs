using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This script resets the ball to the proper position.  It will put the ball infront of one of the players on the team who is going to get the ball.
/// This could be due to a penalty or a goal.
/// </summary>
public class BallReset : MonoBehaviour 
{
    //Locations
    public GameObject RedSideCenter;
    public GameObject RedSideLeft;
    public GameObject RedSideRight;

    public GameObject BlueSideCenter;
    public GameObject BlueSideLeft;
    public GameObject BlueSideRight;

    //Ball
    public GameObject Ball;

    /// <summary>
    /// Used to initize variables
    /// </summary>
    void Start()
    {
        Ball = GameObject.FindGameObjectWithTag("Ball");
    }

    /// <summary>
    /// Places the ball at the location of the Red Side Center ball spawn.
    /// </summary>
    public void placeBallRSC()
    {
        Ball.transform.position = RedSideCenter.transform.position;
        Ball.GetComponent<Possession>().RedTeamPossession();
        StopBall();
    }

    /// <summary>
    ///  Places the ball at the location of the Red Side Left ball spawn.
    /// </summary>
    public void placeBallRSL()
    {
        Ball.transform.position = RedSideLeft.transform.position;
        Ball.GetComponent<Possession>().RedTeamPossession();
        StopBall();
    }

    /// <summary>
    ///  Places the ball at the location of the Red Side Right ball spawn.
    /// </summary>
    public void placeBallRSR()
    {
        Ball.transform.position = RedSideRight.transform.position;
        Ball.GetComponent<Possession>().RedTeamPossession();
        StopBall();
    }

    /// <summary>
    ///  Places the ball at the location of the Blue Side Center ball spawn.
    /// </summary>
    public void placeBallBSC()
    {
        Ball.transform.position = BlueSideCenter.transform.position;
        Ball.GetComponent<Possession>().BlueTeamPossession();
        StopBall();
    }

    /// <summary>
    /// Places the ball at the location of the Blue Side Left ball spawn.
    /// </summary>
    public void placeBallBSL()
    {
        Ball.transform.position = BlueSideLeft.transform.position;
        Ball.GetComponent<Possession>().BlueTeamPossession();
        StopBall();
    }

    /// <summary>
    /// Places the ball at the location of the Blue Side Right ball spawn.
    /// </summary>
    public void placeBallBSR()
    {
        Ball.transform.position = BlueSideRight.transform.position;
        Ball.GetComponent<Possession>().BlueTeamPossession();
        StopBall();
    }

    /// <summary>
    /// Places ball at the location it is closest to
    /// </summary>
    /// <param name="redTeamPositions">True if checking red positions, False if checking blue positions</param>
    public void resetToClosestPoint(bool redTeamPositions)
    {
        Dictionary<GameObject, float> distances = new Dictionary<GameObject, float>();

        if (redTeamPositions)
        {
            Ball.GetComponent<Possession>().RedTeamPossession();

            // Red
            distances.Add(RedSideCenter, Vector3.Distance(Ball.transform.position, RedSideCenter.transform.position));
            distances.Add(RedSideLeft, Vector3.Distance(Ball.transform.position, RedSideLeft.transform.position));
            distances.Add(RedSideRight, Vector3.Distance(Ball.transform.position, RedSideRight.transform.position));
        }
        else
        {
            Ball.GetComponent<Possession>().BlueTeamPossession();

            // Blue
            distances.Add(BlueSideCenter, Vector3.Distance(Ball.transform.position, BlueSideCenter.transform.position));
            distances.Add(BlueSideLeft, Vector3.Distance(Ball.transform.position, BlueSideLeft.transform.position));
            distances.Add(BlueSideRight, Vector3.Distance(Ball.transform.position, BlueSideRight.transform.position));
        }
        
        // Get the closest location
        GameObject closest = distances.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
        // Move ball to it and stop it
        Ball.transform.position = closest.transform.position;
        StopBall();
    }

    /// <summary>
    /// Stops the ball, getting rid of all of the velocity, both directional and rotational.
    /// Used to make sure the ball doesn't keep moving after it is moved.
    /// </summary>
    private void StopBall()
    {
        GameObject[] redPlayers = GameObject.FindGameObjectsWithTag("RedPlayer");
        GameObject[] bluePlayers = GameObject.FindGameObjectsWithTag("BluePlayer");
        GameObject[] players = redPlayers.Concat(bluePlayers).ToArray();
        foreach (GameObject player in players)
        {
            player.GetComponent<CatchThrowV2>().Drop();
        }

        Rigidbody RB = Ball.GetComponent<Rigidbody>();
        RB.velocity = new Vector3(0, 0, 0);
        RB.angularVelocity = new Vector3(0, 0, 0);
        Ball.transform.parent = null;
        RB.useGravity = true;
        //RB.constraints = RigidbodyConstraints.None;
        RB.constraints = RigidbodyConstraints.FreezeAll;
    }
}
