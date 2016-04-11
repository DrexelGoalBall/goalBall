using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>
/// This script controls the game times and knows when it should be going and knows when it should be stopped.
/// </summary>
public class GameTimer : NetworkBehaviour 
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
    bool overtime = false;
    bool fifteenSecondsCheck = false;

    [SyncVar]
    bool gameGoing = false;
    [SyncVar(hook = "ClientGameEnd")]
    bool endGame = false;

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
        breakTimer = GameObject.Find("BreakTimer").GetComponent<BreakTimer>();
	}

    /// <summary>
    /// Keeps track of the Timer and causes referee to say certain things as the time progresses.
    /// </summary>
    void Update () 
    {
        if (endGame) 
            return;

        timeText.text = timer.getTimeString();

        if (isServer)
        {
            if (breakTimer.OnBreak())
            {
                StopGame();
            }
            else
            {
                if (!gameGoing)
                {
                    StartGame();
                }

                if (timer.getTime() <= 0)
                {
                    nextHalf();
                }

                if (timer.getTime() <= 15 && !fifteenSecondsCheck)
                {
                    referee.PlayFifteenSeconds();
                    fifteenSecondsCheck = true;
                }
            }
        }
	}

    /// <summary>
    /// Resets the timer and increments the half variable.
    /// </summary>
	void nextHalf()
    {
        Debug.Log("Half " + half);

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
                    timer.SetLengthOfTimer(overtimeHalfLength + 1);
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
                StopGame();
                ServerEndTheGame();
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
            // Read red score
            referee.PlayRedTeam();
            referee.ReadScore(scoreKeeper.RedScore());
            // Read blue score
            referee.PlayBlueTeam();
            referee.ReadScore(scoreKeeper.BlueScore());
        }

        timer.Reset();
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

    /// <summary>
    /// Check if game is in overtime.
    /// </summary>
    /// <returns></returns>
    public bool InOvertime()
    {
        return overtime;
    }

    /// <summary>
    /// Check if game is over.
    /// </summary>
    /// <returns></returns>
    public bool GameHasEnded()
    {
        return endGame;
    }

    /// <summary>
    /// The game is over so let all the clients know
    /// </summary>
    public void ServerEndTheGame()
    {
        if (isServer)
        {
            endGame = true;
        }
    }

    /// <summary>
    /// Server said the game is over.
    /// </summary>
    public void ClientGameEnd(bool end)
    {
        endGame = end;
    }
}
