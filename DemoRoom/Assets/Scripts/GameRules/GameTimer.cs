using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameTimer : NetworkBehaviour {

    [SyncVar]
    int time;

	public Text timeText;
	public GameObject ball, ballSpawn;
    //int time;
	int frameCounter = 0;

	// Use this for initialization
	void Start () {
        if (isServer)
        {
            time = 120;
        }
        ballSpawn = GameObject.Find("BallSpawn");
	}
	
	// Update is called once per frame
	void Update () {
        if (isServer)
        {
            //if (Input.GetKeyDown(KeyCode.O))
            //    time = 5;

            if (frameCounter >= 60)
            {
                frameCounter = 0;
                time--;
            }
            else
            {
                frameCounter++;
            }
        }

        if (time <= 0 || (!isServer && isClient && time <= 1))
        {
            reset();
        }

        string mins;
        if (time == 120)
            mins = "2";
        else if (time < 120 && time > 59)
            mins = "1";
        else
            mins = "0";

        int secCalc = time % 60;
        string secs = buildMins(secCalc);
        string newTime = mins + " : " + secs;

		timeText.text = newTime;
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
        ball.transform.parent = null;
        if (isServer)
        {
            GameObject obj = GameObject.Instantiate(ball, ballSpawn.transform.position, ballSpawn.transform.rotation) as GameObject;
            NetworkServer.Destroy(ball);
            NetworkServer.Spawn(obj);
            obj.name = "Ball";
            ball = obj;

            time = 120;
            frameCounter = 0;
        }

        // There must be an easier way.....
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(players.Length);
        foreach(GameObject player in players)
        {
            if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                Debug.Log("Found");
                player.GetComponent<CatchThrowV1>().DropBall();
                break;
            }
        }
	}
}
