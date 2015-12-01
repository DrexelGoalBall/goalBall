using UnityEngine;
using System.Collections;

public class BallStartAndReset : MonoBehaviour {

    //Scripts
    private CoinFlip CF;

    //Locations
    public GameObject RedSide;
    public GameObject BlueSide;

    //Ball
    public GameObject ball;

    //Values
    int rValue = 0;
    int bValue = 1;

	// Use this for initialization
	void Start ()
    {
        CF = new CoinFlip();
        if (CF.Flip())
        {
            ball.transform.position = RedSide.transform.position;
            gameObject.GetComponent<Fouls>().possession = 1;
        }
        else
        {
            ball.transform.position = BlueSide.transform.position;
            gameObject.GetComponent<Fouls>().possession = 2;
        }
    }

    public void ScoreWasMade(int whoScored)
    {
        if (whoScored == rValue)
        {
            ball.transform.position = BlueSide.transform.position;
            ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
        else
        {
            ball.transform.position = RedSide.transform.position;
            ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
