using UnityEngine;
using System.Collections;

public class CatchThrowV2 : MonoBehaviour {

    //Catch Colider 
    public GameObject ThrowDirection;
    public GameObject ball;
    public GameObject aim;
    private Rigidbody playerRB;

    //Throw Options
    public float throwForce = 10f;
    private bool ballInRange = false;
    private bool ballheld = false;
    public float pickupDistance = 5f;
    private bool charging = false;
    private float timer = 0f;
    private float maxtime = 3f;

    //Controls
    public string pickUpThrowButton = "Throw";
    public string horizontalAim = "horizontalAim";
    public string verticalAim = "verticalAim";

    //AimingAngles
    public float maxHighAngle = 45f;
    public float maxLowAngle = 45f;
    public float maxLeftAngle = 45f;
    public float maxRightAngle = 45f;

    void Start ()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
    } 

	// Update is called once per frame
	void Update ()
    {
        Vector3 playerPos = gameObject.transform.position;
        Vector3 ballPos = ball.transform.position;

        //Get controller input
        float xAim = Input.GetAxis(horizontalAim);
        float yAim = Input.GetAxis(verticalAim);
        float horizonTilt = 0;
        float verticalTilt = 0f;

        //Aiming direction of throw
        if (xAim > 0)
        {
            horizonTilt = maxLeftAngle * xAim;
        } else if (xAim < 0)
        {
            horizonTilt = maxRightAngle * xAim;
        }
        if (yAim > 0)
        {
            verticalTilt = maxHighAngle * yAim;
        } else if (yAim < 0)
        {
            verticalTilt = maxLowAngle * yAim;
        }

        aim.transform.localRotation = Quaternion.Euler(verticalTilt, horizonTilt, 0);

        //Determine if you can pickup the ball
        if (Vector3.Distance(playerPos, ballPos) <  pickupDistance)
        {
            ballInRange = true;
        }
        else
        {
            ballInRange = false;
        }

        Rigidbody ballRB = ball.GetComponent<Rigidbody>();

        if (Input.GetButtonDown(pickUpThrowButton) && (ballInRange || ballheld))
        {
                ballheld = true;
                charging = true;
                ball.transform.parent = ThrowDirection.transform;
                ballRB.constraints = RigidbodyConstraints.FreezeAll;
                ball.transform.localPosition = new Vector3(0, 0, 0);
        }

        if (Input.GetButtonUp(pickUpThrowButton) && ballheld)
        {
            ballheld = false;
            ball.transform.parent = null;
            ballRB.constraints = RigidbodyConstraints.None;
            float charge = timer / maxtime;
            if (charge > 1) charge = 1;
            float force = throwForce * charge;
            ballRB.AddForce(aim.transform.forward * force + playerRB.velocity);
            charging = false;
            timer = 0f;
        }
	}

    void FixedUpdate()
    {
        if (charging)
        {
            timer += .05f;
        }
    }



}
