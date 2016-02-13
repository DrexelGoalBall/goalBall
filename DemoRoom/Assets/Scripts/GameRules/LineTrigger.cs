using UnityEngine;
using System.Collections;

public class LineTrigger : MonoBehaviour {

    /// <summary>
    /// Line Trigger
    /// Throws a foul when a ball hits the out of bounds area.
    /// </summary>

	public Fouls fouls;
	void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Ball")
        {
            Debug.Log("LineTrigger (Trigger)");

            fouls.LineOut();
        }
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ball")
        {
            Debug.Log("LineTrigger (Collision)");

            fouls.LineOut();
        }
    }
}
