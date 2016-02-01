using UnityEngine;
using System.Collections;
using XInputDotNetPure;


public class RumbleOnCollide : MonoBehaviour
{
    /// <summary>
    /// This script will apply a rumble when you collide with an object with a tag.
    /// 
    /// </summary>
    public float rumbleStrength = .5f;
    public string collideString = "Lines";
	
	// Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == collideString) Rumble();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == collideString) Rumble();
    }

    public void Rumble()
    {
        GamePad.SetVibration(PlayerIndex.One, rumbleStrength, rumbleStrength);
    }
}
