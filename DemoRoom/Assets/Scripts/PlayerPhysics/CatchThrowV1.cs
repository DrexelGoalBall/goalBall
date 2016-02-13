using UnityEngine;
using System.Collections;

/// <summary>
/// This player is an old player throw controller that is no longer used.
/// </summary>
public class CatchThrowV1 : MonoBehaviour {
    //Catch Colider 
    public GameObject ThrowDirection;
    public GameObject ball;

    //Throw Options
    public float throwForce = 10f;
    public bool ballInRange = false;
    public bool ballheld = false;
    public float pickupDistance = 5f;
    public string pickUpThrowButton = "Throw";

	// Update is called once per frame
	void Update ()
    {
        Vector3 playerPos = gameObject.transform.position;
        Vector3 ballPos = ball.transform.position;

        //Determine if you can pickup the ball
        if (Vector3.Distance(playerPos, ballPos) <  pickupDistance)
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
            Rigidbody ballRB = ball.GetComponent<Rigidbody>();

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
                ballRB.constraints = RigidbodyConstraints.FreezePosition;
                ball.transform.parent = ThrowDirection.transform;
                ball.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
	}



}
