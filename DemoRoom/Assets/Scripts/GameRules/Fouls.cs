using UnityEngine;
using System.Collections;

public class Fouls : MonoBehaviour {

    /// <summary>
    /// Foul System
    /// This system keeps track of all of the fouls that can happen in a game and give the ball to the player that should get it.
    /// Current fouls are:
    /// Time foul (have possession of the ball for over 10 seconds)
    /// Out of bounts (throw the ball out of bounds)
    /// Dead Ball Ball stays in neutral area for longer than 3 seconds.
    /// </summary>

    public BallReset BR;
	public GameObject ball;
    public Possession ballPossession;
    public ListObjectLocation ballLocation;
    public GameTimer GT;

    //AreaNames
    public string RedTeamArea = "RedTeamArea";
    public string BlueTeamArea = "BlueTeamArea";
    public string RedNeutralArea = "RedNeutralArea";
    public string BlueNeutralArea = "BlueNeutralArea";

    private string PreviousArea;
	
    //Timer
    Timer timer;
    Timer DeadBallTimer;
    public int DeadBallTime = 3;
    public int TimerFoulTime = 10;

    //Other GameObjects Scripts
    private Referee REF;

	// Use this for initialization
	void Start () {
        GameObject GameController = GameObject.FindGameObjectWithTag("GameController");
        REF = GameObject.FindGameObjectWithTag("Referee").GetComponent<Referee>();
        BR = GameController.GetComponent<BallReset>();
        GT = GameController.GetComponent<GameTimer>();
        
        ballPossession = ball.GetComponent<Possession>();
        ballLocation = ball.GetComponent<ListObjectLocation>();
        timer = gameObject.AddComponent<Timer>();
        timer.SetLengthOfTimer(TimerFoulTime);
        timer.Resume();
        DeadBallTimer = gameObject.AddComponent<Timer>();
        DeadBallTimer.SetLengthOfTimer(DeadBallTime);
        DeadBallTimer.Pause();
	}
	
	// Update is called once per frame
	void Update () {
        string location = ballLocation.currentArea;
        string possession = ballPossession.HasPossessionOfBall();
        //TIMER FOUL
        if (!GT.GameIsGoing())
        {
            timer.Reset();
            timer.Pause();
            return;
        }
        if (location == RedTeamArea || location == BlueTeamArea)
        {
            timer.Resume();
            PreviousArea = location;
        }
        else if ((location == RedTeamArea && PreviousArea == BlueTeamArea) || (location == BlueTeamArea && PreviousArea == RedTeamArea))
        {
            timer.Reset();
            timer.Pause();
        }
        if (timer.getTime() < 0)
        {
            ThrowTimeFoul();
		}

        //DEADBALL
        if (location == RedNeutralArea || location == BlueNeutralArea)
        {
            DeadBallTimer.Resume();
        }
        else
        {
            DeadBallTimer.Pause();
            DeadBallTimer.Reset();

        }
        if (DeadBallTimer.getTime() < 0)
        {
            ThrowDeadBallFoul();
            DeadBallTimer.Pause();
            DeadBallTimer.Reset();
        }
	}

	public void LineOut(){
        string possession = ballPossession.HasPossessionOfBall();
        print("Line Out");
        REF.PlayLineOut();
        if (possession == "Red")
        {
            REF.PlayRedTeam();
            foul(true);
        }
        if (possession == "Blue")
        {
            REF.PlayBlueTeam();
            foul(false);
        }
	}

	void ThrowTimeFoul(){
		print("Throw Time Foul");
        string possession = ballPossession.HasPossessionOfBall();
        REF.PlayFoul();
        if (possession == "Red")
        {
            REF.PlayRedTeam();
            foul(true);
        }
        if (possession == "Blue")
        {
            REF.PlayBlueTeam();
            foul(false);
        }
    }

    void ThrowDeadBallFoul()
    {
        string possession = ballPossession.HasPossessionOfBall();
        REF.PlayDeadBall();
        if (possession == "Red")
        {
            REF.PlayRedTeam();
            foul(true);
        }
        if (possession == "Blue")
        {
            REF.PlayBlueTeam();
            foul(false);
        }
    }

	public void foul(bool isRed)
    {
        //WILL HAVE TO NETWORK THIS SO I AM GETTING RID OF IT FOR NOW
       // if (isRed)
       // {
       //     BR.placeBallBSC();
       // }
       // else
       // {
       //     BR.placeBallRSC();
       // }

        REF.PlayPlay();
    }
}
