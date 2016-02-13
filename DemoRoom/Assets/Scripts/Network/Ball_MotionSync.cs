﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Ball_MotionSync : NetworkBehaviour
{
    /// <summary>
    ///     Synchronizes the position/rotating of the ball over the network
    /// </summary>

    // 
    [SyncVar]
    private Vector3 syncPos;
    // 
    [SyncVar]
    private Vector3 syncRot;
    // 
    [SyncVar]
    private Vector3 throwForce;

    // 
    private Vector3 lastPos;
    private Quaternion lastRot;
    // 
    private Transform myTransform;

    // 
    private float lerpRate = 10;
    private float posThreshold = 0.5f;
    private float rotThreshold = 5;

    // 
    private Transform startTrans;

    //[SyncVar(hook = "ApplyForce")]
    //private bool isThrown = false; 

    /// <summary>
    ///     Sets up the initial values
    /// </summary>
    void Start()
    {
        startTrans = myTransform = transform;
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
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Debug.Log("Reset Ball");
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

        if (Vector3.Distance(myTransform.position, lastPos) > posThreshold || Quaternion.Angle(myTransform.rotation, lastRot) > rotThreshold)
        {
            lastPos = myTransform.position;
            lastRot = myTransform.rotation;

            syncPos = myTransform.position;
            syncRot = myTransform.localEulerAngles;
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

        myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, Quaternion.Euler(syncRot), Time.deltaTime * lerpRate);
    }

   // void ApplyForce(bool isThrown)
   // {
   //     Debug.Log(throwForce);
   //     Rigidbody ballRB = GetComponent<Rigidbody>();
   //     ballRB.AddForce(throwForce);
   //     Debug.Log("Thrown");
   // }

   // [Command]
   // void CmdThrow(bool pIsThrown, Vector3 force)
   // {
   //     isThrown = pIsThrown;
   //     if (isServer && !isClient)
   //         throwForce = force;
   //         ApplyForce(isThrown);
   //     Debug.Log("Command called");
   // }

   //[ClientCallback]
   //public void TransmitThrow(bool pIsThrown, Vector3 force)
   // {
   //     if (isLocalPlayer)// && (ballheld || ball.transform.parent == null))
   //     {
   //         Debug.Log("Send");
   //         CmdThrow(pIsThrown,force);
   //         Debug.Log("Back");
   //         if (!isServer)
   //         {
   //             isThrown = pIsThrown;
   //         }
   //     }
   // }
}
