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

    //For moving back to start position.
    public Vector3 currentPosition = new Vector3(0,0,0);
    public bool goBack = false;

	/// <summary>
	/// initializes variables that need to be used by this script.
	/// </summary>
	void Start ()
	{
		RB = gameObject.GetComponent<Rigidbody>();
	}

    /// <summary>
    /// if goback is true, then it will move it back to its origonal position.
    /// </summary>
    void Update()
    {
        if (goBack)
        {
            if (Mathf.Abs(Vector3.Distance(currentPosition, gameObject.transform.position)) < 1)
            {
                goBack = false;
            }
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, currentPosition, step);
            
        }
    }

	/// <summary>
	/// Increases the velocity of the player by the provided values.
	/// </summary>
	/// <param name="Horizontal"></param>
	/// <param name="Vertical"></param>
	public void Move (float Horizontal, float Vertical)
	{
        if (Mathf.Abs(Horizontal) + Mathf.Abs(Vertical) > .1)
        {
            goBack = false;
        }

            if (!horizontalEnabled)
		{
			RB.velocity = (gameObject.transform.up * Vertical * slowSpeed);
		}
		else
		{
			RB.velocity = (gameObject.transform.right * Horizontal * speed) + (gameObject.transform.forward * Vertical * speed);
		}
	}

    /// <summary>
    /// Set goback to true which should move the player back to his origonal position
    /// </summary>
    public void AutoMove()
    {
        goBack = true;
    }
}
