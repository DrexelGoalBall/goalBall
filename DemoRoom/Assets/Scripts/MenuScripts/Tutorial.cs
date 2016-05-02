using UnityEngine;
using System.Collections;
using MenuTools;

/// <summary>
///     Sets up the Tutorial menu for user interaction
/// </summary>
// Used to navigate and manage the tutorial menu
public class Tutorial : MonoBehaviour 
{
	public string LeaveButton = "Cancel";

    /// <summary>
    ///     Updates the menu logic with the functions for this menu
    /// </summary>
	void Update() 
    {
        if (InputPlayers.player0.GetButtonDown(LeaveButton))
		{
			Application.LoadLevel("MainMenu");			
		}
	}
}
