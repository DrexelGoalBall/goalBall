using UnityEngine;
using System.Collections;

/// <summary>
/// This script controls the frequency of each player's feet noise based off of speed.
/// </summary>

public class WalkSound : MonoBehaviour {

	// Audio Components
	public AudioSource playerWalkSource;
	public AudioClip playerWalkSound;

	// Modifies time between walk sounds
	public float walkSoundMod = 1f; 

//Position Timer - Keeps track of change in position to update speed

	// Total seconds between position update
	private float posUpdate = 0.00005f; 

	private float posTimer = 0; 

	private Vector3 prevPos;

// Walk Timer - Tracks time between footstep sounds

	// Maximum time between foot sounds
	public float walkTimerLimit = 0.4f;
	// Time between walk sounds
	private float walkTimer = 0;

	// Speed var. Public for debug purposes.
	public float avgVel; 

	// Position difference. Public for debug purposes.
	public Vector3 RBvel;

	// Use this for initialization
	void Start () {
		playerWalkSource = gameObject.GetComponentInChildren<AudioSource>();
		playerWalkSource.clip = playerWalkSound;

		prevPos = gameObject.transform.position;
	}

	void FixedUpdate()
	{
		posTimer += Time.deltaTime;

		if (posTimer >= posUpdate)
		{
			Vector3 currPos =  gameObject.transform.position;

			RBvel = currPos - prevPos;

			avgVel = (Mathf.Abs(RBvel.x) + Mathf.Abs(RBvel.z)) / 2; // Networked method of finding velocity

			prevPos = currPos; // Update the previous position
			posTimer = 0;
		}

		if (avgVel > 0 && walkTimer <= 0)
		{
			walkTimer = walkSoundMod / avgVel;
			if (walkTimer > walkTimerLimit) walkTimer = walkTimerLimit;

			playerWalkSource.PlayOneShot(playerWalkSound);
		}

		walkTimer -= Time.deltaTime;
	}


}
