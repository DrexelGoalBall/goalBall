using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {

    //GUI Objects
    public Text timeText;

    //Players
    public GameObject player1;
	public GameObject player2;
	public GameObject ball;

    //Initial Player positions
    private Vector3 player1Start;
    private Vector3 player2Start;
    private Vector3 ballStart;

    private float time;
    public int halfLength = 120;

    int half = 1;

    bool inGame = true;

	// Use this for initialization
	void Start () {
        time = halfLength;
        player1Start = player1.transform.position;
        player2Start = player2.transform.position;
        ballStart = ball.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (!inGame) return;

        time -= Time.deltaTime;

        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        timeText.text = string.Format("{0}:{1:00}", minutes, seconds);

		if(time <= 0){
			reset();
		}
	}

	void reset(){
		player1.transform.position = player1Start;
		player2.transform.position = player2Start;
		ball.transform.position = ballStart;

        ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		time = halfLength;

        //Determine what half it is
        half = half + 1;

        if (half > 2)
        {
            timeText.text = "GAME OVER";
            inGame = false;
        }
    }
}
