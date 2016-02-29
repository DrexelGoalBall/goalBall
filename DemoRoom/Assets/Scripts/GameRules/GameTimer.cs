using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// This script controls the game times and knows when it should be going and knows when it should be stopped.
/// </summary>
public class GameTimer : MonoBehaviour {
	//GUI Objects
	public Text timeText;

	//Players
	//public GameObject player1;
	//public GameObject player2;
	public GameObject ball;

	//Initial Player positions
	//private Vector3 player1Start;
	//private Vector3 player2Start;
	private Vector3 ballStart;

	//Timer Variables
	Timer timer;
	public int halfLength = 120;
	int half = 1;
	bool endGame = false;
	bool gameGoing = false;
	bool fifteenSecondsCheck = false;
	bool halfTime = false;

	//Other Objects
	private ScoreKeeper scoreKeeper;
	private HalfTimePause pauseEvent;
	private Referee referee;

	/// <summary>
	/// Initialize all variables needed for this script to run.
	/// </summary>
	void Start () {
		//timer = gameObject.AddComponent<Timer>();
		timer = GetComponent<Timer>();
		timer.SetLengthOfTimer(halfLength + 1);
		timer.Pause();
		//player1Start = player1.transform.position;
		//player2Start = player2.transform.position;
		ballStart = ball.transform.position;
		scoreKeeper = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreKeeper>();
		pauseEvent = GameObject.FindGameObjectWithTag("GameController").GetComponent<HalfTimePause>();
		referee = GameObject.FindGameObjectWithTag("Referee").GetComponent<Referee>();
	}

	/// <summary>
	/// Keeps track of the Timer and causes referee to say certain things as the time progresses.
	/// </summary>
	void Update () {
		if (endGame) return;
		timeText.text = timer.getTimeString();
		if(timer.getTime() <= 0){
			if (halfTime) // Halftime over, begin second half of game
			{
				halfTime = false;
			}
			else
				nextHalf();
		}

		if (referee.refereeSpeaking())
		{
			StopGame();
		}
		else if (!gameGoing)
		{
			StartGame();
		}

		if ( timer.getTime() <= 15 && !fifteenSecondsCheck)
		{
			referee.PlayFifteenSeconds();
			fifteenSecondsCheck = true;
		}
	}

	/// <summary>
	/// Resets the timer and increments the half variable.
	/// </summary>
	void nextHalf(){
		if (half >= 2)
		{
			//timeText.text = "GAME OVER";
			endGame = true;
			Application.LoadLevel("GameOver");
			return;
		}

		//player1.transform.position = player1Start;
		//player2.transform.position = player2Start;
		ball.transform.position = ballStart;

		ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
//		timer.Reset();
		referee.PlayHalfTime();

		pauseEvent.HalfTime();
		halfTime = true;
		
//		referee.PlayPlay();
		//Determine what half it is
		half = half + 1;
	}

	/// <summary>
	/// Starts the Timer.
	/// </summary>
	public void StartGame()
	{
		timer.Resume();
		gameGoing = true;
	}

	/// <summary>
	/// Stops the Timer.
	/// </summary>
	public void StopGame()
	{
		timer.Pause();
		gameGoing = false;
	}

	/// <summary>
	/// Check if game is going.
	/// </summary>
	/// <returns></returns>
	public bool GameIsGoing()
	{
		return gameGoing;
	}
}
