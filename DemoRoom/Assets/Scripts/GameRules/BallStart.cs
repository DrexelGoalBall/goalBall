using UnityEngine;
using System.Collections;

public class BallStart : MonoBehaviour {

    //Scripts
    private CoinFlip CF;

    //BallReset
    public BallReset BR;

    //Ball
    public GameObject ball;

    //Values
    int rValue = 0;
    int bValue = 1;

	// Use this for initialization
	void Start ()
    {
        BR = GameObject.FindGameObjectWithTag("GameController").GetComponent<BallReset>();

        CF = new CoinFlip();
        if (CF.Flip())
        {
            BR.placeBallRSC();
            //gameObject.GetComponent<Fouls>().possession = 1;
        }
        else
        {
            BR.placeBallBSC();
            //gameObject.GetComponent<Fouls>().possession = 2;
        }
    }
}
