using UnityEngine;
using System.Collections;
using XInputDotNetPure;

    /// <summary>
    /// This script will apply a rumble when you collide with an object with a tag.
    /// 
    /// </summary>
public class RumbleOnCollide : MonoBehaviour
{

    public float rumbleStrength = .5f;
    public string collideString = "Limits";
	
	/// <summary>
    /// Detect when the player enters a collider with the tag collideString and activates the Rubmle Function.
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == collideString) Rumble();
    }

    /// <summary>
    /// Detect when the player enters a trigger with the tag collideString and Activates the Rumble Function.
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == collideString) Rumble();
    }

    /// <summary>
    /// Rumbles an xbox controller.
    /// </summary>
    public void Rumble()
    {
        GamePad.SetVibration(PlayerIndex.One, rumbleStrength, rumbleStrength);
    }
}
