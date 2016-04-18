using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>
///     Announces and displays who has won the game, allows players to exit to menu
/// </summary>
public class GameEnd : NetworkBehaviour
{
    // Enumeration on the possible results of the game
    private enum Winner
    {
        tie,
        red,
        blue,
    }

    // References to necessary scripts
    private GameTimer gameTimer;
    private ScoreKeeper scoreKeeper;
    private Referee referee;
    private BreakTimer breakTimer;

    // UI Text component to update with winner
    public Text winnerText;
    // Panel to display containing winner text and other necessary components
    public GameObject gameOverPanel;

    // The result of the game
    private Winner winner = Winner.tie;

    // Strings to display if either team wins
    public string redWins = "RED TEAM WINS!";
    public string blueWins = "BLUE TEAM WINS!";

    // Colors to change winner text for the winning team
    public Color redTeamColor = Color.red;
    public Color blueTeamColor = Color.blue;

    // Input string of button to press to return to the menu
    public string returnToMenuInput = "Throw";

    // 
    private bool initialized = false;
	
    /// <summary>
    ///     Retrieve the necessary object and script references
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
	///     When the game ends, repeatedly announce who has won and allow player to exit to the menu
	/// </summary>
	void Update()
    {
	    if (gameTimer.GameHasEnded())
        {
            if (Input.GetButtonDown(returnToMenuInput))
            {
                // Player wants to go to the menu now
                LeaveGame();
            }

            if (!initialized)
            {
                // Start the end game break which will automatically go to the menu when it ends
                breakTimer.StartBreak(BreakTimer.Type.gameEnd);

                // Set the winner and update the UI accordingly
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

                gameOverPanel.SetActive(true);

                initialized = true;
            }
            else
            {
                // Check if the referee is currently announcing the results
                if (!referee.refereeSpeaking())
                {
                    // Announce that the game has ended
                    referee.PlayGameOver();
                    
                    // Announce the winner
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

                    // Announce the final scores
                    referee.PlayFinalScore();
                    // Red team score
                    referee.PlayRedTeam();
                    referee.ReadScore(scoreKeeper.RedScore());
                    // Blue team score
                    referee.PlayBlueTeam();
                    referee.ReadScore(scoreKeeper.BlueScore());

                    // Announce how to exit to menu
                    referee.PlayReturnToMenu();
                }
            }
        }
	}

    /// <summary>
    ///     Exits the current networked game
    /// </summary>
    public void LeaveGame()
    {
        if (isServer)
        {
            // Server does not need to keep game open
            //Application.Quit();
        }
        else
        {
            if (isClient)
            {
                // Client should return to the offline scene
                NetworkManager_Custom.singleton.StopClient();
            }
        }
    }
}
