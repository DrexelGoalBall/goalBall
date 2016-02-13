using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

    /// <summary>
    /// This script starts up the game and activates everything that needs to be activated for a game to start.
    /// </summary>

    //Scripts
    private CoinFlip CF;

    //Neccessary Components
    private BallReset BR;
    private Referee Ref;
    private GameTimer GT;

    //Ball
    public GameObject ball;

    //Values
    int rValue = 0;
    int bValue = 1;

    //Checks
    bool check = true;
    bool setupDone = false;

    int startPos;

    /// <summary>
    /// Initializes all variables needed to run the script.
    /// </summary>
    void Start ()
    {
        GameObject GameController = GameObject.FindGameObjectWithTag("GameController");
        BR = GameController.GetComponent<BallReset>();
        GT = GameController.GetComponent<GameTimer>();
        Ref = GameObject.FindGameObjectWithTag("Referee").GetComponent<Referee>();
        Ref.PlayQuietPlease();

        CF = new CoinFlip();
        if (CF.Flip())
        {
            BR.placeBallRSC();
            ball.GetComponent<Possession>().RedTeamPossession();
            startPos = 0;
            Ref.PlayRedTeam();
            Ref.PlayCenter();
            Ref.PlayPlay();
        }
        else
        {
            BR.placeBallBSC();
            ball.GetComponent<Possession>().BlueTeamPossession();
            startPos = 1;
            Ref.PlayBlueTeam();
            Ref.PlayCenter();
            Ref.PlayPlay();
        }
        setupDone = true;
    }

    /// <summary>
    /// Checks if the game is ready to start and the referee has finished speaking.
    /// </summary>
    void Update()
    {
        if (!Ref.refereeSpeaking() && check && setupDone)
        {
            GT.StartGame();
            check = false;
        }
    }
}
