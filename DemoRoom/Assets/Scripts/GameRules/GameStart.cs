using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Linq;

/// <summary>
/// This script starts up the game and activates everything that needs to be activated for a game to start.
/// </summary>
public class GameStart : NetworkBehaviour
{
    //Scripts
    private CoinFlip CF;

    //Neccessary Components
    private BallReset BR;
    private Referee Ref;
    private GameTimer GT;

    //Ball
    public GameObject ball;

    //Checks
    bool setupDone = false;

    /// <summary>
    /// Initializes all variables needed to run the script.
    /// </summary>
    void Start ()
    {
        GameObject GameController = GameObject.FindGameObjectWithTag("GameController");
        BR = GameController.GetComponent<BallReset>();
        GT = GameController.GetComponent<GameTimer>();
        Ref = GameObject.FindGameObjectWithTag("Referee").GetComponent<Referee>();
        
        CF = new CoinFlip();
        if (CF.Flip())
        {
            // Red team won toss, blue will get ball next half
            ball.GetComponent<Possession>().SetNextToGetBall(Possession.Team.blue);
            // Give the ball to the red team
            BR.placeBallRSC();
        }
        else
        {
            // Blue team won toss, red will get the ball next half
            ball.GetComponent<Possession>().SetNextToGetBall(Possession.Team.red);
            // Give the ball to the blue team
            BR.placeBallBSC();
        }
        Ref.PlayQuietPlease();

        setupDone = true;
    }

    /// <summary>
    /// Checks if the game is ready to start and the referee has finished speaking.
    /// </summary>
    void Update()
    {
        if (isServer && !GT.GameHasStarted() && setupDone)
        {
            // Allow server to start the game
            if (Input.GetButtonDown("Throw"))
            {
                GT.StartGame();
                return;
            }

            GameObject[] redPlayers = GameObject.FindGameObjectsWithTag("RedPlayer");
            GameObject[] bluePlayers = GameObject.FindGameObjectsWithTag("BluePlayer");
            GameObject[] players = redPlayers.Concat(bluePlayers).ToArray();

            // Check if any of the players want to start the game
            foreach (GameObject player in players)
            {
                if (player.GetComponent<Player_Ready>().isReady())
                {
                    GT.StartGame();
                    return;
                }
            }
        }
    }
}
