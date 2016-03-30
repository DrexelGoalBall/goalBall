using UnityEngine;
using System.Collections;


/// <summary>
/// This script is in charge of making the player dive.  This accomplishes moving the player down as well as rotating their position.
/// </summary>
public class Dive : MonoBehaviour {
	public GameObject player;
	private bool diveGo = false;
	private bool isUpright = true;
	private int diveCount;

    public float diveSpeed = 10f;
    public string diveCommand = "Dive";
    public float ballSlowdown = .333f;
    private GoalBallPlayerMovementV1 GBPM;

	
    /// <summary>
    /// Initializes all of the variables needed for this script.
    /// </summary>
	void Start ()
    {
        GBPM = GetComponent<GoalBallPlayerMovementV1>();
		diveCount = 0;
        player = gameObject;
	}

    /// <summary>
    /// Makes diving smother when a dive is detected.  Animates the player diving.
    /// </summary>
    void Update () {
        float diveDrop = .5f / diveSpeed;
        float diveRot = 90f / diveSpeed;
        if (diveGo && isUpright)
        {
            player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x,
                                                        player.transform.eulerAngles.y,
                                                    player.transform.eulerAngles.z + diveRot);
            player.transform.position = new Vector3(player.transform.position.x,
                                                    player.transform.position.y - diveDrop,
                                                    player.transform.position.z);
            diveCount++;
        }
        if (diveGo && !isUpright)
        {
            player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x,
                                                        player.transform.eulerAngles.y,
                                                        player.transform.eulerAngles.z - diveRot);
            player.transform.position = new Vector3(player.transform.position.x,
                                        player.transform.position.y + diveDrop,
                                        player.transform.position.z);
            diveCount++;
        }
        if (diveCount == (int)diveSpeed)
        {
            diveGo = false;
            diveCount = 0;
            if (isUpright)
            {
                isUpright = false;
                GBPM.horizontalEnabled = false;
            }
            else if (!isUpright)
            {
                StandUp();
            }
        }

	}

    /// <summary>
    /// Activates a dive action to occur.
    /// </summary>
	public void DivePressed(){
        if (Input.GetButtonDown(diveCommand))
        {
            if (!diveGo)
                diveGo = true;
        }
    }

    /// <summary>
    /// Allows outside classes to reset standing parameters (specifically ResetAim)
    /// </summary>
    public void StandUp()
    {
        isUpright = true;
        GBPM.horizontalEnabled = true;
    }

    /// <summary>
    /// When the player collieds with the ball, decrease the speed of the ball.
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ball" && !isUpright)
        {
            Rigidbody RB = col.gameObject.GetComponent<Rigidbody>();
            RB.velocity = ballSlowdown * RB.velocity;
            RB.angularVelocity = ballSlowdown * RB.angularVelocity;
        }
    }
}
