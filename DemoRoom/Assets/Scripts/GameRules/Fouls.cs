using UnityEngine;
using System.Collections;

public class Fouls : MonoBehaviour {

	public int possession;		//1 for red team, 2 for blue


	public GameObject redBallLanding, blueBallLanding;
	public GameObject ball;

	//Foul possession locations
	Vector3 redStart, blueStart, ballStart;
	
	Vector3 stopVector;
	int frameTimer;				// Counts the throw timing

	// Use this for initialization
	void Start () {
		redStart = redBallLanding.transform.position;
		blueStart = blueBallLanding.transform.position;
		ballStart = ball.transform.position;
		
		stopVector = new Vector3(0, 0, 0);
		frameTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(frameTimer == 600){
			ThrowTimeFoul();
		}
		frameTimer++;
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
            ball.transform.position = redStart;
			ball.GetComponent<Rigidbody>().velocity = stopVector;
			frameTimer = 0;
			possession = 2;
		}
		else if(possession == 2){
            ball.transform.position = blueStart;
			ball.GetComponent<Rigidbody>().velocity = stopVector;
			frameTimer = 0;
			possession = 1;
		}

	}
}
