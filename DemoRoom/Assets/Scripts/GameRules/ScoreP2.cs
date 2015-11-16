using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreP2 : MonoBehaviour {

	public Text p2score;
	int score;
	// Use this for initialization
	void Start () {
		score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		string newScore = score.ToString();
		p2score.text = newScore;
	}

	public void addScore(){
		score++;
		print("Added: " + score);
	}

	public void subScore(){
		score--;
	}
}
