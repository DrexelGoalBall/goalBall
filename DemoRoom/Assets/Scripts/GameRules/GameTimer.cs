using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {

	public Text timeText;
	public GameObject player1;
	public GameObject player2;
	public GameObject ball;
	float xp1, xp2, xp3, yp1, yp2, yp3, zp1, zp2, zp3;
	int time;
	int frameCounter = 0;

	// Use this for initialization
	void Start () {
		time = 120;
		xp1 = player1.transform.position.x;
		xp2 = player2.transform.position.x;
		xp3 = ball.transform.position.x;
		yp1 = player1.transform.position.y;
		yp2 = player2.transform.position.y;
		yp3 = ball.transform.position.y;
		zp1 = player1.transform.position.z;
		zp2 = player2.transform.position.z;
		zp3 = ball.transform.position.z;
	}
	
	// Update is called once per frame
	void Update () {
		if(frameCounter == 60){
			frameCounter = 0;
			time--;
		}
		else{
			frameCounter++;
		}

		string mins;
		if(time == 120)
			mins = "2";
		else if(time < 120 && time > 59)
			mins = "1";
		else
			mins = "0";

		int secCalc = time % 60;
		string secs = buildMins(secCalc);
		string newTime = mins + " : " + secs;
		
		timeText.text = newTime;

		if(time == 0){
			reset();
		}
	}

	string buildMins(int time){
		string rtn;
		if(time == 9)
			rtn = "09";
		else if(time == 8)
			rtn = "08";
		else if(time == 7)
			rtn = "07";
		else if(time == 6)
			rtn = "06";
		else if(time == 5)
			rtn = "05";
		else if(time == 4)
			rtn = "04";
		else if(time == 3)
			rtn = "03";
		else if(time == 2)
			rtn = "02";
		else if(time == 1)
			rtn = "01";
		else if(time == 0)
			rtn = "00";
		else
			rtn = time.ToString();

		return rtn;
	}

	void reset(){
		Vector3 newP1 = new Vector3(xp1, yp1, zp1);
		Vector3 newP2 = new Vector3(xp2, yp2, zp2);
		Vector3 newB = new Vector3(xp3, yp3, zp3);

		player1.transform.position = newP1;
		player2.transform.position = newP2;
		ball.transform.position = newB;
		time = 120;
		frameCounter = 0;
	}
}
