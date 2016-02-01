using UnityEngine;
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

    private GoalBallPlayerMovementV1 PlayerMovement;
    private CatchThrowV2 CatchThrow;
    private Dive dive;
	// Update is called once per frame
	void Start ()
    {
        PlayerMovement = GetComponent<GoalBallPlayerMovementV1>();
        CatchThrow = GetComponent<CatchThrowV2>();
        dive = GetComponent<Dive>();
	}

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
    }
}
