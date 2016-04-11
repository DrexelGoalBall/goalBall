using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameEnd : NetworkBehaviour
{
    // 
    private enum Winner
    {
        tie,
        red,
        blue,
    }

    // 
    private GameTimer gameTimer;
    private ScoreKeeper scoreKeeper;
    private Referee referee;
    private BreakTimer breakTimer;

    // 
    public Text winnerText;
    public Image gameOverBackground;

    // 
    private Winner winner = Winner.tie;

    // 
    public string redWins = "RED TEAM WINS!";
    public string blueWins = "BLUE TEAM WINS!";

    // 
    public Color redTeamColor = Color.red;
    public Color blueTeamColor = Color.blue;

    // 
    public string skipInput = "Throw";

    // 
    private bool initialized = false;
	
    /// <summary>
    /// Initializes all variables needed to run the script.
    /// </summary>
    void Start()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        gameTimer = gameController.GetComponent<GameTimer>();
        scoreKeeper = gameController.GetComponent<ScoreKeeper>();
        referee = GameObject.FindGameObjectWithTag("Referee").GetComponent<Referee>();
        breakTimer = GameObject.Find("BreakTimer").GetComponent<BreakTimer>();
    }

	/// <summary>
	/// Displays and announces who has won the game
	/// </summary>
	void Update()
    {
	    if (gameTimer.GameHasEnded())
        {
            if (Input.GetButtonDown(skipInput))
            {
                ReturnToMenu();
            }

            if (!initialized)
            {
                breakTimer.StartBreak(BreakTimer.Type.gameEnd);

                if (scoreKeeper.RedScore() > scoreKeeper.BlueScore())
                {
                    winnerText.text = redWins;
                    winnerText.color = redTeamColor;
                    winner = Winner.red;
                }
                else if (scoreKeeper.BlueScore() > scoreKeeper.RedScore())
                {
                    winnerText.text = blueWins;
                    winnerText.color = blueTeamColor;
                    winner = Winner.blue;
                }
                else
                {
                    winnerText.text = "TIE";
                    winnerText.color = Color.white;
                    winner = Winner.tie;
                }

                gameOverBackground.enabled = true;

                initialized = true;
            }
            else
            {
                if (!referee.refereeSpeaking())
                {
                    referee.PlayGameOver();
                    
                    switch(winner)
                    {
                        case Winner.red:
                            referee.PlayRedTeam();
                            referee.PlayWins();
                            break;
                        case Winner.blue:
                            referee.PlayBlueTeam();
                            referee.PlayWins();
                            break;
                        case Winner.tie:
                        default:
                            break;
                    }

                    referee.PlayFinalScore();
                    
                    // Read red score
                    referee.PlayRedTeam();
                    referee.ReadScore(scoreKeeper.RedScore());

                    // Read blue score
                    referee.PlayBlueTeam();
                    referee.ReadScore(scoreKeeper.BlueScore());

                    referee.PlayReturnToMenu();
                }
            }
        }
	}

    /// <summary>
    /// Loads the offline menu scene to exit the game.
    /// </summary>
    public void ReturnToMenu()
    {
        if (!isServer && isClient)
        {
            NetworkManager_Custom.singleton.StopClient();
        }
    }
}
