using UnityEngine;
using System.Collections;
using MenuTools;

// Used to navigate and manage the tutorial menu
public class Tutorial : MenuLogic {

	void Update () {
		directionalMenuLogic(Left, Right, Up, Down);
	}

	new private void Left ()
	{
		// Undefined
	}

	new private void Right ()
	{
		Application.LoadLevel("MainMenu");
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
