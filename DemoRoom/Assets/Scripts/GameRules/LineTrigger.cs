using UnityEngine;
using System.Collections;

/// <summary>
/// Line Trigger
/// Throws a foul when a ball hits the out of bounds area.
/// </summary>
public class LineTrigger : MonoBehaviour {
	public Fouls fouls;
    public string ballTag = "Ball";

    /// <summary>
    /// Detects when an object collides with the trigger and contains the ballTag.
    /// </summary>
    /// <param name="col"></param>
	void OnTriggerEnter(Collider col)
    {
        if (col.tag == ballTag)
        {
            Debug.Log("LineTrigger (Trigger)");

            fouls.LineOut();
        }
    }

    /// <summary>
    /// Detects when an object collides with the colider and contains the ballTag.
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == ballTag)
        {
            Debug.Log("LineTrigger (Collision)");

            fouls.LineOut();
        }
    }
}
