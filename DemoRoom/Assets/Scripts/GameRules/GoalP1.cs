using UnityEngine;
using System.Collections;

public class GoalP1 : MonoBehaviour {

	public GameObject ball;
	public GameObject controller;
	bool inside;
	// Use this for initialization
	void Start () {
		inside = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.name == "Ball" && !inside){
			controller.GetComponent<ScoreDisplay>().addScoreP2();
			inside = true;
		}
	}

	void OnTriggerExit(Collider col){
		if(col.gameObject.name == "Ball" && inside){
			inside = false;
		}
	}
}
