using UnityEngine;
using System.Collections;


public class RumbleOnCollide : MonoBehaviour
{
    /// <summary>
    /// This script will apply a rumble when you collide with an object with a tag.
    /// 
    /// </summary>

	
	// Update is called once per frame
    void OnTriggerEnter(Collider col)
    {
        Rumble();
    }

    void OnCollisionEnter(Collision col)
    {
        Rumble();
    }

    public void Rumble()
    {
        
    }
}
