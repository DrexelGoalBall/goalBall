using UnityEngine;
using System.Collections;
using XInputDotNetPure;

    /// <summary>
    /// This script will apply a rumble when you collide with an object with a tag.
    /// 
    /// </summary>
public class RumbleOnCollide : MonoBehaviour
{

    public float rumbleWall = .5f;
    public float rumbleBall = .8f;
    public string WallString = "Limits";
    public string BallString = "Ball";
    public float maxTime = 1f;

    public RumbleController rumbler;

    void Start()
    {
        rumbler = GetComponent<RumbleController>();
    }

    /// <summary>
    /// Detect when the player enters a trigger with the tag collideString and Activates the Rumble Function.
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == WallString) rumbler.BasicRumble(maxTime,rumbleWall,rumbleWall);
        else if (col.gameObject.tag == BallString) rumbler.BasicRumble(maxTime, rumbleBall, rumbleBall);
    }
}
