﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Ball_MotionSync : NetworkBehaviour
{
    [SyncVar]
    private Vector3 syncPos;
    [SyncVar]
    private Vector3 syncRot;

    private Vector3 lastPos;
    private Quaternion lastRot;
    private Transform myTransform;
    private float lerpRate = 10;
    private float posThreshold = 0.5f;
    private float rotThreshold = 5;

    private Transform startTrans;

    // Use this for initialization
    void Start()
    {
        startTrans = myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        TransmitMotion();
        LerpMotion();
    }

    public void Reset()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Debug.Log("Reset Ball");
        myTransform.position = startTrans.position;
        myTransform.rotation = startTrans.rotation;
    }

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

    void LerpMotion()
    {
        if (isServer || transform.parent != null)
        {
            return;
        }

        myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, Quaternion.Euler(syncRot), Time.deltaTime * lerpRate);
    }
}