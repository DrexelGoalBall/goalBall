using UnityEngine;
using System.Collections;

public class RedGoal : MonoBehaviour {

    public GameObject ball;
    public ScoreKeeper scoreKeeper;
    bool inside;
    // Use this for initialization
    void Start()
    {
        scoreKeeper = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreKeeper>();
        inside = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Ball" && !inside)
        {
            scoreKeeper.RedTeamScored();
            inside = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Ball" && inside)
        {
            inside = false;
        }
    }
}
