﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GoalBallPlayerMovementV1 : NetworkBehaviour
{

    //Control names
    public string Horizonal = "Horizontal";
    public string Vertical = "Vertical";

    //Required Components
    private Rigidbody RB;

    //Movement Speed
    public float speed = 10f;

	// Use this for initialization
	void Start ()
    {
        RB = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        float hinput = Input.GetAxis(Horizonal);
        float vinput = Input.GetAxis(Vertical);

        RB.velocity = (gameObject.transform.right * hinput * speed)  +  (gameObject.transform.forward * vinput * speed);
	}
}
