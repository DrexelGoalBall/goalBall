using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GoalBallPlayerMovementV1 : NetworkBehaviour
{
    /// <summary>
    /// Script controls the movements of the player and the speed of the movments
    /// </summary>


    //Required Components
    private Rigidbody RB;

    //Movement Speed
    public float speed = 10f;

	// Use this for initialization
	void Start ()
    {
        RB = gameObject.GetComponent<Rigidbody>();
	}
	
    public void Move(string Horizontal, string Vertical)
    {
        float hinput = Input.GetAxis(Horizontal);
        float vinput = Input.GetAxis(Vertical);

        RB.velocity = (gameObject.transform.right * hinput * speed) + (gameObject.transform.forward * vinput * speed);
    }
}
