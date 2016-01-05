using UnityEngine;
using System.Collections;
using MenuTools;

// Used to navigate and manage the settings menu
public class Settings : MenuLogic {

	void Update () {
		directionalMenuLogic(Left, Right, Up, Down);
	}
	
	new private void Left ()
	{
		// Undefined
	}

	new private void Right ()
	{
		// Undefined
	}

	new private void Up ()
	{
		// Undefined
	}

	new private void Down ()
	{
		Application.LoadLevel("MainMenu");
	}
}
