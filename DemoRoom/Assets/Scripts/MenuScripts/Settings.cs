using UnityEngine;
using System.Collections;
using MenuTools;
/// <summary>
///     Sets up the Settings menu for user interaction
/// </summary>
public class Settings : MenuLogic 
{
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
	new private void Left()
	{
		// Undefined
	}

    /// <summary>
    ///     Right menu option is selected
    /// </summary>
	new private void Right()
	{
		// Undefined
	}

    /// <summary>
    ///     Up menu option is selected
    /// </summary>
	new private void Up()
	{
		// Undefined
	}

    /// <summary>
    ///     Down menu option is selected, load Main menu
    /// </summary>
	new private void Down()
	{
		Application.LoadLevel("MainMenu");
	}
}
