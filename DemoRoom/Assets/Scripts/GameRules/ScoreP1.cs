using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ScoreP1 : MonoBehaviour {

	public Text p1score;
	int score;

	// Use this for initialization
	void Start () {
		score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		string newScore = score.ToString();
		p1score.text = newScore;
	}

	public void addScore(){
		score++;
		print("Added: " + score);
	}

	public void subScore(){
		score--;
	}
}
