using UnityEngine;
using System.Collections;

public class Fouls : MonoBehaviour {

	public int possession;		//1 for red team, 2 for blue


	public GameObject redBallLanding, blueBallLanding;
	public GameObject ball;

	//Foul possession locations
	Vector3 redStart, blueStart, ballStart;
	
	Vector3 stopVector;
    //Timer
    Timer timer;
    public int dogp = 10;


	// Use this for initialization
	void Start () {
		redStart = redBallLanding.transform.position;
		blueStart = blueBallLanding.transform.position;
		ballStart = ball.transform.position;
        timer = gameObject.AddComponent<Timer>();
        timer.SetLengthOfTimer(10);
		stopVector = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if(timer.getTime() < 0){
			ThrowTimeFoul();
		}
	}

	public void LineOut(){
		print("Line Out");
		foul();
	}

	void ThrowTimeFoul(){
		print("Throw Time Foul");
		foul();
	}

	public void foul(){
		if(possession == 1){
			ball.transform.position = blueStart;	
			ball.GetComponent<Rigidbody>().velocity = stopVector;
            timer.Reset();
            possession = 2;
		}
		else if(possession == 2){
			ball.transform.position = redStart;
			ball.GetComponent<Rigidbody>().velocity = stopVector;
            timer.Reset();
            possession = 1;
		}

	}
}
