using UnityEngine;
using System.Collections;

public class BlueGoal : MonoBehaviour {

	public GameObject ball;
	public ScoreKeeper scoreKeeper;
	bool inside;
	// Use this for initialization
	void Start ()
    {
		inside = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (scoreKeeper == null)
        {
            scoreKeeper = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreKeeper>();
        }
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.name == "Ball" && !inside){
			scoreKeeper.BlueTeamScored();
			inside = true;
		}
	}

	void OnTriggerExit(Collider col){
		if(col.gameObject.name == "Ball" && inside){
			inside = false;
		}
	}
}
