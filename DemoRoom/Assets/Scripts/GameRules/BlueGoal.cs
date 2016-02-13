using UnityEngine;
using System.Collections;

public class BlueGoal : MonoBehaviour {

    /// <summary>
    /// This Script is supposed to detect when a ball enters a goal and alerts the ScoreKeeper.
    /// </summary>

	public GameObject ball;
	public ScoreKeeper scoreKeeper;
	bool inside;

    /// <summary>
    /// Initializes variables.
    /// </summary>
    void Start ()
    {
		inside = false;
	}


    /// <summary>
    /// When an object enters the objects collider, check if it is the ball then alert the scorekeeper.
    /// </summary>
    /// <param name="col"></param>
	void OnTriggerEnter(Collider col){
		if(col.gameObject.name == "Ball" && !inside){
			scoreKeeper.BlueTeamScored();
			inside = true;
		}
	}

    /// <summary>
    /// Detects when the ball exits the colider.
    /// </summary>
    /// <param name="col"></param>
	void OnTriggerExit(Collider col){
		if(col.gameObject.name == "Ball" && inside){
			inside = false;
		}
	}
}
