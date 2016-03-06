using UnityEngine;
using System.Collections;

/// <summary>
/// This script starts up the game and activates everything that needs to be activated for a game to start.
/// </summary>
public class GameStart : MonoBehaviour {
    //Scripts
    private CoinFlip CF;

    //Neccessary Components
    private BallReset BR;
    private Referee Ref;
    private GameTimer GT;
    private BreakTimer BT;

    //Ball
    public GameObject ball;

    //Values
    int rValue = 0;
    int bValue = 1;

    //Checks
    bool check = true;
    bool setupDone = false;

    /// <summary>
    /// Initializes all variables needed to run the script.
    /// </summary>
    void Start ()
    {
        GameObject GameController = GameObject.FindGameObjectWithTag("GameController");
        BR = GameController.GetComponent<BallReset>();
        GT = GameController.GetComponent<GameTimer>();
        BT = GameObject.Find("BreakTimer").GetComponent<BreakTimer>();
        Ref = GameObject.FindGameObjectWithTag("Referee").GetComponent<Referee>();
        Ref.PlayQuietPlease();

        CF = new CoinFlip();
        if (CF.Flip())
        {
            // Red team won toss, blue will get ball next half
            ball.GetComponent<Possession>().SetNextToGetBall(Possession.Team.blue);
            // Give the ball to the red team
            BR.placeBallRSC();
            Ref.PlayRedTeam();
            Ref.PlayCenter();
            //Ref.PlayPlay();
            BT.StartBreak(BreakTimer.Type.gameStart);
        }
        else
        {
            // Blue team won toss, red will get the ball next half
            ball.GetComponent<Possession>().SetNextToGetBall(Possession.Team.red);
            // Give the ball to the blue team
            BR.placeBallBSC();
            Ref.PlayBlueTeam();
            Ref.PlayCenter();
            //Ref.PlayPlay();
            BT.StartBreak(BreakTimer.Type.gameStart);
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
