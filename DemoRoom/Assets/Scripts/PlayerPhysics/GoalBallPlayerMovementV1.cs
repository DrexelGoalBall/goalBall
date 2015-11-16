using UnityEngine;
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

    [SyncVar]
    public int sideCorrection = 1;

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

        RB.velocity = (sideCorrection * gameObject.transform.right * hinput * speed) + 
                      (sideCorrection * gameObject.transform.forward * vinput * speed);
	}
}
