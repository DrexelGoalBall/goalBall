using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ScoreDisplay : MonoBehaviour {

	public Text p1score;
    public Text p2score;
	int scorep1;
    int scorep2;

	// Use this for initialization
	void Start ()
    {
		scorep1 = 0;
        scorep2 = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
		string newScore = scorep1.ToString();
		p1score.text = newScore;
        string newScore2 = scorep2.ToString();
        p2score.text = newScore2;
    }

	public void addScoreP1()
    {
		scorep1++;
		print("Added: " + scorep1);
	}

	public void subScoreP1()
    {
		scorep1--;
	}

    public void addScoreP2()
    {
        scorep2++;
        print("Added: " + scorep2);
    }

    public void subScoreP2()
    {
        scorep2--;
    }
}
