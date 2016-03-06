using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BreakTimer : NetworkBehaviour 
{
    public enum Type
    {
        none,
        gameStart,
        halftime,
        overtime,
        goal,
        foul,
    }

    // Hook is not working for some reason, getting around with local flag for now
    //[SyncVar(hook = "BreakChange")]
    [SyncVar]
    public bool onBreak = false;
    private bool clientPreviousBreak;
    
    private Type currentBreakType = Type.none;

    private Timer timer;

    public int gameStartBreakLength = 0;
    public int halftimeBreakLength = 10;
    public int overtimeBreakLength = 10;
    public int goalBreakLength = 2;
    public int foulBreakLength = 2;

    // Debugging purposes
    public bool debug = false;
    public Text breakText;

	// Use this for initialization
	void Start()
    {
        timer = gameObject.AddComponent<Timer>();

        clientPreviousBreak = onBreak;
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
            if (debug)
                breakText.text = timer.getTimeString();
        }

        // Since hook is not working, here is a work around to check for change in syncvar flag
        if (isClient)
        {
            if (clientPreviousBreak != onBreak)
            {
                BreakChange(onBreak);
                clientPreviousBreak = onBreak;
            }
        }
	}

    public void StartBreak(Type type)
    {
        if (isServer)
        {
            onBreak = true;
            currentBreakType = type;
            GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
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

            GameObject ball = GameObject.FindGameObjectWithTag("Ball");
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            Referee referee = GameObject.FindGameObjectWithTag("Referee").GetComponent<Referee>();         
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
                case Type.none:
                default:
                    break;
            }

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
            GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
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
