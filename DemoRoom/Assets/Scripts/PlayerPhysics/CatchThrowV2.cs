﻿using UnityEngine;
using System.Collections;

public class CatchThrowV2 : MonoBehaviour {

    /// <summary>
    /// CatchThrowV2
    /// This script
    /// </summary>

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
    public string catchButton = "Catch";
    public string throwButton = "Throw";
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
            horizonTilt = maxLeftAngle * Mathf.Pow(xAim,2);
        } else if (xAim < 0)
        {
            horizonTilt = maxRightAngle *-1* Mathf.Pow(xAim, 2);
        }
        if (yAim > 0)
        {
            verticalTilt = maxHighAngle * Mathf.Pow(yAim, 2);
        } else if (yAim < 0)
        {
            verticalTilt = maxLowAngle * -1 * Mathf.Pow(yAim, 2);
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

        if (Input.GetButtonDown(catchButton) && (ballInRange || ballheld))
        {
            ballheld = true;
            if (gameObject.tag == "BluePlayer")
            {
                ball.GetComponent<Possession>().BlueTeamPossession();
            }
            else if (gameObject.tag == "RedPlayer")
            {
                ball.GetComponent<Possession>().RedTeamPossession();
            }
            ball.transform.parent = ThrowDirection.transform;
            ballRB.constraints = RigidbodyConstraints.FreezeAll;
            ball.transform.localPosition = new Vector3(0, 0, 0);
        }

        if (Input.GetButtonDown(throwButton) && ballheld)
        {
            charging = true;
        }

        if (Input.GetButtonUp(throwButton) && ballheld)
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