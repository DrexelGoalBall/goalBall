﻿using UnityEngine;
using System.Collections;

public class PlayerInputController : MonoBehaviour {
    /// <summary>
    /// This script will manage all of the inputs for the player for any script that requires player input
    /// This is being done so that all player action scripts can be kept on while not having to worry about
    /// controlling other players due to network games.
    /// </summary>

    public string HorizontalMove = "Horizontal";
    public string VerticalMove = "Vertical";
    public string Catch = "Catch";
    public string Throw = "Throw";
    public string horizontalAim = "horizontalAim";
    public string verticalAim = "verticalAim";
    public string DiveButton = "Dive";
    public string ResetAim = "ResetAim";

    private GoalBallPlayerMovementV1 PlayerMovement;
    private CatchThrowV2 CatchThrow;
    private Dive dive;

    /// <summary>
    /// Initializes the variables and objects that are needed by this script.
    /// </summary>
    void Start ()
    {
        PlayerMovement = GetComponent<GoalBallPlayerMovementV1>();
        CatchThrow = GetComponent<CatchThrowV2>();
        dive = GetComponent<Dive>();
	}

    /// <summary>
    /// Detects when the player uses an input on the controller and sends a message to the scripts that are affected by this change.
    /// </summary>
    void Update ()
    {
        PlayerMovement.Move(HorizontalMove, VerticalMove);
        CatchThrow.Aim(horizontalAim, verticalAim);

        if (Input.GetButtonDown(Catch))
        {
            CatchThrow.CatchBall();
        }

        if (Input.GetButtonDown(Throw))
        {
            CatchThrow.ChargeBall();
        }
        if (Input.GetButtonUp(Throw))
        {
            CatchThrow.ThrowBall();
        }
        if (Input.GetButtonDown(DiveButton))
        {
            dive.DivePressed();
        }
        if (Input.GetButtonDown(ResetAim))
        {
            CatchThrow.ResetAim();
        }
    }
}
