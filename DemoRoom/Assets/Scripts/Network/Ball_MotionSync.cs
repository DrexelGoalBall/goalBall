using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
///     Synchronizes the position/rotating of the ball over the network
/// </summary>
public class Ball_MotionSync : NetworkBehaviour
{
    // Position vector to sync clients with from server
    [SyncVar] private Vector3 syncPos;
    // Velocity vector to sync clients with from server
    [SyncVar] private Vector3 syncVel;
    // Throw force vector to sync clients with from server
    [SyncVar] private Vector3 throwForce;

    // Last position and velocity of ball on server that was sent
    private Vector3 lastPos;
    private Vector3 lastVel;

    // Rate to move the ball to correct position on clients
    public float lerpRate = 10;

    // Threshold for position and velocity changes to be considered on server
    public float posThreshold = 0.05f;
    public float velThreshold = 0.05f;
    
    // Initial transformation values when started
    private Transform startTrans;
    // Current transformation values
    private Transform myTransform;
    // Rigidbody component of this object
    private Rigidbody rigidBody; 

    /// <summary>
    ///     Sets up the initial values
    /// </summary>
    void Start()
    {
        startTrans = transform;
        myTransform = transform;
        rigidBody = GetComponent<Rigidbody>();
        throwForce = new Vector3(0, 0, 0);
    }

    /// <summary>
    ///     Every frame, send or lerp position of ball depending on server or client
    /// </summary>
    void Update()
    {
        TransmitMotion();
        LerpMotion();
    }

    /// <summary>
    ///     Moves the ball to its intital position/rotation
    /// </summary>
    public void Reset()
    {
        Debug.Log("Reset Ball");
        rigidBody.velocity = Vector3.zero;
        myTransform.position = startTrans.position;
        myTransform.rotation = startTrans.rotation;
    }

    /// <summary>
    ///     If this is the server and the ball has moved/rotated significantly, update its position/rotation
    /// </summary>
    void TransmitMotion()
    {
        // Do not continue if you are a client
        if (!isServer)
        {
            return;
        }

        if (Vector3.Distance(myTransform.position, lastPos) > posThreshold)
        {
            // Significant change in position, so update it
            lastPos = myTransform.position;
            syncPos = myTransform.position;
        }

        if (Vector3.Distance(rigidBody.velocity, lastVel) > velThreshold)
        {
            // Significant change in velocity, so update it
            lastVel = rigidBody.velocity;
            syncVel = rigidBody.velocity;
        }
        else if (Vector3.Distance(rigidBody.velocity, Vector3.zero) < velThreshold)
        {
            // Ball's velocity is close enough to zero to be considered zero (prevents continuous bouncing)
            lastVel = Vector3.zero;
            syncVel = Vector3.zero;
        }
    }

    /// <summary>
    ///     On clients, lerps position/rotation of ball based on transformations provided by server
    /// </summary>
    void LerpMotion()
    {
        // Do not continue if you are a server or the ball is being held
        if (isServer || transform.parent != null)
        {
            return;
        }

        if (syncVel == Vector3.zero)
        {
            // Stop the ball in this position
            myTransform.position = syncPos;
            rigidBody.velocity = syncVel;
        }
        else
        {
            // Move the ball towards the provided position from the server
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
        }
    }
}
