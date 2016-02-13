using UnityEngine;
using System.Collections;

/// <summary>
/// This script resets the ball to the proper position.  It will put the ball infront of one of the players on the team who is going to get the ball.
/// This could be due to a penalty or a goal.
/// </summary>
public class BallReset : MonoBehaviour {

 
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
        StopBall();
    }

    /// <summary>
    ///  Places the ball at the location of the Red Side Left ball spawn.
    /// </summary>
    public void placeBallRSL()
    {
        Ball.transform.position = RedSideLeft.transform.position;
        StopBall();
    }

    /// <summary>
    ///  Places the ball at the location of the Red Side Right ball spawn.
    /// </summary>
    public void placeBallRSR()
    {
        Ball.transform.position = RedSideRight.transform.position;
        StopBall();
    }

    /// <summary>
    ///  Places the ball at the location of the Blue Side Center ball spawn.
    /// </summary>
    public void placeBallBSC()
    {
        Ball.transform.position = BlueSideCenter.transform.position;
        StopBall();
    }

    /// <summary>
    /// Places the ball at the location of the Blue Side Left ball spawn.
    /// </summary>
    public void placeBallBSL()
    {
        Ball.transform.position = BlueSideLeft.transform.position;
        StopBall();
    }

    /// <summary>
    /// Places the ball at the location of the Blue Side Right ball spawn.
    /// </summary>
    public void placeBallBSR()
    {
        Ball.transform.position = BlueSideRight.transform.position;
        StopBall();
    }

    /// <summary>
    /// Stops the ball, getting rid of all of the velocity, both directional and rotational.
    /// Used to make sure the ball doesn't keep moving after it is moved.
    /// </summary>
    private void StopBall()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.GetComponent<CatchThrowV2>().Drop();
        }

        Rigidbody RB = Ball.GetComponent<Rigidbody>();
        RB.velocity = new Vector3(0, 0, 0);
        RB.angularVelocity = new Vector3(0, 0, 0);
        Ball.transform.parent = null;
        RB.useGravity = true;
        RB.constraints = RigidbodyConstraints.None;
    }
}
