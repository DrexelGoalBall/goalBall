using UnityEngine;
using System.Collections;
using MenuTools;

// Used to navigate and manage the networking menu
public class Exit : MenuLogic {

	void Update () {
		directionalMenuLogic(Left, Right, Up, Down);
	}

	new private void Left ()
	{
		Application.LoadLevel("MainMenu");
	}

	new private void Right ()
	{
		Application.Quit();

	}

	new private void Up ()
	{
		// Undefined
	}

	new private void Down ()
	{
		// Undefined
	}
}
