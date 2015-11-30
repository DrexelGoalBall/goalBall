using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ScoreKeeper: MonoBehaviour {

    //Display Variables
	public int BlueTeamScore;
    public int RedTeamScore;

    //MessagesToUpdate
    private BallReset BR;

	// Use this for initialization
	void Start ()
    {
		BlueTeamScore = 0;
        RedTeamScore = 0;
        BR = GameObject.FindGameObjectWithTag("GameController").GetComponent<BallReset>();
	}
	
    //Setters
	public void BlueTeamScored()
    {
		BlueTeamScore++;
        BR.placeBallRSC();
		print("Added: " + BlueTeamScore);
	}

	public void BlueTeamLostPoint()
    {
		BlueTeamScore--;
	}

    public void RedTeamScored()
    {
        RedTeamScore++;
        BR.placeBallBSC();
        print("Added: " + RedTeamScore);
    }

    public void RedTeamLostPoint()
    {
        RedTeamScore--;
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
