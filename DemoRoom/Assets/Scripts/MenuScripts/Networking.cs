using UnityEngine;
using System.Collections;
using MenuTools;
/// <summary>
///     Sets up the Networking Menu for user interaction
/// </summary>
public class Networking : MenuLogic 
{
    /// <summary>
    ///     Updates the menu logic with the functions for this menu
    /// </summary>
	void Update () 
    {
		directionalMenuLogic(Left, Right, Up, Down);
	}

    /// <summary>
    ///     Left menu option is selected
    /// </summary>
	new private void Left()
	{
		Application.LoadLevel("MainMenu");
	}

    /// <summary>
    ///     Right menu option is selected
    /// </summary>
	new private void Right()
	{
		// Navigate to Joining Menu
	}

    /// <summary>
    ///     Up menu option is selected, load Main menu
    /// </summary>
	new private void Up()
	{
		// Navigate to Quick Match Menu
	}

    /// <summary>
    ///     Down menu option is selected
    /// </summary>
	new private void Down ()
	{
		// Navigate to Host Menu
	}
}
