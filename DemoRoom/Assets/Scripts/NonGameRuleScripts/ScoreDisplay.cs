using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour {

    //Get necessary items
    public ScoreKeeper scoreKeeper;
    public Text RedScore;
    public Text BlueScore;

    // Use this for initialization
    void Start () {
        scoreKeeper = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreKeeper>();
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        RedScore.text = scoreKeeper.RedScoreString();
        BlueScore.text = scoreKeeper.BlueScoreString();	
	}
}
