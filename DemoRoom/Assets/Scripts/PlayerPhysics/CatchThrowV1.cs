using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CatchThrowV1 : NetworkBehaviour
{
    [SyncVar(hook = "ChangeBallHold")]
    public bool ballheld = false;
    private bool balldrop = false;

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
        
    }

	// Update is called once per frame
	void Update ()
    {
        Vector3 playerPos = gameObject.transform.position;
        
        if (ball == null)
            ball = GameObject.FindGameObjectWithTag("Ball");    
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
            TransmitBallChange(!ballheld, false);
        }
	}

    void ChangeBallHold(bool held)
    {
        Debug.Log("Ball held = " + held);

        Rigidbody ballRB = ball.GetComponent<Rigidbody>();

        if (!held)
        {
            ball.transform.parent = null;
            ballRB.constraints = RigidbodyConstraints.None;
            if (!balldrop)
                ballRB.AddForce(ThrowDirection.transform.forward * throwForce);
        }
        else
        {
            ballRB.constraints = RigidbodyConstraints.FreezeAll;
            ball.transform.parent = ThrowDirection.transform;
            ball.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    [Command]
    void CmdBallUpdates(bool held, bool drop)
    {
        balldrop = drop;
        ballheld = held;
        if (isServer && !isClient)
            ChangeBallHold(held);
        Debug.Log("Command called");
    }

    [ClientCallback]
    void TransmitBallChange(bool held, bool drop)
    {
        if (isLocalPlayer)// && (ballheld || ball.transform.parent == null))
        {
            Debug.Log("Send");
            CmdBallUpdates(held, drop);
            Debug.Log("Back");
            if (!isServer)
            {
                balldrop = drop;
                ballheld = held;
            }
        }
    }

    public void DropBall()
    {
        if (ballheld)
        {
            Debug.Log("Drop Ball");
            TransmitBallChange(false, true);
        }
    }
}