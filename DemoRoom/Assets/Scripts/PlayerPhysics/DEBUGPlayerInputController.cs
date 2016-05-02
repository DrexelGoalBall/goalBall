using UnityEngine;
using System.Collections;

/// <summary>
/// This script will manage all of the inputs for the player for any script that requires player input
/// This is being done so that all player action scripts can be kept on while not having to worry about
/// controlling other players due to network games.
/// </summary>
public class DEBUGPlayerInputController : MonoBehaviour 
{
    public string MoveHorizontal = "Move Horizontal";
    public string MoveVertical = "Move Vertical";
    public string LookHorizontal = "Look Horizontal";
    public string LookVertical = "Look Vertical";
    public string Catch = "Catch";
    public string Throw = "Throw";
    public string Dive = "Dive";
    public string ResetAim = "Reset Aim";
    public string ResetPosition = "Reset Position";

    private GoalBallPlayerMovementV1 PlayerMovement;
    private DEBUGCatchThrowV2 CatchThrow;
    private Dive dive;
    private GameTimer gameTimer;

    /// <summary>
    /// Initializes the variables and objects that are needed by this script.
    /// </summary>
    void Start ()
    {
        PlayerMovement = GetComponent<GoalBallPlayerMovementV1>();
        CatchThrow = GetComponent<DEBUGCatchThrowV2>();
        dive = GetComponent<Dive>();
	}

    /// <summary>
    /// Detects when the player uses an input on the controller and sends a message to the scripts that are affected by this change.
    /// </summary>
    void Update ()
    {
        PlayerMovement.Move(InputPlayers.player0.GetAxis(MoveHorizontal), InputPlayers.player0.GetAxis(MoveVertical));
        CatchThrow.Aim(InputPlayers.player0.GetAxis(LookHorizontal), InputPlayers.player0.GetAxis(LookVertical));

        if (InputPlayers.player0.GetButtonDown(Catch))
        {
            CatchThrow.CatchBall();
        }

        if (InputPlayers.player0.GetButtonDown(Throw))
        {
            CatchThrow.ChargeBall();
        }

        if (InputPlayers.player0.GetButtonUp(Throw))
        {
            CatchThrow.ThrowBall();
        }

        if (InputPlayers.player0.GetButtonDown(Dive))
        {
            dive.DivePressed();
        }

        if (InputPlayers.player0.GetButtonDown(ResetAim))
        {
            CatchThrow.ResetAim();
        }

        if (InputPlayers.player0.GetButtonDown(ResetPosition))
        {
            PlayerMovement.AutoMove();
        }
      
    }
}
