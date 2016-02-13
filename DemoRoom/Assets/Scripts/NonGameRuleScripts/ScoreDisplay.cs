using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// This script displays the score in the GUI Text object.
/// </summary>
public class ScoreDisplay : MonoBehaviour {
    //Get necessary items
    public ScoreKeeper scoreKeeper;
    public Text RedScore;
    public Text BlueScore;

    /// <summary>
    /// Initializes variables needed for this script.
    /// </summary>
    void Start () {
        scoreKeeper = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreKeeper>();
	
	}

    /// <summary>
    /// Updates the score with the current values of scoreKeeper.
    /// </summary>
    void Update ()
    {
        RedScore.text = scoreKeeper.RedScoreString();
        BlueScore.text = scoreKeeper.BlueScoreString();	
	}
}
