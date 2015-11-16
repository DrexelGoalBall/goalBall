using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ScoreDisplay : NetworkBehaviour {

    [SyncVar]
    int scorep1;
    [SyncVar]
    int scorep2;

	public Text p1score;
    public Text p2score;

	// Use this for initialization
	void Start ()
    {
        if (isServer)
        {
            scorep1 = 0;
            scorep2 = 0;
        }
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
        if (isServer)
        {
            scorep1++;
            print("Added: " + scorep1);
        }
	}

	public void subScoreP1()
    {
        if (isServer)
        {
            scorep1--;
        }
	}

    public void addScoreP2()
    {
        if (isServer)
        {
            scorep2++;
            print("Added: " + scorep2);
        }
    }

    public void subScoreP2()
    {
        if (isServer)
        {
            scorep2--;
        }
    }
}
