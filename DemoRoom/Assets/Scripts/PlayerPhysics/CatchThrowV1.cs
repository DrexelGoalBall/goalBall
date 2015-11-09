using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CatchThrowV1 : NetworkBehaviour
{
    [SyncVar(hook = "PickedUpBall")]
    public bool ballheld = false;
    [SyncVar()]
    public bool ballInRange = false;

    //Catch Colider 
    public GameObject ThrowDirection;
    public GameObject ball;

    //Throw Options
    public float throwForce = 10f;
    //public bool ballInRange = false;
    //public bool ballheld = false;
    public float pickupDistance = 5f;
    public string pickUpThrowButton = "Throw";

    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

	// Update is called once per frame
	void Update ()
    {
        Vector3 playerPos = gameObject.transform.position;
        Vector3 ballPos = ball.transform.position;

        //Determine if you can pickup the ball
        if (isLocalPlayer && Vector3.Distance(playerPos, ballPos) <  pickupDistance)
        {
            ballInRange = true;
        }
        else
        {
            ballInRange = false;
        }

        //Show Visually if you can pickup the ball
        if (ballInRange)
        {
            Debug.DrawLine(playerPos, ballPos);
        }
        else
        {
            Vector3 distCheckPos = new Vector3(playerPos.x - pickupDistance, playerPos.y, playerPos.z);
            Debug.DrawLine(playerPos, distCheckPos);
        }

        if (Input.GetButtonDown(pickUpThrowButton) && (ballInRange || ballheld))
        {
            ballheld = !ballheld;

            /*Rigidbody ballRB = ball.GetComponent<Rigidbody>();

            if (ballheld)
            {
                ballheld = false;
                ball.transform.parent = null;
                ballRB.constraints = RigidbodyConstraints.None;
                ballRB.AddForce(ThrowDirection.transform.forward * throwForce);
            }
            else
            {
                ballheld = true;
                ballRB.constraints = RigidbodyConstraints.FreezeAll;
                ball.transform.parent = ThrowDirection.transform;
                ball.transform.localPosition = new Vector3(0, 0, 0);
            }*/
        }
	}

    [Client]
    void PickedUpBall(bool ballHeld)
    {
        Debug.Log("Ball held = " + ballHeld);

        Rigidbody ballRB = ball.GetComponent<Rigidbody>();

        if (ballheld)
        {
            ball.transform.parent = null;
            ballRB.constraints = RigidbodyConstraints.None;
            ballRB.AddForce(ThrowDirection.transform.forward * throwForce);
        }
        else
        {
            ballRB.constraints = RigidbodyConstraints.FreezeAll;
            ball.transform.parent = ThrowDirection.transform;
            ball.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}