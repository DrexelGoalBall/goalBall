﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
/// Foul System
/// This system keeps track of all of the fouls that can happen in a game and give the ball to the player that should get it.
/// Current fouls are:
/// Time foul (have possession of the ball for over 10 seconds)
/// Out of bounts (throw the ball out of bounds)
/// Dead Ball Ball stays in neutral area for longer than 3 seconds.
/// </summary>
public class Fouls : NetworkBehaviour 
{
    public BallReset BR;
	public GameObject ball;
    public Possession ballPossession;
    public ListObjectLocation ballLocation;
    public GameTimer GT;
    public BreakTimer BT;

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

    /// <summary>
    /// Initializes all of the objects and variables needed for this script.
    /// </summary>
    void Start () 
    {
        GameObject GameController = GameObject.FindGameObjectWithTag("GameController");
        REF = GameObject.FindGameObjectWithTag("Referee").GetComponent<Referee>();
        BR = GameController.GetComponent<BallReset>();
        GT = GameController.GetComponent<GameTimer>();
        BT = GameObject.Find("BreakTimer").GetComponent<BreakTimer>();
        
        ballPossession = ball.GetComponent<Possession>();
        ballLocation = ball.GetComponent<ListObjectLocation>();
        timer = gameObject.AddComponent<Timer>();
        timer.SetLengthOfTimer(TimerFoulTime);
        timer.Resume();
        DeadBallTimer = gameObject.AddComponent<Timer>();
        DeadBallTimer.SetLengthOfTimer(DeadBallTime);
        DeadBallTimer.Pause();
	}

    /// <summary>
    /// Main function that checks whether a foul has been made in real time.  Keeps a timer running for checking fouls.
    /// </summary>
    void Update () 
    {
        if (!isServer)
            return;

        string location = ballLocation.currentArea;
        
        //TIMER FOUL
        if (!GT.GameIsGoing())
        {
            timer.Reset();
            timer.Pause();
            return;
        }
        if ((location == RedTeamArea && PreviousArea == BlueTeamArea) || (location == BlueTeamArea && PreviousArea == RedTeamArea))
        {
            // Reset timer if ball has moved from red to blue/blue to red zone
            timer.Reset();
            timer.Pause();
        }
        else if (location == RedTeamArea || location == BlueTeamArea)
        {
            // Continue timer while in the same zone
            timer.Resume();
            PreviousArea = location;

            if (timer.getTime() < 0)
            {
                ThrowTimeFoul(location == RedTeamArea);
                timer.Reset();
                timer.Pause();
            }
        }
        else
        {
            // Pause the timer when the ball is not in either team's zone
            timer.Pause();
        }

        //DEADBALL
        if (ball.transform.parent == null && (location == RedNeutralArea || location == BlueNeutralArea))
        {
            DeadBallTimer.Resume();

            if (DeadBallTimer.getTime() < 0)
            {
                ThrowDeadBallFoul();
                DeadBallTimer.Pause();
                DeadBallTimer.Reset();
            }
        }
        else
        {
            DeadBallTimer.Pause();
            DeadBallTimer.Reset();
        }
	}

    /// <summary>
    /// Calls the LineOut foul and all associated actions.
    /// </summary>
	public void LineOut()
    {
        if (!isServer)
            return;

        REF.PlayLineOut();

        Possession.Team possession = ballPossession.HasPossessionOfBall();
        if (possession == Possession.Team.red)
        {
            REF.PlayRedTeam();
            foul(true, true);
        }
        else
        {
            REF.PlayBlueTeam();
            foul(false, true);
        }

        BT.StartBreak(new FoulBreak());
	}

    /// <summary>
    /// Throw the time Foul and all associated actions.
    /// </summary>
    /// <param name="inRedZone">Whether the foul took place in the red zone or not</param>
	void ThrowTimeFoul(bool inRedZone)
    {
        if (!isServer)
            return;

        REF.PlayFoul();

        if (inRedZone)
        {
            REF.PlayRedTeam();
            foul(true, false);
        }
        else
        {
            REF.PlayBlueTeam();
            foul(false, false);
        }

        BT.StartBreak(new FoulBreak());
    }

    /// <summary>
    /// Throw DeadBall Foul and all associated actions.
    /// </summary>
    void ThrowDeadBallFoul()
    {
        if (!isServer)
            return;

        REF.PlayDeadBall();

        Possession.Team possession = ballPossession.HasPossessionOfBall();
        if (possession == Possession.Team.red)
        {
            REF.PlayRedTeam();
            foul(true, false);
        }
        else
        {
            REF.PlayBlueTeam();
            foul(false, false);
        }

        BT.StartBreak(new FoulBreak());
    }

    /// <summary>
    /// Activates the move of the foul and makes the Referee call play at the end.ss
    /// </summary>
    /// <param name="committedByRed"></param>
    /// <param name="resetToClosest">Whether to reset ball to closest position or center</param>
    private void foul(bool committedByRed, bool resetToClosest)
    {
        if (!isServer)
            return;
        
        if (committedByRed)
        {
            if (resetToClosest)
                BR.resetToClosestPoint(false);
            else
                BR.placeBallBSC();
        }
        else
        {
            if (resetToClosest)
                BR.resetToClosestPoint(true);
            else
                BR.placeBallRSC();
        }
    }
}
