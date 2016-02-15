using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
/// This script controls the translation movements of the play.
/// </summary>
public class GoalBallPlayerMovementV1 : NetworkBehaviour
{
	//Required Components
	private Rigidbody RB;

	//Movement Speed
	public float speed = 10f;
	public float slowSpeed = 1f;

	//Disable Horizontal when Dived
	public bool horizontalEnabled = true;

	/// <summary>
	/// initializes variables that need to be used by this script.
	/// </summary>
	void Start ()
	{
		RB = gameObject.GetComponent<Rigidbody>();
	}

	/// <summary>
	/// Gets the Horizontal and Vertical movement axis and gets their values.
	/// Then this function increases the velocity of the player in that direction.
	/// </summary>
	/// <param name="Horizontal"></param>
	/// <param name="Vertical"></param>
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
	}
}
