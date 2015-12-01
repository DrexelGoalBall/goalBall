using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ScoreKeeper: MonoBehaviour {

    //Display Variables
	int BlueTeamScore;
    int RedTeamScore;

    //MessagesToUpdate
    private BallReset BR;
    private Referee Ref;

	// Use this for initialization
	void Start ()
    {
		BlueTeamScore = 0;
        RedTeamScore = 0;
        BR = GameObject.FindGameObjectWithTag("GameController").GetComponent<BallReset>();
        Ref = GameObject.FindGameObjectWithTag("Referee").GetComponent<Referee>();
	}
	
    //Setters
	public void BlueTeamScored()
    {
		BlueTeamScore++;
        Ref.PlayGoal();
        Ref.PlayBlueTeam();
        Ref.PlayPlay();
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
        Ref.PlayGoal();
        Ref.PlayRedTeam();
        Ref.PlayPlay();
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
