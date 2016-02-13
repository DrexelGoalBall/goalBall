using UnityEngine;
using System.Collections;
using MenuTools;

// Used to navigate and manage the tutorial menu
public class Tutorial : MenuLogic 
{
    /// <summary>
    ///     Sets up the Tutorial menu for user interaction
    /// </summary>

    /// <summary>
    ///     Updates the menu logic with the functions for this menu
    /// </summary>
	void Update() 
    {
		directionalMenuLogic(Left, Right, Up, Down);
	}

    /// <summary>
    ///     Left menu option is selected
    /// </summary>
	new private void Left ()
	{
		// Undefined
	}

    /// <summary>
    ///     Right menu option is selected, load Main menu
    /// </summary>
	new private void Right()
	{
		Application.LoadLevel("MainMenu");
	}

    /// <summary>
    ///     Up menu option is selected
    /// </summary>
	new private void Up()
	{
		// Undefined
	}

    /// <summary>
    ///     Down menu option is selected
    /// </summary>
	new private void Down()
	{
		// Undefined
	}
}
