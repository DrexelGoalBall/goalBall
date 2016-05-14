using UnityEngine;
using System.Collections;
using MenuTools;

/// <summary>
///     Sets up the Main menu for user interaction
/// </summary>
public class MainMenu : MenuLogic 
{
    /// <summary>
    ///     Updates the menu logic with the functions for this menu
    /// </summary>
	void Update() 
    {
        PlayInstructionsAfterDelay();
		directionalMenuLogic(Left, Right, Up, Down);
	}

    /// <summary>
    ///     Left menu option is selected, load Tutorial menu
    /// </summary>
	new private void Left()
	{
		Application.LoadLevel("Exit");
	}

    /// <summary>
    ///     Right menu option is selected, load Exit menu
    /// </summary>
	new private void Right()
	{
		Application.LoadLevel("NetworkingMenuBasic");
	}

    /// <summary>
    ///     Up menu option is selected, load Settings menu
    /// </summary>
	new private void Up()
	{
		Application.LoadLevel("Tutorial");
	}

    /// <summary>
    ///     Down menu option is selected, load Networking menu
    /// </summary>
	new private void Down()
	{
//		Application.LoadLevel("Networking");
	}
}
