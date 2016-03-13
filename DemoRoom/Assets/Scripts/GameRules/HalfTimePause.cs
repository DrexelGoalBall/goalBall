using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class HalfTimePause : MonoBehaviour {

	private float maxPauseTime = 10f;
	private float pauseStart;

	private bool halfTime;

	private ScoreKeeper scoreKeeper;
	private Timer timer;
	private Referee referee;
	private GameObject ball;

	// Use this for initialization
	void Start () {
		scoreKeeper = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreKeeper>();
		timer = GetComponent<Timer>();
		referee = GameObject.FindGameObjectWithTag("Referee").GetComponent<Referee>();
		ball = GameObject.FindGameObjectWithTag("Ball");
	}
	
	// Update is called once per frame
	void Update () {
		if(timer.getTime() <= 0 && halfTime)
		{
			halfTime = false;
	//		ball.SetActive(true);
	//		ball.transform.position = ballStart;

			timer.Reset();
			referee.PlayPlay();				
		}
	}

	/// <summary>
	/// Pauses the game and announces the current score. Then resumes game for all players.
	/// This is called by the GameTimer class.
	/// </summary>
	public void HalfTime()
	{
		halfTime = true;
		timer.SetTime(maxPauseTime);

		// Read red score
		referee.PlayRedTeam();
		referee.ReadScore(scoreKeeper.RedScore());

		// Read blue score
		referee.PlayBlueTeam();
		referee.ReadScore(scoreKeeper.BlueScore());
	
	}

	
}
