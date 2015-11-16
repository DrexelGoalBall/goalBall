using UnityEngine;
using System.Collections;

public class LineTrigger : MonoBehaviour {

    /// <summary>
    /// Line Trigger
    /// Throws a foul when a ball hits the out of bounds area.
    /// </summary>

	public GameObject controller;
	void OnTriggerEnter(Collider col){
        Debug.Log("YO");

        if (col.tag == "Ball")
        {
            controller.GetComponent<Fouls>().LineOut();
        }
    }
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("YO");
        if (col.gameObject.tag == "Ball")
        {
            controller.GetComponent<Fouls>().LineOut();
        }
    }
}
