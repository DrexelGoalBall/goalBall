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
    private float timer = 0f;
    public float maxTime = 1f;
    private bool rumble = false;
	
	/// <summary>
    /// Detect when the player enters a collider with the tag collideString and activates the Rubmle Function.
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == collideString) Rumble();
    }

    /// <summary>
    /// Detect when the player enters a trigger with the tag collideString and Activates the Rumble Function.
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == collideString) Rumble();
    }


    void Update()
    {
        if (rumble)
        {
            timer += Time.deltaTime;
            GamePad.SetVibration(PlayerIndex.One, rumbleStrength, 0);
            if (timer > maxTime)
            {
                rumble = false;
            }
        }
        else
        {
            GamePad.SetVibration(PlayerIndex.One, 0, 0);
        }
    }

    /// <summary>
    /// Rumbles an xbox controller.
    /// </summary>
    public void Rumble()
    {
        rumble = true;
        timer = 0f;
    }
}
