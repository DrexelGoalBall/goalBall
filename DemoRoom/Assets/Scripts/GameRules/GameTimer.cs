using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// This script controls the game times and knows when it should be going and knows when it should be stopped.
/// </summary>
public class GameTimer : MonoBehaviour 
{
    //GUI Objects
    public Text timeText;
    
    // 
	public GameObject ball;

    //Timer Variables
    Timer timer;
    public int halfLength = 240;
    public int overtimeHalfLength = 180;
    int half = 1;
    bool endGame = false;
    public bool gameGoing = false;
    bool overtime = false;
    bool fifteenSecondsCheck = false;
    
    //Other Objects
    private Referee referee;
    private ScoreKeeper scoreKeeper;
    private BallReset ballReset;
    private BreakTimer breakTimer;

    /// <summary>
    /// Initialize all variables needed for this script to run.
    /// </summary>
    void Start () 
    {
        timer = GetComponent<Timer>();
        timer.SetLengthOfTimer(halfLength + 1);
        timer.Pause();
        referee = GameObject.FindGameObjectWithTag("Referee").GetComponent<Referee>();
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        scoreKeeper = gameController.GetComponent<ScoreKeeper>();
        ballReset = gameController.GetComponent<BallReset>();
        breakTimer = gameController.GetComponent<BreakTimer>();
	}

    /// <summary>
    /// Keeps track of the Timer and causes referee to say certain things as the time progresses.
    /// </summary>
    void Update () 
    {
        if (endGame) 
            return;

        if (breakTimer.OnBreak())
        {
            StopGame();
        }
        else
        {
            timeText.text = timer.getTimeString();

            if (timer.getTime() <= 0)
            {
                nextHalf();
            }

            //if (referee.refereeSpeaking())
            //{
            //    StopGame();
            //}
            //else if (!gameGoing)
            if (!gameGoing)
            {
                StartGame();
            }

            if (timer.getTime() <= 15 && !fifteenSecondsCheck)
            {
                referee.PlayFifteenSeconds();
                fifteenSecondsCheck = true;
            }
        }
	}

    /// <summary>
    /// Resets the timer and increments the half variable.
    /// </summary>
	void nextHalf()
    {
        if (half >= 2)
        {
            if (scoreKeeper.BlueScore() == scoreKeeper.RedScore())
            {
                if (overtime)
                {
                    // SHOOTOUT
                }
                else
                {
                    overtime = true;
                    timer.SetLengthOfTimer(overtimeHalfLength);
                    Possession poss = ball.GetComponent<Possession>();
                    if (new CoinFlip().Flip())
                    {
                        poss.SetNextToGetBall(Possession.Team.blue);
                        ballReset.placeBallRSC();
                    }
                    else
                    {
                        poss.SetNextToGetBall(Possession.Team.red);
                        ballReset.placeBallBSC();
                    }

                    half = 1;

                    breakTimer.StartBreak(BreakTimer.Type.overtime);
                    referee.PlayOvertime();
                }
            }
            else
            {
                //timeText.text = "GAME OVER";
                endGame = true;
                Application.LoadLevel("GameOver");
                return;
            }
        }
        else
        {
            if (ball.GetComponent<Possession>().GetNextToGetBall() == Possession.Team.red)
                ballReset.placeBallRSC();
            else
                ballReset.placeBallBSC();

            // Update what half it is
            half++;

            breakTimer.StartBreak(BreakTimer.Type.halftime);
            referee.PlayHalfTime();
        }

        ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        timer.Reset();
        //referee.PlayPlay();
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
