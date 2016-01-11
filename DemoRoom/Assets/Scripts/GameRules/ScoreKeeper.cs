using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ScoreKeeper: NetworkBehaviour {

    //Display Variables
    [SyncVar]
	public int BlueTeamScore;
    [SyncVar]
    public int RedTeamScore;

    //MessagesToUpdate
    private BallReset BR;
    private Referee Ref;

	// Use this for initialization
	void Start ()
    {
        if (isServer)
        {
            BlueTeamScore = 0;
            RedTeamScore = 0;
        }
        BR = GameObject.FindGameObjectWithTag("GameController").GetComponent<BallReset>();
        Ref = GameObject.FindGameObjectWithTag("Referee").GetComponent<Referee>();
	}
	
    //Setters
	public void BlueTeamScored()
    {
        if (isServer)
        {
            BlueTeamScore++;
        }
        Ref.PlayGoal();
        Ref.PlayBlueTeam();
        Ref.PlayPlay();
        BR.placeBallRSC();
		print("Added: " + BlueTeamScore);
	}

	public void BlueTeamLostPoint()
    {
        if (isServer)
        {
            BlueTeamScore--;
        }
	}

    public void RedTeamScored()
    {
        if (isServer)
        {
            RedTeamScore++;
        }
        Ref.PlayGoal();
        Ref.PlayRedTeam();
        Ref.PlayPlay();
        BR.placeBallBSC();
        print("Added: " + RedTeamScore);
    }

    public void RedTeamLostPoint()
    {
        if (isServer)
        {
            RedTeamScore--;
        }
    }

    //Getters
    public string RedScoreString()
    {
        return RedTeamScore.ToString();
    }
    public string BlueScoreString()
    {
        return BlueTeamScore.ToString();
    }
    public int RedScore()
    {
        return RedTeamScore;
    }
    public int BlueScore()
    {
        return BlueTeamScore;
    }
}
