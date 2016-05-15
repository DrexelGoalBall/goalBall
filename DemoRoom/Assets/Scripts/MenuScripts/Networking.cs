using UnityEngine;
using System.Collections;
using MenuTools;
/// <summary>
///     Sets up the Networking Menu for user interaction
/// </summary>
public class Networking : MenuLogic 
{
    // Reference to NetworkManager_Custom script
    public NetworkManager_Custom networkManager;

    // Reference to JoinGameList script
    public JoinGameList joinGameList;

    /// <summary>
    ///     Updates the menu logic with the functions for this menu
    /// </summary>
	void Update () 
    {
        // Do not accept input, while join game list is open
        if (!joinGameList.Displayed)
        {
            directionalMenuLogic(Left, Right, Up, Down);
        }
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
		// Show the join game list
        joinGameList.Displayed = true;
        // Stop any audio from playing
        StopAudio();
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
		// Try to start a game on the Goalball server
        networkManager.GoalballServer();
	}
}
