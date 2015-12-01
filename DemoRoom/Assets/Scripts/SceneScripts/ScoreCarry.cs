using UnityEngine;
using System.Collections;

public class ScoreCarry : MonoBehaviour {

	public int teamWinning;
	
	void Awake(){
		DontDestroyOnLoad(this.gameObject);
	}

	// Update is called once per frame
	void Update () {
		if(Application.loadedLevelName == "DetailedGameScene"){
			if(GameObject.Find("GameController").GetComponent<ScoreKeeper>().BlueTeamScore >
				GameObject.Find("GameController").GetComponent<ScoreKeeper>().RedTeamScore)
				teamWinning = 0;
			else
				teamWinning = 1;
		}
	}
}
