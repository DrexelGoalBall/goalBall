﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>
/// This script keeps track of the score of the game and makes this information available to other objects.
/// </summary>
public class ScoreKeeper: NetworkBehaviour {
    //Display Variables
    [SyncVar]
	public int BlueTeamScore;
    [SyncVar]
    public int RedTeamScore;

    //MessagesToUpdate
    private GameTimer GT;
    private BallReset BR;
    private Referee Ref;
    private BreakTimer BT;

    /// <summary>
    /// Gets the objects and initializes the variables to their proper values.
    /// </summary>
    void Start ()
    {
        if (isServer)
        {
            BlueTeamScore = 0;
            RedTeamScore = 0;
        }
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");

        GT = gameController.GetComponent<GameTimer>();
        BR = gameController.GetComponent<BallReset>();
        Ref = GameObject.FindGameObjectWithTag("Referee").GetComponent<Referee>();
        BT = GameObject.Find("BreakTimer").GetComponent<BreakTimer>();
	}

    /// <summary>
    /// Adds points for the blue team and has the Referee tell the players who scored.
    /// </summary>
    public void BlueTeamScored()
    {
        if (isServer)
        {
            BlueTeamScore++;
            Ref.PlayGoal();
            Ref.PlayBlueTeam();
            //Ref.PlayPlay();
            BR.resetToClosestPoint(true);
            if (GT.InOvertime())
            {
                GT.ServerEndTheGame();
            }
            else
            {
                BT.StartBreak(new GoalBreak());
            }
        }
        
		print("Added: " + BlueTeamScore);
	}

    /// <summary>
    /// Subract a point from the blue team.
    /// </summary>
	public void BlueTeamLostPoint()
    {
        if (isServer)
        {
            BlueTeamScore--;
        }
	}

    /// <summary>
    /// Adds points for the red team and has the Referee tell the players who scored.
    /// </summary>
    public void RedTeamScored()
    {
        if (isServer)
        {
            RedTeamScore++;
            Ref.PlayGoal();
            Ref.PlayRedTeam();
            //Ref.PlayPlay();
            BR.resetToClosestPoint(false);
            if (GT.InOvertime())
            {
                GT.ServerEndTheGame();
            }
            else
            {
                BT.StartBreak(new GoalBreak());
            }
        }
        
        print("Added: " + RedTeamScore);
    }

    /// <summary>
    /// Subrtacts a point from the red teams score.
    /// </summary>
    public void RedTeamLostPoint()
    {
        if (isServer)
        {
            RedTeamScore--;
        }
    }

    //Getters
    /// <summary>
    /// Returns the Score of the red team as a string.
    /// </summary>
    /// <returns></returns>
    public string RedScoreString()
    {
        return RedTeamScore.ToString();
    }

    /// <summary>
    /// Returns the score of the blue team as a string.
    /// </summary>
    /// <returns></returns>
    public string BlueScoreString()
    {
        return BlueTeamScore.ToString();
    }

    /// <summary>
    /// Returns the score of the red team as an int.
    /// </summary>
    /// <returns></returns>
    public int RedScore()
    {
        return RedTeamScore;
    }

    /// <summary>
    /// Returns the score of the blue team as an int.
    /// </summary>
    /// <returns></returns>
    public int BlueScore()
    {
        return BlueTeamScore;
    }
}
