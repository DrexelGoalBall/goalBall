using UnityEngine;
using System.Collections;

public class HalfTimePause : MonoBehaviour {

	public bool paused; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Pauses the game and announces the current score. Then resumes game for all players.
	/// This is called by the GameTimer class.
	/// </summary>
	public void HalfTime()
	{
		// Pause game
		Time.timeScale = 0f;
		print("I PAUSED THE GAME");
	}

	void OnApplicationPause(bool pauseStatus)
	{

	}
}
