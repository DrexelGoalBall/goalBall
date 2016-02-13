using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class CatchThrowV2 : NetworkBehaviour {

    /// <summary>
    /// CatchThrowV2
    /// This script controls the catching and trowing of the ball and the networked capabilities associated with those actions.
    /// </summary>
    #region variables
    //Catch Colider 
    public GameObject ThrowDirection;
    public GameObject ball;
    public GameObject aim;
    private Rigidbody playerRB;

    //Throw Options
    public float throwForce = 10f;
    private bool ballInRange = false;
    [SyncVar(hook = "ChangeBallHold")]
    public bool ballheld = false;
    [SyncVar(hook = "ApplyForceToBall")]
    Vector3 Force = new Vector3(0, 0, 0);
    public float pickupDistance = 5f;
    private bool charging = false;
    private float timer = 0f;
    private float maxtime = 3f;

    //AimingAngles
    public float maxHighAngle = 45f;
    public float maxLowAngle = 45f;
    public float maxLeftAngle = 90f;
    public float maxRightAngle = 90f;

    bool stiff = false;
    public float aimSpeed = 1f;

    private float initialAim;

    private bool localDrop = false;
    [SyncVar(hook = "DropIfHolding")]
    public bool dropBall = false;

    #endregion

    #region basicUnityFunctions

    /// <summary>
    /// This function initializes all of the variables needed for the script.
    /// </summary>
    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
        ball = GameObject.FindGameObjectWithTag("Ball");
        initialAim = transform.localEulerAngles.y;
    }

    /// <summary>
    /// Detects the distance between the player and the ball and determines if the player is in range to pick up the ball.
    /// DEBUGGING: can be used to change the player controls when the M key is pressed.
    /// </summary>
    void Update()
    {
        Vector3 playerPos = gameObject.transform.position;
        Vector3 ballPos = ball.transform.position;

        //Determine if you can pickup the ball
        if (Vector3.Distance(playerPos, ballPos) < pickupDistance)
        {
            ballInRange = true;
        }
        else
        {
            ballInRange = false;
        }

        Rigidbody ballRB = ball.GetComponent<Rigidbody>();

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (stiff) stiff = false;
            else stiff = true;
        }

    }
    #endregion

    /// <summary>
    /// Controls the Aim of the player based on the horizontalAim and verticalAim parameters.
    /// </summary>
    /// <param name="horizontalAim"></param>
    /// <param name="verticalAim"></param>
    public void Aim(string horizontalAim, string verticalAim)
    {
        if (ball == null)
            ball = GameObject.FindGameObjectWithTag("Ball");

        //Get controller input
        float xAim = Input.GetAxis(horizontalAim);
        float yAim = Input.GetAxis(verticalAim);

        if (stiff)
        {
            stiffAim(xAim, yAim);
        } else
        {
            FPSAim(xAim, yAim);
        }

    }

    /// <summary>
    /// Stiff aim is when the player uses the control stick to change the direction of the ball from the center of their initial view.  
    /// When the control stick is released the aim will center.
    /// </summary>
    /// <param name="xAim"></param>
    /// <param name="yAim"></param>
    void stiffAim(float xAim, float yAim)
    {
        float horizonTilt = 0;
        float verticalTilt = 0f;

        //Aiming direction of throw
        if (xAim > 0)
        {
            horizonTilt = maxLeftAngle * Mathf.Pow(xAim, 2);
        }
        else if (xAim < 0)
        {
            horizonTilt = maxRightAngle * -1 * Mathf.Pow(xAim, 2);
        }
        if (yAim > 0)
        {
            verticalTilt = maxHighAngle * Mathf.Pow(yAim, 2);
        }
        else if (yAim < 0)
        {
            verticalTilt = maxLowAngle * -1 * Mathf.Pow(yAim, 2);
        }

        aim.transform.localRotation = Quaternion.Euler(verticalTilt, horizonTilt, 0);
    }

    /// <summary>
    /// Classic first person shooter aiming that allows the palyers to look around and not have their view reset to the origonal position.
    /// </summary>
    /// <param name="xAim"></param>
    /// <param name="yAim"></param>
    void FPSAim(float xAim, float yAim)
    {
        float yRot = yAim * aimSpeed;
        float xRot = xAim * aimSpeed;

        Vector3 selfOrig = transform.eulerAngles;

        Quaternion aimTemp = aim.transform.localRotation * Quaternion.Euler(yRot, xRot, 0);
        Quaternion selfTemp = transform.localRotation * Quaternion.Euler(yRot, xRot, 0);
        aimTemp.eulerAngles = new Vector3(aimTemp.eulerAngles.x,0, 0);
        selfTemp.eulerAngles = new Vector3(selfOrig.x, selfTemp.eulerAngles.y, selfOrig.z);

        float differenceInAngles = Mathf.DeltaAngle(selfTemp.eulerAngles.y, initialAim);
        if (differenceInAngles < -1 * maxRightAngle)
        {
            selfTemp.eulerAngles = new Vector3(selfOrig.x, maxRightAngle + initialAim, selfOrig.z);
        }

        if (differenceInAngles > maxLeftAngle)
        {
            selfTemp.eulerAngles = new Vector3(selfOrig.x, initialAim - maxLeftAngle , selfOrig.z);
        }

        float differenceInVertAngles = Mathf.DeltaAngle(aimTemp.eulerAngles.x, 0);
        Debug.Log(differenceInVertAngles);
        if (differenceInVertAngles >  maxHighAngle)
        {
            aimTemp.eulerAngles = new Vector3(-1 * maxHighAngle, 0, 0);
        }

        if (differenceInVertAngles < -1 *maxLowAngle)
        {
            aimTemp.eulerAngles = new Vector3(maxLowAngle, 0, 0);
        }


        aim.transform.localRotation = aimTemp;
        transform.localRotation = selfTemp;
    }

    /// <summary>
    /// Resets the aim to the initial aim on a button click.
    /// </summary>
    public void ResetAim()
    {
        transform.localEulerAngles = new Vector3(0, initialAim, 0);
        aim.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Makes the calls to Catch the ball.
    /// </summary>
    public void CatchBall()
    {
        if (ballInRange || ballheld)
        {
            TransmitBallPickup(true);
        }
    }

    /// <summary>
    /// Makes the calls to charge the ball.
    /// </summary>
    public void ChargeBall()
    {
        if (ballheld)
        {
            charging = true;
        }
    }

    /// <summary>
    /// Makes the call to throw the ball.
    /// Differentiates calls depending on whether the player is hosing or a client.
    /// </summary>
    public void ThrowBall()
    {
        if (ballheld)
        {
            float charge = timer / maxtime;
            if (charge > 1) charge = 1;
            Vector3 Force = aim.transform.forward * throwForce * charge;

            if (isServer)
            {
                Rigidbody ballRB = ball.GetComponent<Rigidbody>();
                ball.transform.parent = null;
                ballRB.constraints = RigidbodyConstraints.None;

                ballRB.AddForce(Force);
                charging = false;
                timer = 0f;
                Debug.Log("Shoot");
                ballheld = false;
            }
            else
            {
                TransmitBallThrow(Force);
                ballheld = false;
            }
        }
    }

    /// <summary>
    /// Increments the value of the charging variable.
    /// </summary>
    void FixedUpdate()
    {
        if (charging)
        {
            timer += .05f;
        }
    }

    /*
    Networked Functions
    */
    /// <summary>
    /// Changes whos has the ball in a given game.
    /// </summary>
    /// <param name="held"></param>
    void ChangeBallHold(bool held)
    {
        if (!held) return;
        Debug.Log("Ball held = " + held);

        Rigidbody ballRB = ball.GetComponent<Rigidbody>();

        Debug.Log(held);
        Debug.Log("Start");
            
        if (gameObject.tag == "BluePlayer")
        {
            GetComponent<Possession>().BlueTeamPossession();
        }
        else if (gameObject.tag == "RedPlayer")
        {
            GetComponent<Possession>().RedTeamPossession();
        }
        ball.transform.parent = ThrowDirection.transform;
        ballRB.constraints = RigidbodyConstraints.FreezeAll;
        ball.transform.localPosition = new Vector3(0, 0, 0);
        Debug.Log("Grab");
    }

    /// <summary>
    /// Throws the ball from a given player at a given position.
    /// </summary>
    /// <param name="Force"></param>
    void ApplyForceToBall(Vector3 Force)
    {
        Rigidbody ballRB = ball.GetComponent<Rigidbody>();
        ball.transform.parent = null;
        ballRB.constraints = RigidbodyConstraints.None;

        ballRB.AddForce(Force);
        charging = false;
        timer = 0f;
        ballheld = false;
        Debug.Log("Shoot");
    }

    /// <summary>
    /// Command sent to server to let the server know that the player has picked up the ball.
    /// </summary>
    /// <param name="held"></param>
    [Command]
    void CmdBallPickup(bool held)
    {
        ballheld = held;

        ChangeBallHold(held);
    }

    /// <summary>
    /// Command sent to the server to let the server know that the player has thrown the ball.
    /// </summary>
    /// <param name="pForce"></param>
    [Command]
    void CmdBallThrow(Vector3 pForce)
    {
        Force = pForce;
        ApplyForceToBall(Force);
        ballheld = false;
    }

    /// <summary>
    /// Client transmits that they have picked up the ball.
    /// </summary>
    /// <param name="held"></param>
    [ClientCallback]
    void TransmitBallPickup(bool held)
    {
        Debug.Log("TRANSMITTING");

        if (isLocalPlayer)// && (ballheld || ball.transform.parent == null))
        {
            Debug.Log("Send");
            CmdBallPickup(held);
            Debug.Log("Back");
            if (!isServer)
            {
               ballheld = held;
            }
        }
    }

    /// <summary>
    /// CLient transmits that they have thrown the ball.
    /// </summary>
    /// <param name="pForce"></param>
    [ClientCallback]
    void TransmitBallThrow(Vector3 pForce)
    {
        if (isLocalPlayer)// && (ballheld || ball.transform.parent == null))
        {
            Debug.Log("Send");
            if (!isServer)
            {
                Force = pForce;
            }
            CmdBallThrow(pForce);
            Debug.Log("Back");
        }
    }

    /// <summary>
    /// Detect when the ball is dropped.
    /// </summary>
    public void Drop()
    {
        if (isServer && ballheld)
        {
            Debug.Log("Drop");
            dropBall = !dropBall;
            ballheld = false;
        }
    }

    /// <summary>
    /// If the player is holding the ball and it needs to be dropped it will drop it.
    /// </summary>
    /// <param name="drop"></param>
    private void DropIfHolding(bool drop)
    {
        if (ballheld && drop != localDrop)
        {
            Rigidbody ballRB = ball.GetComponent<Rigidbody>();
            //ballRB.velocity = new Vector3(0, 0, 0);
            //ballRB.angularVelocity = new Vector3(0, 0, 0);
            ball.transform.parent = null;
            ballRB.constraints = RigidbodyConstraints.None;

            Debug.Log("DROPPING");
            charging = false;
            timer = 0f;
            ballheld = false;
            localDrop = drop;
        }
    }
}


