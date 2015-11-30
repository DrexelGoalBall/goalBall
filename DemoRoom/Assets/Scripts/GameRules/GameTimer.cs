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

    //Timer Variables
    Timer timer;
    public int halfLength = 120;
    int half = 1;
    bool endGame = false;
    bool gameGoing = false;

	// Use this for initialization
	void Start () {
        timer = gameObject.AddComponent<Timer>();
        timer.SetLengthOfTimer(halfLength + 1);
        timer.Pause();
        player1Start = player1.transform.position;
        player2Start = player2.transform.position;
        ballStart = ball.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (endGame) return;
        timeText.text = timer.getTimeString();
		if(timer.getTime() <= 0){
			nextHalf();
		}
	}

	void nextHalf(){
        if (half >= 2)
        {
            /*timeText.text = "GAME OVER";
            endGame = true;*/
            Application.LoadLevel("GameOver");
            return;
        }
        player1.transform.position = player1Start;
		player2.transform.position = player2Start;
		ball.transform.position = ballStart;

        ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        timer.Reset();

        //Determine what half it is
        half = half + 1;
    }

    public void StartGame()
    {
        timer.Resume();
        gameGoing = true;
    }

    public void StopGame()
    {
        timer.Pause();
        gameGoing = false;
    }

    public bool GameIsGoing()
    {
        return gameGoing;
    }
}
