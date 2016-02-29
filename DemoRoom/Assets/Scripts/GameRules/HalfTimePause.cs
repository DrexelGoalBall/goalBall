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

//		ball.SetActive(false);

		// Read red score
		referee.PlayRedTeam();
		ReadScore(scoreKeeper.RedScore());

		// Read blue score
		referee.PlayBlueTeam();
		ReadScore(scoreKeeper.BlueScore());	
	}

	/// <summary>
	/// Takes the current scores and announces them in audio.
	/// </summary>
	void ReadScore(int scoreToRead)
	{
		string fullScore = scoreToRead.ToString();
		int ones = fullScore[fullScore.Length-1] - '0';
		int tens = fullScore.Length > 1 ? fullScore[fullScore.Length-2] - '0' : 0;

		switch (tens)
		{
			case 0:
				break;

			case 1:
				Debug.Log("Ten");
//				referee.PlayTen();
				break;

			case 2:
				Debug.Log("Twenty");
//				referee.PlayTwenty();
				break;

			case 3:
				Debug.Log("Thirty");
//				referee.PlayThirty();
				break;

			case 4:
				Debug.Log("Fourty");
//				referee.PlayFourty();
				break;

			case 5:
				Debug.Log("Fifty");
//				referee.PlayFifty();
				break;

			case 6:
				Debug.Log("Sixty");
//				referee.PlaySixty();
				break;

			case 7:
				Debug.Log("Seventy");
//				referee.PlaySeventy();
				break;

			case 8:
				Debug.Log("Eighty");
//				referee.PlayEighty();
				break;

			case 9:
				Debug.Log("Ninety");
//				referee.PlayNinety();
				break;
		}

		switch (ones)
		{
			case 0:
				Debug.Log("Zero");
				referee.PlayZero();	
				break;

			case 1:
				Debug.Log("One");
				referee.PlayOne();
				break;

			case 2:
				Debug.Log("Two");
//				referee.PlayTwo();
				break;

			case 3:
				Debug.Log("Three");
//				referee.PlayThree();
				break;

			case 4:
				Debug.Log("Four");
//				referee.PlayFour();
				break;

			case 5:
				Debug.Log("Five");
//				referee.PlayFive();
				break;

			case 6:
				Debug.Log("Six");
//				referee.PlaySix();
				break;

			case 7:
				Debug.Log("Seven");
//				referee.PlaySeven();
				break;

			case 8:
				Debug.Log("Eight");
//				referee.PlayEight();
				break;

			case 9:
				Debug.Log("Nine");
//				referee.PlayNine();
				break;
		}
	}
}
