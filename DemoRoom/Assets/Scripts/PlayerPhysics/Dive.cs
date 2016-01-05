using UnityEngine;
using System.Collections;

public class Dive : MonoBehaviour {

	public GameObject player;
	private bool diveGo = false;
	private bool isUpright = true;
	private int diveCount;

    public float diveSpeed = 10f;
    public string diveCommand = "DiveP1";
    public float ballSlowdown = .333f;
	//public GameObject diveMade;
	// Use this for initialization
	void Start () {
		diveCount = 0;
        player = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown(diveCommand)){
			if(!diveGo)
				diveGo = true;
		}
	}

	void FixedUpdate(){
		float diveDrop = .5f / diveSpeed;
        float diveRot = 90f / diveSpeed;
		if(diveGo && isUpright){
			player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x,
														player.transform.eulerAngles.y,
													player.transform.eulerAngles.z + diveRot);
			player.transform.position = new Vector3(player.transform.position.x,
													player.transform.position.y - diveDrop,
													player.transform.position.z);
			diveCount++;
		}
		if(diveGo && !isUpright){
			player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x,
														player.transform.eulerAngles.y,
														player.transform.eulerAngles.z - diveRot);
						player.transform.position = new Vector3(player.transform.position.x,
													player.transform.position.y + diveDrop,
													player.transform.position.z);
			diveCount++;
		}
		if(diveCount == (int)diveSpeed){
			diveGo = false;
			diveCount = 0;
			if(isUpright)
				isUpright = false;
			else if(!isUpright)
				isUpright = true;
		}
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ball" && !isUpright)
        {
            Rigidbody RB = col.gameObject.GetComponent<Rigidbody>();
            RB.velocity = ballSlowdown * RB.velocity;
            RB.angularVelocity = ballSlowdown * RB.angularVelocity;
        }
    }
}
