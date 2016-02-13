using UnityEngine;
using System.Collections;
using MenuTools;

/// <summary>
///     Sets up the Exit menu for user interaction
/// </summary>
public class Exit : MenuLogic 
{
    /// <summary>
    ///     Updates the menu logic with the functions for this menu
    /// </summary>
	void Update() 
    {
		directionalMenuLogic(Left, Right, Up, Down);
	}

    /// <summary>
    ///     Left menu option is selected, load Main menu
    /// </summary>
	new private void Left()
	{
		Application.LoadLevel("MainMenu");
	}

    /// <summary>
    ///     Right menu option is selected, close game
    /// </summary>
	new private void Right()
	{
		Application.Quit();

	}

    /// <summary>
    ///     Up menu option is selected
    /// </summary>
	new private void Up ()
	{
		// Undefined
	}

    /// <summary>
    ///     Down menu option is selected
    /// </summary>
	new private void Down ()
	{
		// Undefined
	}
}
