using UnityEngine;
using System.Collections;

/// <summary>
///     Keeps track of which team is winning and carries it between scenes
/// </summary>
public class ScoreCarry : MonoBehaviour 
{
    //
	public int teamWinning;
	
    /// <summary>
    ///     Do not destroy the object with this script when a new scene is loaded
    /// </summary>
	void Awake()
    {
		DontDestroyOnLoad(this.gameObject);
	}

	/// <summary>
	///     Every frame in game scene, check who is winning
	/// </summary>
	void Update() 
    {
		if (Application.loadedLevelName == "DetailedGameScene")
        {
			if (GameObject.Find("GameController").GetComponent<ScoreKeeper>().BlueTeamScore >
				GameObject.Find("GameController").GetComponent<ScoreKeeper>().RedTeamScore)
				teamWinning = 0;
			else
				teamWinning = 1;
		}
	}
}
