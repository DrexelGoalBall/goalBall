using UnityEngine;
using System.Collections;

public class GoalBallPlayerMovementV1 : MonoBehaviour
{

	//Control names
	public string Horizonal = "Horizontal";
	public string Vertical = "Vertical";

	//Required Components
	private Rigidbody RB;
	private AudioSource playerWalkSource;

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
		playerWalkSource = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		float hinput = Input.GetAxis(Horizonal);
		float vinput = Input.GetAxis(Vertical);

		RB.velocity = (gameObject.transform.right * hinput * speed)  +  (gameObject.transform.forward * vinput * speed);

		if (RB.velocity.x != 0 && RB.velocity.z != 0) // Our ground speed is not zero
		{
//			float picthLevel = Random.Range(minPitch, maxPitch);
//			playerWalkSource.pitch = picthLevel;

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
