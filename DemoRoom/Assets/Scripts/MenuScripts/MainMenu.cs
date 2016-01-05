using UnityEngine;
using System.Collections;
using MenuTools;

// Used to navigate and manage the main menu
public class MainMenu : MenuLogic {

	void Update () {
		directionalMenuLogic(Left, Right, Up, Down);
	}

	new private void Left ()
	{
		Application.LoadLevel("Tutorial");
	}

	new private void Right ()
	{
		Application.LoadLevel("Exit");
	}

	new private void Up ()
	{
		Application.LoadLevel("Settings");
	}

	new private void Down ()
	{
		Application.LoadLevel("Networking");
	}
}
