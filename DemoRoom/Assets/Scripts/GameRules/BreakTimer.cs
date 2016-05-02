using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>
///     Stops the action for a specific amount of time and conveys necessary information for the given type of break
/// </summary>
public class BreakTimer : NetworkBehaviour 
{
    // Type of break that is currently being taken
    private GameBreak gameBreakType = null;

    // References to necessary objects and scripts
    public GameObject ball;

    // Flag to sync clients with from server to tell when a break is occurring
    [SyncVar(hook = "BreakChange")] public bool onBreak = false;

    // Timer to keep track how much time is left in break
    private Timer timer;

    // UI text to display the amount of time left for this break
    public Text breakText;

    /// <summary>
    ///     Retrieve the necessary object and script references
    /// </summary>
	void Start()
    {
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
    public void StartBreak(GameBreak type)
    {
        if (isServer)
        {
            onBreak = true;
            // Prevent the ball from moving
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            // Set the type of break that is happening and perform any necessary actions for it
            gameBreakType = type;
            gameBreakType.StartOfBreakActions();
            timer.SetLengthOfTimer(gameBreakType.GetBreakLength());
            // Start the countdown
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

            // Perform any necessary actions for the end of this break type
            gameBreakType.EndOfBreakActions();
            gameBreakType = null;

            // Allow ball to move
            ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
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
}
