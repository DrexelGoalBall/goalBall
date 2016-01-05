using UnityEngine;
using System.Collections;
using MenuTools;

// Used to navigate and manage the networking menu
public class Networking : MenuLogic {

	void Update () {
		directionalMenuLogic(Left, Right, Up, Down);
	}

	new private void Left ()
	{
		// Navigate to Quick Match Menu
	}

	new private void Right ()
	{
		// Navigate to Joining Menu
	}

	new private void Up ()
	{
		Application.LoadLevel("MainMenu");
	}

	new private void Down ()
	{
		// Navigate to Host Menu
	}
}
