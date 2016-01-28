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

	// Modifies time between walk sounds
	public float walkSoundMod = 3f; 

	// Really only global so I can view the value in the Debug window.
	private float avgVel; 

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

		RB.velocity = (gameObject.transform.right * hinput * speed)  +  (gameObject.transform.forward * vinput * speed);

		if (RB.velocity.x != 0 && RB.velocity.z != 0) // Our ground speed is not zero
		{
			avgVel = Mathf.Abs((RB.velocity.x + RB.velocity.z) / 2);

			StartCoroutine(WaitFor(avgVel / walkSoundMod));
		}
		else
		{
			playerWalkSource.Stop();
		}
	}

	IEnumerator WaitFor(float time)
	{
		yield return new WaitForSeconds(time);
		if(!playerWalkSource.isPlaying)
		{
			playerWalkSource.Play();
		}
	}
}
