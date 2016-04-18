using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>
///     Stops the action for a specific amount of time and conveys necessary information for the given type of break
/// </summary>
public class BreakTimer : NetworkBehaviour 
{
    // References to necessary objects and scripts
    public GameObject ball;
    private Referee referee;
    private GameEnd gameEnd;

    // Enumeration of the different types of breaks that can occur
    // Note: Since the BreakTimer must be attached to object at run-time, need to use enums instead of abstraction
    public enum Type
    {
        none,
        gameStart,
        halftime,
        overtime,
        gameEnd,
        goal,
        foul,
    }

    // Flag to sync clients with from server to tell when a break is occurring
    [SyncVar(hook = "BreakChange")] public bool onBreak = false;
    
    // The type of break that is currently taking place
    private Type currentBreakType = Type.none;

    // Timer to keep track how much time is left in break
    private Timer timer;

    // Length of the different types of breaks
    public int gameStartBreakLength = 0;
    public int halftimeBreakLength = 10;
    public int overtimeBreakLength = 10;
    public int gameEndBreakLength = 33;
    public int goalBreakLength = 2;
    public int foulBreakLength = 2;

    // Debugging purposes
    public bool debug = false;
    public Text breakText;

    /// <summary>
    ///     Retrieve the necessary object and script references
    /// </summary>
	void Start()
    {
        referee = GameObject.FindGameObjectWithTag("Referee").GetComponent<Referee>();
        gameEnd = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameEnd>();

        timer = gameObject.GetComponent<Timer>();
	}

    /// <summary>
    ///     Checks if the break has ended or displays the amount of time left on the current break
    /// </summary>
	void Update()
    {
        if (onBreak && (timer.isPaused() || timer.getTime() <= 0))
        {
            // When timer runs out, break is over
            EndBreak();
        }
        else
        {
            // Display how much time is left on the break
            breakText.text = timer.getTimeString();
        }
	}

    /// <summary>
    ///     On the server, initiate the given type of break
    /// </summary>
    /// <param name="type">The type of break that is necessary to start</param>
    public void StartBreak(Type type)
    {
        if (isServer)
        {
            onBreak = true;
            currentBreakType = type;
            // Prevent the ball from moving
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            // Start the timer
            timer.SetLengthOfTimer(GetLengthFromType(currentBreakType));
            timer.Resume();
        }
    }

    /// <summary>
    ///     On the server, end the current break
    /// </summary>
    private void EndBreak()
    {
        if (isServer)
        {
            onBreak = false;
            
            // Stop the timer
            timer.Pause();
            timer.SetLengthOfTimer(0);

            // Execute the necessary end of break actions for this type of break
            switch (currentBreakType)
            {
                case Type.gameStart:
                case Type.goal:
                case Type.foul:
                    referee.PlayPlay();
                    break;
                case Type.halftime:
                case Type.overtime:
                    if (ball.GetComponent<Possession>().HasPossessionOfBall() == Possession.Team.red)
                        referee.PlayRedTeam();
                    else
                        referee.PlayBlueTeam();
                    referee.PlayPlay();
                    break;
                case Type.gameEnd:
                    gameEnd.LeaveGame();
                    break;
                case Type.none:
                default:
                    break;
            }

            // Allow ball to move
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

            // Reset the break type
            currentBreakType = Type.none;
        }
    }

    /// <summary>
    ///     Allows outside scripts to check if there is a break occurring
    /// </summary>
    public bool OnBreak()
    {
        return onBreak;
    }

    /// <summary>
    ///     On the clients, change the break flag and the ball constraints accordingly
    /// </summary>
    /// <param name="breakStarted">Whether the break has just started or ended</param>
    private void BreakChange(bool breakStarted)
    {
        if (breakStarted)
        {
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }

        onBreak = breakStarted;
    }

    /// <summary>
    ///     Retrieve the length of time for the given break type
    /// </summary>
    /// <param name="type">Type of break</param>
    /// <returns>Integer amount of time in seconds</returns>
    private int GetLengthFromType(Type type)
    {
        int length = 0;

        switch (type)
        {
            case Type.gameStart:
                length = gameStartBreakLength;
                break;
            case Type.halftime:
                length = halftimeBreakLength;
                break;
            case Type.overtime:
                length = overtimeBreakLength;
                break;
            case Type.gameEnd:
                length = gameEndBreakLength;
                break;
            case Type.goal:
                length = goalBreakLength;
                break;
            case Type.foul:
                length = foulBreakLength;
                break;
            case Type.none:
            default:
                break;
        }

        return length;
    }
}
