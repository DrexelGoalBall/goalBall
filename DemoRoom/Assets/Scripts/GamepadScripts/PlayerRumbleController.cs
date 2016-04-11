using UnityEngine;
using System.Collections;
using XInputDotNetPure;

    /// <summary>
    /// This script will apply a rumble when you collide with an object with a tag.
    /// 
    /// </summary>
public class PlayerRumbleController : MonoBehaviour
{

    public float rumbleWall = .5f;
    public float rumbleBall = .8f;
    public float rumbleBallNear = .1f;
    public string WallString = "Limits";
    public string BallString = "Ball";
    public float WallTime = 1f;
    public float BallTime = 1f;
    private float pickupDist = 5f;
    private RumbleController rumbler;
    private GameObject ball;
    private bool gotCloseToBall = false;

    void Start()
    {
        rumbler = GetComponent<RumbleController>();
        pickupDist = GetComponent<CatchThrowV2>().pickupDistance;
        ball = GameObject.FindGameObjectWithTag("Ball");
    }

    /// <summary>
    /// Detect when the player enters a trigger with the tag collideString and Activates the Rumble Function.
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == WallString)
        {
            rumbler.BasicRumble(WallTime, rumbleWall, rumbleWall);
        }
        else if (col.gameObject.tag == BallString)
        {
            rumbler.BasicRumble(BallTime, rumbleBall, rumbleBall);
        }
    }

    /// <summary>
    /// When the ball is close enought to the player to pick up, vibrate slightly
    /// </summary>
    void Update()
    {
        if (Vector3.Distance(transform.position, ball.transform.position) < pickupDist && !gotCloseToBall)
        {
            rumbler.RumbleBursts(3, rumbleBallNear, rumbleBallNear);
            gotCloseToBall = true;
        }
        if (Vector3.Distance(transform.position, ball.transform.position) > pickupDist)
        {
            gotCloseToBall = false;
        }
    }
}
