using UnityEngine;
using System.Collections;

public class LineTrigger : MonoBehaviour {

	public GameObject controller;
	void OnTriggerEnter(Collider col){
		controller.GetComponent<Fouls>().LineOut();
	}
}
