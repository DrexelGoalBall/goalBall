using UnityEngine;
using System.Collections;

public class Fouls : MonoBehaviour {

    /// <summary>
    /// Foul System
    /// This system keeps track of all of the fouls that can happen in a game and give the ball to the player that should get it.
    /// Current fouls are:
    /// Time foul (have possession of the ball for over 10 seconds)
    /// Out of bounts (throw the ball out of bounds)
    /// </summary>

    public BallReset BR;
	public GameObject ball;
    public Possession ballPossession;
    public ListObjectLocation ballLocation;

    //AreaNames
    public string RedTeamArea = "RedTeamArea";
    public string BlueTeamArea = "BlueTeamArea";
	
    //Timer
    Timer timer;
    public int dogp = 10;


	// Use this for initialization
	void Start () {
        BR = GameObject.FindGameObjectWithTag("GameController").GetComponent<BallReset>();
        ballPossession = ball.GetComponent<Possession>();
        ballLocation = ball.GetComponent<ListObjectLocation>();
        timer = gameObject.AddComponent<Timer>();
        timer.SetLengthOfTimer(10);
        timer.Resume();
	}
	
	// Update is called once per frame
	void Update () {
        string location = ballLocation.currentArea;
        string possession = ballPossession.HasPossessionOfBall();
        if (location == RedTeamArea || location == BlueTeamArea)
        {
            timer.Resume();
        }
        else if (location != RedTeamArea || location != BlueTeamArea)
        {
            timer.Reset();
            timer.Pause();
        }
        if (timer.getTime() < 0)
        {
            ThrowTimeFoul();
		}
	}

	public void LineOut(){
        string possession = ballPossession.HasPossessionOfBall();
        print("Line Out");
        if (possession == "Red")
        {
            foul(true);
        }
        if (possession == "Blue")
        {
            foul(false);
        }
	}

	void ThrowTimeFoul(){
		print("Throw Time Foul");
        string possession = ballPossession.HasPossessionOfBall();
        if (possession == "Red")
        {
            foul(true);
        }
        if (possession == "Blue")
        {
            foul(false);
        }
    }

	public void foul(bool isRed)
    {
        if (isRed)
        {
            BR.placeBallRSC();
        }
        else
        {
            BR.placeBallBSC();
        }
    }
}
