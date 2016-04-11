using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
///     Synchronizes the position/rotating of the ball over the network
/// </summary>
public class Ball_MotionSync : NetworkBehaviour
{
    // 
    [SyncVar]
    private Vector3 syncPos;
    // 
    [SyncVar]
    private Vector3 syncVel;
    // 
    [SyncVar]
    private Vector3 throwForce;

    // 
    private Vector3 lastPos;
    private Vector3 lastVel;
    // 
    private Transform myTransform;

    // 
    public float lerpRate = 10;
    public float posThreshold = 0.05f;
    public float velThreshold = 0.05f;
    
    // 
    private Transform startTrans;
    private Rigidbody rigidBody; 

    /// <summary>
    ///     Sets up the initial values
    /// </summary>
    void Start()
    {
        startTrans = myTransform = transform;
        rigidBody = GetComponent<Rigidbody>();
        throwForce = new Vector3(0, 0, 0);
    }

    /// <summary>
    ///     Every frame, send or lerp position of ball depending on client or server
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
        if (!isServer)
        {
            return;
        }

        if (Vector3.Distance(myTransform.position, lastPos) > posThreshold)
        {
            lastPos = myTransform.position;
            syncPos = myTransform.position;
        }

        if (Vector3.Distance(rigidBody.velocity, lastVel) > velThreshold)
        {
            lastVel = rigidBody.velocity;
            syncVel = rigidBody.velocity;
        }
        else if (Vector3.Distance(rigidBody.velocity, Vector3.zero) < velThreshold)
        {
            lastVel = Vector3.zero;
            syncVel = Vector3.zero;
        }
    }
    /// <summary>
    ///     On clients, lerps position/rotation of ball based on transformations provided by server
    /// </summary>
    void LerpMotion()
    {
        if (isServer || transform.parent != null)
        {
            return;
        }

        if (syncVel == Vector3.zero)
        {
            rigidBody.velocity = syncVel;
            myTransform.position = syncPos;
        }
        else
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
        }
    }
}
