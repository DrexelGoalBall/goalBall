using UnityEngine;
using System.Collections;

public class GoalP2 : MonoBehaviour {

	public GameObject ball;
	public GameObject controller;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.name == "Ball"){
			controller.GetComponent<ScoreP2>().addScore();
		}
	}
}
