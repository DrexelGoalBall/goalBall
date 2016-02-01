using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class GoalBallPlayerMovementV1 : NetworkBehaviour
{
	//Required Components
	private Rigidbody RB;
	public AudioSource playerWalkSource;
   	public AudioClip playerWalkSound;

	//Movement Speed
	public float speed = 10f;
    public float slowSpeed = 1f;

	// Modifies time between walk sounds
	public float walkSoundMod = 1f; 

// Timer
	// Maximum time between foot sounds
	public float timerLimit = 1f;

	// Time between walk sounds
	private float timer = 0;

// Debug Globals
	public float avgVel; 
	public Vector3 RBvel;

    //Disable Horizontal when Dived
    public bool horizontalEnabled = true;

	// Use this for initialization
	void Start ()
	{
		RB = gameObject.GetComponent<Rigidbody>();
		playerWalkSource = gameObject.GetComponentInChildren<AudioSource>();
		playerWalkSource.clip = playerWalkSound;
	}
	
	// Update is called once per frame
	public void Move (string Horizontal, string Vertical)
	{
		float hinput = Input.GetAxis(Horizontal);
		float vinput = Input.GetAxis(Vertical);
        if (!horizontalEnabled)
        {
            RB.velocity = (gameObject.transform.up * vinput * slowSpeed);
        }
        else
        {
            RB.velocity = (gameObject.transform.right * hinput * speed) + (gameObject.transform.forward * vinput * speed);
        }

		RBvel = RB.velocity;

		avgVel = (Mathf.Abs(RB.velocity.x) + Mathf.Abs(RB.velocity.z)) / 2;

		if (avgVel > 0 && timer <= 0)
		{
			timer = walkSoundMod / avgVel;
			if (timer > timerLimit) timer = timerLimit;

			playerWalkSource.PlayOneShot(playerWalkSound);
		}

		timer -= Time.deltaTime;
	}
}
