using UnityEngine;
using System.Collections;

public class BallReset : MonoBehaviour {

    /// <summary>
    /// This script resets the ball to the proper position.  It will put the ball infront of one of the players on the team
    /// who is going to get the ball
    /// This could be due to a penalty or a goal
    /// </summary>

    //Locations
    public GameObject RedSideCenter;
    public GameObject RedSideLeft;
    public GameObject RedSideRight;

    public GameObject BlueSideCenter;
    public GameObject BlueSideLeft;
    public GameObject BlueSideRight;

    //Ball
    public GameObject Ball;
	
    //Find Gameobjects
    void Start()
    {
        Ball = GameObject.FindGameObjectWithTag("Ball");
    }

    public void placeBallRSC()
    {
        Ball.transform.position = RedSideCenter.transform.position;
        StopBall();
    }

    public void placeBallRSL()
    {
        Ball.transform.position = RedSideLeft.transform.position;
        StopBall();
    }

    public void placeBallRSR()
    {
        Ball.transform.position = RedSideRight.transform.position;
        StopBall();
    }
    public void placeBallBSC()
    {
        Ball.transform.position = BlueSideCenter.transform.position;
        StopBall();
    }
    public void placeBallBSL()
    {
        Ball.transform.position = BlueSideLeft.transform.position;
        StopBall();
    }
    public void placeBallBSR()
    {
        Ball.transform.position = BlueSideRight.transform.position;
        StopBall();
    }

    private void StopBall()
    {
        Rigidbody RB = Ball.GetComponent<Rigidbody>();
        RB.velocity = new Vector3(0, 0, 0);
        RB.angularVelocity = new Vector3(0, 0, 0);
        Ball.transform.parent = null;
    }

}
