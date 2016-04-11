using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BreakTimer : NetworkBehaviour 
{
    public GameObject ball;
    private Referee referee;
    private GameEnd gameEnd;

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

    [SyncVar(hook = "BreakChange")]
    public bool onBreak = false;
    
    private Type currentBreakType = Type.none;

    private Timer timer;

    public int gameStartBreakLength = 0;
    public int halftimeBreakLength = 10;
    public int overtimeBreakLength = 10;
    public int gameEndBreakLength = 33;
    public int goalBreakLength = 2;
    public int foulBreakLength = 2;

    // Debugging purposes
    public bool debug = false;
    public Text breakText;

	// Use this for initialization
	void Start()
    {
        referee = GameObject.FindGameObjectWithTag("Referee").GetComponent<Referee>();
        gameEnd = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameEnd>();

        timer = gameObject.GetComponent<Timer>();
	}
	
	// Update is called once per frame
	void Update()
    {
        if (onBreak && (timer.isPaused() || timer.getTime() <= 0))
        {
            EndBreak();
        }
        else
        {
            //if (debug && isServer)
                breakText.text = timer.getTimeString();
        }
	}

    public void StartBreak(Type type)
    {
        if (isServer)
        {
            onBreak = true;
            currentBreakType = type;
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            timer.SetLengthOfTimer(GetLengthFromType(currentBreakType));
            timer.Resume();
        }
    }

    private void EndBreak()
    {
        if (isServer)
        {
            onBreak = false;
            timer.Pause();
            timer.SetLengthOfTimer(0);

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
                    gameEnd.ReturnToMenu();
                    break;
                case Type.none:
                default:
                    break;
            }

            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

            currentBreakType = Type.none;
        }
    }

    public bool OnBreak()
    {
        return onBreak;
    }

    private void BreakChange(bool brk)
    {
        if (brk)
        {
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }

        onBreak = brk;
    }

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
